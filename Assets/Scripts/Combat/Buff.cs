using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Combat.Buffs
{
    /// <summary>
    /// 버프가 중첩될 때의 처리 방식을 정의합니다.
    /// </summary>
    public enum BuffStackingType
    {
        /// <summary>
        /// 동일한 버프를 여러 개 가질 수 있습니다 (효과 중첩).
        /// </summary>
        AllowDuplicate,
        /// <summary>
        /// 이미 동일한 버프가 있다면, 기존 버프의 지속시간만 초기화합니다.
        /// </summary>
        RefreshDuration,
        /// <summary>
        /// 이미 동일한 버프가 있다면, 새로 적용되는 버프를 무시합니다.
        /// </summary>
        Ignore
    }
    
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

        [Tooltip("버프 중첩 시 처리 방식")]
        public BuffStackingType StackingType = BuffStackingType.RefreshDuration;

        [Tooltip("버프가 적용될 때 캐릭터에게 추가될 스탯 모디파이어 목록")]
        public List<StatModifier> StatModifiers;
    }
}
