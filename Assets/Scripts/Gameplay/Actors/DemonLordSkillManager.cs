using System.Collections.Generic;
using DungeonMaster.Data.Structs;
using UnityEngine;

namespace DungeonMaster.Gameplay.Actors
{
    /// <summary>
    /// 플레이어가 직접 사용하는 마왕의 액티브 스킬을 관리합니다.
    /// </summary>
    public class DemonLordSkillManager : MonoBehaviour
    {
        private List<ActiveSkillHandler> _activeSkills = new List<ActiveSkillHandler>();

        /// <summary>
        /// 관리할 스킬들을 초기화합니다.
        /// </summary>
        /// <param name="skillDataList">마왕이 장착한 스킬 데이터 목록</param>
        public void InitializeSkills(List<UniqueSkillData> skillDataList)
        {
            _activeSkills = new List<ActiveSkillHandler>();
            foreach (var skillData in skillDataList)
            {
                // TODO: SkillSettings를 로드하여 쿨다운 등 정보 가져오기
                _activeSkills.Add(new ActiveSkillHandler(skillData, 5f)); // 임시로 쿨다운 5초
            }
        }

        /// <summary>
        /// 지정된 인덱스의 스킬을 사용합니다.
        /// </summary>
        /// <param name="skillIndex">사용할 스킬의 리스트 인덱스</param>
        /// <param name="targetPosition">스킬을 사용할 대상 위치</param>
        public void UseSkill(int skillIndex, Vector2Int targetPosition)
        {
            if (skillIndex < 0 || skillIndex >= _activeSkills.Count)
            {
                Debug.LogError("잘못된 스킬 인덱스입니다.");
                return;
            }

            _activeSkills[skillIndex].Use(targetPosition);
        }

        private void Update()
        {
            // 모든 스킬의 쿨다운을 갱신합니다.
            foreach (var skill in _activeSkills)
            {
                skill.Tick(Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// 개별 액티브 스킬의 상태(쿨다운 등)를 관리하는 핸들러 클래스입니다.
    /// </summary>
    public class ActiveSkillHandler
    {
        public UniqueSkillData SkillData { get; }
        public float Cooldown { get; private set; }
        public float RemainingCooldown { get; private set; }
        public bool IsReady => RemainingCooldown <= 0;

        public ActiveSkillHandler(UniqueSkillData skillData, float cooldown)
        {
            SkillData = skillData;
            Cooldown = cooldown;
            RemainingCooldown = 0;
        }

        public void Tick(float deltaTime)
        {
            if (RemainingCooldown > 0)
            {
                RemainingCooldown -= deltaTime;
            }
        }

        public void Use(Vector2Int targetPosition)
        {
            if (!IsReady)
            {
                Debug.LogWarning($"{SkillData.SkillId} 스킬은 아직 쿨다운 중입니다.");
                return;
            }

            Debug.Log($"{SkillData.SkillId} 스킬 사용! (타겟: {targetPosition})");
            // TODO: 실제 스킬 효과 발동 로직 구현
            
            RemainingCooldown = Cooldown;
        }
    }
}
