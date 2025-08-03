using DungeonMaster.Data.Enums;
using UnityEngine;

namespace DungeonMaster.Settings
{
    /// <summary>
    /// 카드 등급(C~UR)별 기본 스탯을 정의하는 ScriptableObject입니다.
    /// </summary>
    [CreateAssetMenu(fileName = "CardGradeSettings", menuName = "Settings/Card Grade Settings")]
    public class CardGradeSettings : ScriptableObject
    {
        [System.Serializable]
        public struct GradeStatSetting
        {
            public CardGrade Grade;
            [Tooltip("이 등급의 기본 체력")]
            public int BaseHealth;
            [Tooltip("이 등급의 기본 공격력")]
            public int BaseAttack;
            [Tooltip("이 등급의 기본 방어력")]
            public int BaseDefense;
        }

        public GradeStatSetting[] GradeSettingsList;

        /// <summary>
        /// 특정 카드 등급에 해당하는 스탯 설정을 찾습니다.
        /// </summary>
        public GradeStatSetting GetSetting(CardGrade cardGrade)
        {
            foreach (var setting in GradeSettingsList)
            {
                if (setting.Grade == cardGrade)
                {
                    return setting;
                }
            }
            Debug.LogError($"{cardGrade}에 대한 설정이 CardGradeSettings에 없습니다.");
            return new GradeStatSetting { Grade = cardGrade };
        }
    }
}
