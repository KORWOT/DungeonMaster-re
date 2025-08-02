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
    
    /// <summary>
    /// 파생 클래스에서 새 에셋을 저장할 기본 경로를 지정해야 합니다. (예: "Assets/Data/Buffs")
    /// </summary>
    protected abstract string GetNewAssetPath();

    protected virtual void OnEnable()
    {
        LoadAllAssets();
    }

    protected void LoadAllAssets()
    {
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

        // 폴더가 없으면 생성
        if (!AssetDatabase.IsValidFolder(path))
        {
            // "Assets"를 기준으로 하위 폴더 경로를 만듭니다.
            string parentFolder = "Assets";
            string[] folders = path.Split('/');
            // 첫번째(Assets)는 건너뛰고 시작
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
        
        // 중복되지 않는 파일 경로 생성
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{path}/New {typeof(T).Name}.asset");

        AssetDatabase.CreateAsset(newAsset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        // 생성된 에셋을 선택하고, 목록을 새로고침
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

        foreach (T item in filteredItems)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
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
