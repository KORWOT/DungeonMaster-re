using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Balance.Editor
{
    /// <summary>
    /// ScriptableObject 데이터의 밸런싱을 위한 범용 에디터 윈도우의 추상 기반 클래스입니다.
    /// 이 클래스는 전체적인 창의 구조와 공통 기능을 제공하며, 세부적인 UI 드로잉과 정렬/필터링 로직은 파생 클래스에 위임합니다.
    /// </summary>
    /// <typeparam name="T">편집할 ScriptableObject의 타입</typeparam>
    public abstract class BalanceEditorWindow<T> : EditorWindow where T : ScriptableObject
    {
        protected List<T> allItems;
        private List<T> _filteredItems; // 필터링된 결과를 캐시하여 성능 향상
        private Vector2 _scrollPosition;

        private string _searchQuery = "";
        protected string _currentSortKey;
        protected bool _isAscending = true;

        // 일괄 편집을 위한 선택된 아이템 목록
        protected readonly HashSet<T> _selectedItems = new HashSet<T>();

        #region Abstract & Virtual Methods
        /// <summary>
        /// 파생 클래스에서 새 에셋을 저장할 기본 경로를 지정해야 합니다. (예: "Assets/Data/Buffs")
        /// </summary>
        protected abstract string GetNewAssetPath();

        /// <summary>
        /// 파생 클래스에서 테이블의 헤더 UI를 그립니다.
        /// </summary>
        protected abstract void DrawHeader();

        /// <summary>
        /// 파생 클래스에서 테이블의 각 행(Row) UI를 그립니다.
        /// </summary>
        protected abstract void DrawRow(T item);

        /// <summary>
        /// 파생 클래스에서 정렬 로직을 구현합니다.
        /// </summary>
        protected abstract IOrderedEnumerable<T> GetSorted(IEnumerable<T> items);

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
        #endregion

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
            ApplyFiltersAndSorting();
        }

        protected virtual void OnGUI()
        {
            GUILayout.Label($"{typeof(T).Name} Balance Sheet", EditorStyles.boldLabel);

            DrawToolbarAndFilters();
            DrawBulkEditSection();
            
            EditorGUILayout.Space();

            DrawTable();
        }

        private void DrawToolbarAndFilters()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            
            // --- 검색창 (가장 안정적인 방식으로 수정) ---
            EditorGUI.BeginChangeCheck();
            _searchQuery = GUILayout.TextField(_searchQuery, EditorStyles.toolbarSearchField, GUILayout.MaxWidth(250));
            if (GUILayout.Button("X", EditorStyles.toolbarButton, GUILayout.Width(22)))
            {
                _searchQuery = string.Empty;
                GUI.FocusControl(null); // 포커스 해제
            }

            if (EditorGUI.EndChangeCheck())
            {
                ApplyFiltersAndSorting();
            }

            // --- 고급 필터 ---
            EditorGUI.BeginChangeCheck();
            DrawFilters();
            if (EditorGUI.EndChangeCheck())
            {
                ApplyFiltersAndSorting(); // 필터가 변경될 때만 필터링 다시 적용
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
        }
        
        private void DrawBulkEditSection()
        {
            if (_selectedItems.Count > 0)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Label($"Bulk Edit ({_selectedItems.Count} items selected)", EditorStyles.boldLabel);
                DrawBulkEditControls();
                EditorGUILayout.EndVertical();
            }
        }

        private void DrawTable()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            DrawHeader();
            EditorGUILayout.EndHorizontal();

            DrawRows();

            EditorGUILayout.EndScrollView();
        }

        private void DrawRows()
        {
            if (_filteredItems == null) return;

            foreach (T item in _filteredItems)
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

                // --- 행 데이터 그리기 ---
                EditorGUI.BeginChangeCheck();
                DrawRow(item);
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(item);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void CreateNewAsset()
        {
            T newAsset = CreateInstance<T>();
            string path = GetNewAssetPath();

            // --- 폴더 생성 (더 안정적인 방식으로 변경) ---
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{path}/New {typeof(T).Name}.asset");

            AssetDatabase.CreateAsset(newAsset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newAsset;
            LoadAllAssets();
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
                ApplyFiltersAndSorting();
            }
        }

        private void ApplyFiltersAndSorting()
        {
            IEnumerable<T> items = allItems;

            // 검색어 필터링
            if (!string.IsNullOrEmpty(_searchQuery))
            {
                items = items.Where(item => item.name.IndexOf(_searchQuery, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            
            // 고급 필터링
            items = ApplyAdvancedFilters(items);

            // 정렬
            var sorted = GetSorted(items);
            if (sorted != null)
            {
                _filteredItems = sorted.ToList();
            }
            else
            {
                _filteredItems = items.ToList();
            }
        }
    }
}
