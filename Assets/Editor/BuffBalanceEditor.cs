using System.Collections.Generic;
using System.Linq;
using Combat.Buffs;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Buff 데이터 밸런싱을 위한 에디터 윈도우입니다.
/// 제네릭 BalanceEditorWindow 클래스를 상속받아 구현되었습니다.
/// </summary>
public class BuffBalanceEditor : BalanceEditorWindow<Buff>
{
    [MenuItem("Tools/Buff Balance Editor")]
    public static void ShowWindow()
    {
        GetWindow<BuffBalanceEditor>("Buff Balance Editor");
    }

    /// <summary>
    /// 테이블의 헤더를 그립니다. 이제 정렬이 가능한 버튼으로 구성됩니다.
    /// </summary>
    protected override void DrawHeader()
    {
        DrawSortableHeader("Buff Asset", "Name", GUILayout.Width(150));
        DrawSortableHeader("Buff ID", "ID", GUILayout.Width(150));
        DrawSortableHeader("Duration", "Duration", GUILayout.Width(70));
        DrawSortableHeader("Stacking Type", "Stacking", GUILayout.Width(120));
    }

    /// <summary>
    /// 각 Buff 데이터의 행을 그립니다.
    /// </summary>
    /// <param name="buff">그려질 Buff 데이터</param>
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
    /// <param name="items">정렬할 Buff 아이템 목록</param>
    /// <returns>정렬된 Buff 아이템 목록</returns>
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
