using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Dungeon
{
    /// <summary>
    /// 전투 시작 전, 던전 그리드에 몬스터를 배치하는 로직을 관리합니다.
    /// </summary>
    public class MonsterPlacementManager
    {
        private readonly DungeonGrid _dungeonGrid;
        private readonly Dictionary<Vector2Int, BaseMonsterCard> _placedMonsters = new Dictionary<Vector2Int, BaseMonsterCard>();

        public MonsterPlacementManager(DungeonGrid dungeonGrid)
        {
            _dungeonGrid = dungeonGrid;
        }

        /// <summary>
        /// 지정된 위치에 몬스터를 배치합니다.
        /// </summary>
        /// <param name="monster">배치할 몬스터 카드</param>
        /// <param name="position">배치할 그리드 좌표</param>
        /// <returns>배치 성공 여부</returns>
        public bool PlaceMonster(BaseMonsterCard monster, Vector2Int position)
        {
            // 해당 위치에 방이 있고, 다른 몬스터가 없는지 확인
            if (!_dungeonGrid.Grid[position.x, position.y].IsOccupied || _placedMonsters.ContainsKey(position))
            {
                Debug.LogWarning($"위치 {position}에 몬스터를 배치할 수 없습니다.");
                return false;
            }
            
            // TODO: 몬스터 배치 수 제한 등 추가 규칙 검증 필요

            _placedMonsters[position] = monster;
            Debug.Log($"{monster.MonsterId}를 위치 {position}에 배치했습니다.");
            return true;
        }

        /// <summary>
        /// 지정된 위치의 몬스터를 제거합니다.
        /// </summary>
        /// <param name="position">제거할 몬스터의 그리드 좌표</param>
        public void RemoveMonster(Vector2Int position)
        {
            if (_placedMonsters.Remove(position))
            {
                Debug.Log($"위치 {position}의 몬스터를 제거했습니다.");
            }
        }

        /// <summary>
        /// 현재 몬스터 배치가 유효한지 검증합니다.
        /// </summary>
        /// <returns>배치가 유효하면 true</returns>
        public bool IsPlacementValid()
        {
            // TODO: 게임 시작을 위한 최소/최대 몬스터 수 등 배치 완료 조건 검증 로직 구현
            return _placedMonsters.Count > 0;
        }

        /// <summary>
        /// 배치된 모든 몬스터의 정보를 가져옵니다.
        /// </summary>
        public IReadOnlyDictionary<Vector2Int, BaseMonsterCard> GetPlacedMonsters()
        {
            return _placedMonsters;
        }
    }
}
