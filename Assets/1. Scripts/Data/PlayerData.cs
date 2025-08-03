using System.Collections.Generic;

namespace DungeonMaster.Data
{
    /// <summary>
    /// 플레이어의 모든 영구 데이터를 담는 최상위 클래스입니다.
    /// 이 데이터는 저장 및 로드의 대상이 됩니다.
    /// </summary>
    public class PlayerData
    {
        // TODO: BaseDemonLordCard, BaseDungeonRoom 클래스 정의 후 주석 해제 필요
        // public List<BaseDemonLordCardData> OwnedDemonLords;
        // public List<BaseDungeonRoomData> OwnedRooms;
        
        // 보유 베이스 카드들
        public List<BaseMonsterCardData> OwnedBaseCards;

        // 선택된 카드들 (게임에서 사용할)
        // public string SelectedDemonLordId;
        // public List<string> SelectedRoomIds;
        public List<string> SelectedMonsterIds;

        // 보유 재화
        public long Crystal; // 뽑기용 재화
        public long Coin;    // 강화용 재화

        public PlayerData()
        {
            // 데이터가 null이 되지 않도록 생성자에서 리스트를 초기화합니다.
            OwnedBaseCards = new List<BaseMonsterCardData>();
            SelectedMonsterIds = new List<string>();
            
            // OwnedDemonLords = new List<BaseDemonLordCardData>();
            // OwnedRooms = new List<BaseDungeonRoomData>();
            // SelectedRoomIds = new List<string>();
        }
    }
}
