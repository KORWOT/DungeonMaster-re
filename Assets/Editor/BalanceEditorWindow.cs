using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ScriptableObject 기반 데이터의 밸런싱 툴을 만들기 위한 제네릭 에디터 윈도우 추상 클래스입니다.
/// </summary>
/// <typeparam name="T">편집할 ScriptableObject의 타입</typeparam>
public abstract class BalanceEditorWindow<T> : EditorWindow where T : ScriptableObject
{
    protected List<T> allItems;
    private Vector2 _scrollPosition;
    
    // 정렬 관련 변수
    protected string _currentSortKey;
    protected bool _isAscending = true;

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
        
        // 초기 정렬 (이름순)
        _currentSortKey = "Name";
        SortItems();
    }

    protected virtual void OnGUI()
    {
        GUILayout.Label($"{typeof(T).Name} Balance Sheet", EditorStyles.boldLabel);

        if (GUILayout.Button($"Reload All {typeof(T).Name}s"))
        {
            LoadAllAssets();
        }

        EditorGUILayout.Space();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        // 헤더 그리기
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        DrawHeader();
        EditorGUILayout.EndHorizontal();
        
        // 데이터 행 그리기
        if (allItems != null)
        {
            foreach (T item in allItems)
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

        EditorGUILayout.EndScrollView();
    }
    
    /// <summary>
    /// 클릭 가능한 정렬 헤더를 그립니다.
    /// </summary>
    /// <param name="title">헤더에 표시될 이름</param>
    /// <param name="key">이 헤더의 정렬 기준 키</param>
    /// <param name="options">레이아웃 옵션</param>
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

    /// <summary>
    /// 파생 클래스에서 테이블의 헤더를 그리기 위해 이 메서드를 구현해야 합니다.
    /// </summary>
    protected abstract void DrawHeader();

    /// <summary>
    /// 파생 클래스에서 각 데이터 행을 그리기 위해 이 메서드를 구현해야 합니다.
    /// </summary>
    /// <param name="item">그려질 데이터 아이템</param>
    protected abstract void DrawRow(T item);

    /// <summary>
    /// 파생 클래스에서 정렬 로직을 제공하기 위해 이 메서드를 구현해야 합니다.
    /// </summary>
    /// <param name="items">정렬할 아이템 목록</param>
    /// <returns>정렬된 아이템 목록</returns>
    protected abstract IOrderedEnumerable<T> GetSorted(IEnumerable<T> items);
}
