using System.Collections.Generic;
using UnityEngine;
// using Data; // TODO: BaseDungeonRoom 클래스 정의 후 주석 해제

namespace Dungeon
{
    /// <summary>
    /// 던전의 특정 셀에 대한 정보를 담는 클래스입니다.
    /// </summary>
    public class GridCell
    {
        public Vector2Int Position;
        // public BaseDungeonRoom PlacedRoom { get; private set; }
        public bool IsOccupied => PlacedRoom != null;
        public object PlacedRoom { get; set; } // 임시로 object 사용

        public GridCell(int x, int y)
        {
            Position = new Vector2Int(x, y);
        }
    }

    /// <summary>
    /// 던전의 그리드 구조와 룸 배치를 관리하는 시스템입니다.
    /// </summary>
    public class DungeonGrid
    {
        public readonly GridCell[,] Grid = new GridCell[3, 3];
        public readonly Vector2Int DemonLordPosition = new Vector2Int(0, 1); // 마왕방은 왼쪽 중앙 고정

        public DungeonGrid()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    Grid[x, y] = new GridCell(x, y);
                }
            }
            // 마왕방은 기본적으로 배치된 상태로 가정
            Grid[DemonLordPosition.x, DemonLordPosition.y].PlacedRoom = new object(); // 임시
        }

        /// <summary>
        /// 지정된 위치에 룸을 배치합니다.
        /// </summary>
        /// <returns>배치 성공 여부</returns>
        public bool PlaceRoom(object room, Vector2Int position) // room 타입을 BaseDungeonRoom으로 변경 필요
        {
            if (!IsValidPosition(position) || Grid[position.x, position.y].IsOccupied)
            {
                return false;
            }
            Grid[position.x, position.y].PlacedRoom = room;
            return true;
        }
        
        /// <summary>
        /// 지정된 위치의 룸을 제거합니다.
        /// </summary>
        public void RemoveRoom(Vector2Int position)
        {
            if (IsValidPosition(position) && position != DemonLordPosition)
            {
                Grid[position.x, position.y].PlacedRoom = null;
            }
        }

        /// <summary>
        /// 몬스터를 배치할 수 있는 유효한 위치 목록을 반환합니다.
        /// </summary>
        public List<Vector2Int> GetAvailableMonsterPositions()
        {
            var availablePositions = new List<Vector2Int>();
            // TODO: 몬스터 배치 규칙에 따라 유효한 위치를 찾는 로직 구현
            // 예: 비어있지 않은 모든 방
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (Grid[x, y].IsOccupied)
                    {
                        availablePositions.Add(new Vector2Int(x, y));
                    }
                }
            }
            return availablePositions;
        }

        /// <summary>
        /// 좌표가 그리드 범위 내에 있는지 확인합니다.
        /// </summary>
        private bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < 3 && position.y >= 0 && position.y < 3;
        }
    }
}
