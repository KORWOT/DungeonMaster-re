using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEditor;
using UnityEngine;

/// <summary>
/// WaveData 밸런싱을 위한 에디터 윈도우입니다.
/// </summary>
public class WaveDataBalanceEditor : BalanceEditorWindow<WaveData>
{
    [MenuItem("Tools/Balance/WaveData Balance Editor")]
    public static void ShowWindow()
    {
        GetWindow<WaveDataBalanceEditor>("WaveData Balance Editor");
    }

    /// <summary>
    /// 새로운 WaveData 에셋이 저장될 기본 경로를 반환합니다.
    /// </summary>
    protected override string GetNewAssetPath()
    {
        return "Assets/Resources/WaveData";
    }

    /// <summary>
    /// 테이블의 헤더를 그립니다.
    /// </summary>
    protected override void DrawHeader()
    {
        DrawSortableHeader("Wave Asset", "Name", GUILayout.Width(150));
        DrawSortableHeader("Spawn Groups", "Groups", GUILayout.Width(100));
        DrawSortableHeader("Total Enemies", "Total", GUILayout.Width(100));
        DrawSortableHeader("Time To Next", "Time", GUILayout.Width(100));
    }

    /// <summary>
    /// 각 WaveData의 행을 그립니다.
    /// </summary>
    protected override void DrawRow(WaveData wave)
    {
        EditorGUILayout.ObjectField(wave, typeof(WaveData), false, GUILayout.Width(150));
        
        int spawnGroupCount = wave.SpawnList?.Length ?? 0;
        EditorGUILayout.LabelField(spawnGroupCount.ToString(), GUILayout.Width(100));

        int totalEnemies = wave.SpawnList?.Sum(info => info.Count) ?? 0;
        EditorGUILayout.LabelField(totalEnemies.ToString(), GUILayout.Width(100));
        
        wave.TimeToNextWave = EditorGUILayout.FloatField(wave.TimeToNextWave, GUILayout.Width(100));
    }

    /// <summary>
    /// 정렬 키에 따라 WaveData 리스트를 정렬하는 로직을 제공합니다.
    /// </summary>
    protected override IOrderedEnumerable<WaveData> GetSorted(IEnumerable<WaveData> items)
    {
        switch (_currentSortKey)
        {
            case "Name":
                return _isAscending ? items.OrderBy(w => w.name) : items.OrderByDescending(w => w.name);
            case "Groups":
                return _isAscending 
                    ? items.OrderBy(w => w.SpawnList?.Length ?? 0) 
                    : items.OrderByDescending(w => w.SpawnList?.Length ?? 0);
            case "Total":
                return _isAscending 
                    ? items.OrderBy(w => w.SpawnList?.Sum(info => info.Count) ?? 0) 
                    : items.OrderByDescending(w => w.SpawnList?.Sum(info => info.Count) ?? 0);
            case "Time":
                return _isAscending ? items.OrderBy(w => w.TimeToNextWave) : items.OrderByDescending(w => w.TimeToNextWave);
            default:
                return items.OrderBy(w => w.name);
        }
    }
}
