using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public abstract class BalanceEditorWindow<T> : EditorWindow where T : ScriptableObject
{
    protected List<T> allItems;
    private Vector2 _scrollPosition;
    
    private string _searchQuery = "";
    protected string _currentSortKey;
    protected bool _isAscending = true;
    
    // 일괄 편집을 위한 선택된 아이템 목록
    protected readonly HashSet<T> _selectedItems = new HashSet<T>();

    /// <summary>
    /// 파생 클래스에서 새 에셋을 저장할 기본 경로를 지정해야 합니다. (예: "Assets/Data/Buffs")
    /// </summary>
    protected abstract string GetNewAssetPath();

    /// <summary>
    /// (선택적) 파생 클래스에서 고급 필터 UI를 그립니다.
    /// </summary>
    protected virtual void DrawFilters() { }

    /// <summary>
    /// (선택적) 파생 클래스에서 일괄 편집 UI를 그립니다.
    /// </summary>
    protected virtual void DrawBulkEditControls() { }

    /// <summary>
    /// 파생 클래스에서 고급 필터링 로직을 구현합니다.
    /// </summary>
    protected abstract IEnumerable<T> ApplyAdvancedFilters(IEnumerable<T> items);

    protected virtual void OnEnable()
    {
        LoadAllAssets();
    }

    protected void LoadAllAssets()
    {
        _selectedItems.Clear();
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        allItems = new List<T>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            T item = AssetDatabase.LoadAssetAtPath<T>(path);
            if (item != null)
            {
                allItems.Add(item);
            }
        }
        
        _currentSortKey = "Name";
        SortItems();
    }

    protected virtual void OnGUI()
    {
        GUILayout.Label($"{typeof(T).Name} Balance Sheet", EditorStyles.boldLabel);

        // --- 상단 툴바 ---
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        _searchQuery = EditorGUILayout.TextField(_searchQuery, GUI.skin.FindStyle("ToolbarSeachTextField"), GUILayout.MaxWidth(250));
        if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
        {
            _searchQuery = "";
            GUI.FocusControl(null);
        }
        
        DrawFilters();
        
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Create New", EditorStyles.toolbarButton))
        {
            CreateNewAsset();
        }

        if (GUILayout.Button("Reload All", EditorStyles.toolbarButton))
        {
            LoadAllAssets();
        }
        EditorGUILayout.EndHorizontal();
        
        // --- 일괄 편집 UI ---
        if (_selectedItems.Count > 0)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label($"Bulk Edit ({_selectedItems.Count} items selected)", EditorStyles.boldLabel);
            DrawBulkEditControls();
            EditorGUILayout.EndVertical();
        }
        
        EditorGUILayout.Space();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        DrawHeader();
        EditorGUILayout.EndHorizontal();
        
        DrawRows();

        EditorGUILayout.EndScrollView();
    }
    
    private void CreateNewAsset()
    {
        T newAsset = CreateInstance<T>();
        string path = GetNewAssetPath();
        
        if (!AssetDatabase.IsValidFolder(path))
        {
            string parentFolder = "Assets";
            string[] folders = path.Split('/');
            for(int i = 1; i < folders.Length; i++)
            {
                string currentPath = parentFolder + "/" + folders[i];
                if (!AssetDatabase.IsValidFolder(currentPath))
                {
                    AssetDatabase.CreateFolder(parentFolder, folders[i]);
                }
                parentFolder = currentPath;
            }
        }
        
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{path}/New {typeof(T).Name}.asset");

        AssetDatabase.CreateAsset(newAsset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newAsset;
        LoadAllAssets();
    }

    private void DrawRows()
    {
        if (allItems == null) return;
        
        var filteredItems = string.IsNullOrEmpty(_searchQuery)
            ? allItems
            : allItems.Where(item => item.name.IndexOf(_searchQuery, StringComparison.OrdinalIgnoreCase) >= 0);
            
        filteredItems = ApplyAdvancedFilters(filteredItems);

        foreach (T item in filteredItems)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            
            // --- 선택 체크박스 ---
            bool isSelected = _selectedItems.Contains(item);
            EditorGUI.BeginChangeCheck();
            isSelected = EditorGUILayout.Toggle(isSelected, GUILayout.Width(20));
            if (EditorGUI.EndChangeCheck())
            {
                if (isSelected) _selectedItems.Add(item);
                else _selectedItems.Remove(item);
            }

            EditorGUI.BeginChangeCheck();
            DrawRow(item);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(item);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
    
    protected void DrawSortableHeader(string title, string key, params GUILayoutOption[] options)
    {
        string arrow = "";
        if (_currentSortKey == key)
        {
            arrow = _isAscending ? " ▲" : " ▼";
        }

        if (GUILayout.Button(title + arrow, EditorStyles.toolbarButton, options))
        {
            if (_currentSortKey == key)
            {
                _isAscending = !_isAscending;
            }
            else
            {
                _currentSortKey = key;
                _isAscending = true;
            }
            SortItems();
        }
    }

    private void SortItems()
    {
        if (allItems == null || string.IsNullOrEmpty(_currentSortKey)) return;
        var sorted = GetSorted(allItems);
        if (sorted != null)
        {
            allItems = sorted.ToList();
        }
    }
    
    protected abstract void DrawHeader();
    protected abstract void DrawRow(T item);
    protected abstract IOrderedEnumerable<T> GetSorted(IEnumerable<T> items);
}
