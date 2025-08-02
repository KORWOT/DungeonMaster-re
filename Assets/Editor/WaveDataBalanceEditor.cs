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
    private enum TimeFilterType { All, GreaterThan, LessThan }
    private TimeFilterType _timeFilter = TimeFilterType.All;
    private float _timeFilterValue = 10f;

    private enum BulkEditMode { Add, Set }
    private BulkEditMode _bulkEditMode;
    private float _bulkTimeValue = 1.0f;

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
    /// `TimeToNextWave` 값을 기준으로 필터링하는 UI를 그립니다.
    /// </summary>
    protected override void DrawFilters()
    {
        base.DrawFilters();
        
        EditorGUILayout.LabelField("Filter by Time:", GUILayout.Width(100));
        _timeFilter = (TimeFilterType)EditorGUILayout.EnumPopup(_timeFilter, GUILayout.Width(100));
        
        if (_timeFilter != TimeFilterType.All)
        {
            _timeFilterValue = EditorGUILayout.FloatField(_timeFilterValue, GUILayout.Width(50));
        }
    }

    /// <summary>
    /// 선택된 시간 필터에 따라 리스트를 필터링합니다.
    /// </summary>
    protected override IEnumerable<WaveData> ApplyAdvancedFilters(IEnumerable<WaveData> items)
    {
        switch (_timeFilter)
        {
            case TimeFilterType.GreaterThan:
                return items.Where(w => w.TimeToNextWave > _timeFilterValue);
            case TimeFilterType.LessThan:
                return items.Where(w => w.TimeToNextWave < _timeFilterValue);
            default:
                return items;
        }
    }
    
    /// <summary>
    /// 선택된 웨이브들의 TimeToNextWave 값을 일괄 변경하는 UI를 그립니다.
    /// </summary>
    protected override void DrawBulkEditControls()
    {
        base.DrawBulkEditControls();
        
        EditorGUILayout.BeginHorizontal();
        
        GUILayout.Label("Time To Next Wave:", GUILayout.Width(120));
        _bulkEditMode = (BulkEditMode)EditorGUILayout.EnumPopup(_bulkEditMode, GUILayout.Width(60));
        _bulkTimeValue = EditorGUILayout.FloatField(_bulkTimeValue, GUILayout.Width(50));

        if (GUILayout.Button("Apply to Selected"))
        {
            foreach (var wave in _selectedItems)
            {
                if (_bulkEditMode == BulkEditMode.Add)
                {
                    wave.TimeToNextWave += _bulkTimeValue;
                }
                else // Set
                {
                    wave.TimeToNextWave = _bulkTimeValue;
                }
                EditorUtility.SetDirty(wave);
            }
        }
        
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 테이블의 헤더를 그립니다.
    /// </summary>
    protected override void DrawHeader()
    {
        GUILayout.Space(24);
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
