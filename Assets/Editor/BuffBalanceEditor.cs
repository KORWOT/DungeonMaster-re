using System.Collections.Generic;
using System.Linq;
using Combat.Buffs;
using UnityEditor;
using UnityEngine;

public class BuffBalanceEditor : EditorWindow
{
    private List<Buff> _allBuffs;
    private Vector2 _scrollPosition;

    [MenuItem("Tools/Buff Balance Editor")]
    public static void ShowWindow()
    {
        GetWindow<BuffBalanceEditor>("Buff Balance Editor");
    }

    private void OnEnable()
    {
        LoadAllBuffs();
    }

    private void LoadAllBuffs()
    {
        // 프로젝트 전체에서 Buff 타입의 모든 .asset 파일을 찾습니다.
        string[] guids = AssetDatabase.FindAssets("t:Buff");
        _allBuffs = new List<Buff>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Buff buff = AssetDatabase.LoadAssetAtPath<Buff>(path);
            if (buff != null)
            {
                _allBuffs.Add(buff);
            }
        }
        
        // 이름순으로 정렬
        _allBuffs = _allBuffs.OrderBy(b => b.name).ToList();
    }

    private void OnGUI()
    {
        GUILayout.Label("Buff Balance Sheet", EditorStyles.boldLabel);

        if (GUILayout.Button("Reload All Buffs"))
        {
            LoadAllBuffs();
        }

        EditorGUILayout.Space();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        // 헤더
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Buff Asset", EditorStyles.boldLabel, GUILayout.Width(150));
        GUILayout.Label("Buff ID", EditorStyles.boldLabel, GUILayout.Width(150));
        GUILayout.Label("Duration", EditorStyles.boldLabel, GUILayout.Width(70));
        GUILayout.Label("Stacking Type", EditorStyles.boldLabel, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // 데이터 필드
        if (_allBuffs != null)
        {
            foreach (Buff buff in _allBuffs)
            {
                // 변경 감지를 시작합니다.
                EditorGUI.BeginChangeCheck();
                
                EditorGUILayout.BeginHorizontal();
                
                // Buff 에셋 파일을 직접 수정할 수 있도록 ObjectField를 사용합니다.
                EditorGUILayout.ObjectField(buff, typeof(Buff), false, GUILayout.Width(150));
                
                buff.BuffId = EditorGUILayout.TextField(buff.BuffId, GUILayout.Width(150));
                buff.Duration = EditorGUILayout.FloatField(buff.Duration, GUILayout.Width(70));
                buff.StackingType = (BuffStackingType)EditorGUILayout.EnumPopup(buff.StackingType, GUILayout.Width(120));
                
                EditorGUILayout.EndHorizontal();

                // EndChangeCheck()가 true를 반환하면, BeginChangeCheck() 이후의 필드 중 하나라도 변경된 것입니다.
                if (EditorGUI.EndChangeCheck())
                {
                    // 변경된 ScriptableObject를 'dirty' 상태로 표시하여 Unity가 저장할 수 있도록 합니다.
                    EditorUtility.SetDirty(buff);
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }
}
