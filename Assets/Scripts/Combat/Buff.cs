using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Combat.Buffs
{
    /// <summary>
    /// 버프/디버프의 데이터와 동작을 정의하는 ScriptableObject입니다.
    /// 다양한 종류의 버프를 에셋 파일로 생성하고 관리할 수 있습니다.
    /// </summary>
    [CreateAssetMenu(fileName = "New Buff", menuName = "Combat/Buff")]
    public class Buff : ScriptableObject
    {
        [Header("기본 정보")]
        [Tooltip("버프의 고유 ID")]
        public string BuffId;
        
        [Tooltip("버프 이름")]
        public string BuffName;

        [Tooltip("버프 설명")]
        [TextArea] public string Description;
        
        [Tooltip("버프 아이콘")]
        public Sprite Icon;

        [Header("시간 및 효과")]
        [Tooltip("지속시간 (초). 0 이하는 영구 지속을 의미합니다.")]
        public float Duration;

        [Tooltip("버프가 적용될 때 캐릭터에게 추가될 스탯 모디파이어 목록")]
        public List<StatModifier> StatModifiers;

        // 버프의 실제 인스턴스를 관리하는 로직은 BuffController에서 처리합니다.
        // 이 ScriptableObject는 버프의 '원형' 또는 '설계도' 역할을 합니다.
    }
}
