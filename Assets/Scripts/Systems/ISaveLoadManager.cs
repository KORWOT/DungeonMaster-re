using DungeonMaster.Data;

namespace DungeonMaster.Systems
{
    public interface ISaveLoadManager
    {
        PlayerData LoadedData { get; }
        void SaveData(PlayerData playerData);
        void LoadData();
    }
}
