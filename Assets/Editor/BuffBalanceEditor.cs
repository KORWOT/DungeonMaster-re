using System.Collections.Generic;
using System.Linq;
using Combat.Buffs;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Buff 데이터 밸런싱을 위한 에디터 윈도우입니다.
/// </summary>
public class BuffBalanceEditor : BalanceEditorWindow<Buff>
{
    // 필터링을 위한 변수
    private bool _showAllStackingTypes = true;
    private BuffStackingType _selectedStackingType;
    
    // 일괄 편집을 위한 변수
    private enum BulkEditMode { Add, Set }
    private BulkEditMode _bulkEditMode;
    private float _bulkDurationValue = 1.0f;

    [MenuItem("Tools/Balance/Buff Balance Editor")]
    public static void ShowWindow()
    {
        GetWindow<BuffBalanceEditor>("Buff Balance Editor");
    }

    /// <summary>
    /// 새로운 Buff 에셋이 저장될 기본 경로를 반환합니다.
    /// </summary>
    protected override string GetNewAssetPath()
    {
        return "Assets/Resources/Buffs";
    }

    /// <summary>
    /// StackingType에 따른 필터 UI를 그립니다.
    /// </summary>
    protected override void DrawFilters()
    {
        base.DrawFilters();
        
        var displayOptions = new List<string> { "All" };
        displayOptions.AddRange(System.Enum.GetNames(typeof(BuffStackingType)));
        
        int currentIndex = _showAllStackingTypes ? 0 : (int)_selectedStackingType + 1;

        EditorGUI.BeginChangeCheck();
        int newIndex = EditorGUILayout.Popup("Filter by Stacking Type:", currentIndex, displayOptions.ToArray(), GUILayout.Width(300));
        if (EditorGUI.EndChangeCheck())
        {
            if (newIndex == 0)
            {
                _showAllStackingTypes = true;
            }
            else
            {
                _showAllStackingTypes = false;
                _selectedStackingType = (BuffStackingType)(newIndex - 1);
            }
        }
    }
    
    /// <summary>
    /// 선택된 StackingType에 따라 리스트를 필터링합니다.
    /// </summary>
    protected override IEnumerable<Buff> ApplyAdvancedFilters(IEnumerable<Buff> items)
    {
        if (_showAllStackingTypes)
        {
            return items;
        }
        return items.Where(b => b.StackingType == _selectedStackingType);
    }
    
    /// <summary>
    /// 선택된 버프들의 Duration 값을 일괄 변경하는 UI를 그립니다.
    /// </summary>
    protected override void DrawBulkEditControls()
    {
        base.DrawBulkEditControls();
        
        EditorGUILayout.BeginHorizontal();
        
        GUILayout.Label("Duration:", GUILayout.Width(70));
        _bulkEditMode = (BulkEditMode)EditorGUILayout.EnumPopup(_bulkEditMode, GUILayout.Width(60));
        _bulkDurationValue = EditorGUILayout.FloatField(_bulkDurationValue, GUILayout.Width(50));

        if (GUILayout.Button("Apply to Selected"))
        {
            foreach (var buff in _selectedItems)
            {
                if (_bulkEditMode == BulkEditMode.Add)
                {
                    buff.Duration += _bulkDurationValue;
                }
                else // Set
                {
                    buff.Duration = _bulkDurationValue;
                }
                EditorUtility.SetDirty(buff);
            }
        }
        
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 테이블의 헤더를 그립니다.
    /// </summary>
    protected override void DrawHeader()
    {
        // 첫 번째 헤더는 체크박스를 위한 공간
        GUILayout.Space(24); 
        DrawSortableHeader("Buff Asset", "Name", GUILayout.Width(150));
        DrawSortableHeader("Buff ID", "ID", GUILayout.Width(150));
        DrawSortableHeader("Duration", "Duration", GUILayout.Width(70));
        DrawSortableHeader("Stacking Type", "Stacking", GUILayout.Width(120));
    }

    /// <summary>
    /// 각 Buff 데이터의 행을 그립니다.
    /// </summary>
    protected override void DrawRow(Buff buff)
    {
        EditorGUILayout.ObjectField(buff, typeof(Buff), false, GUILayout.Width(150));
        buff.BuffId = EditorGUILayout.TextField(buff.BuffId, GUILayout.Width(150));
        buff.Duration = EditorGUILayout.FloatField(buff.Duration, GUILayout.Width(70));
        buff.StackingType = (BuffStackingType)EditorGUILayout.EnumPopup(buff.StackingType, GUILayout.Width(120));
    }

    /// <summary>
    /// 정렬 키에 따라 Buff 리스트를 정렬하는 로직을 제공합니다.
    /// </summary>
    protected override IOrderedEnumerable<Buff> GetSorted(IEnumerable<Buff> items)
    {
        switch (_currentSortKey)
        {
            case "Name":
                return _isAscending ? items.OrderBy(b => b.name) : items.OrderByDescending(b => b.name);
            case "ID":
                return _isAscending ? items.OrderBy(b => b.BuffId) : items.OrderByDescending(b => b.BuffId);
            case "Duration":
                return _isAscending ? items.OrderBy(b => b.Duration) : items.OrderByDescending(b => b.Duration);
            case "Stacking":
                return _isAscending ? items.OrderBy(b => b.StackingType) : items.OrderByDescending(b => b.StackingType);
            default:
                return items.OrderBy(b => b.name);
        }
    }
}
