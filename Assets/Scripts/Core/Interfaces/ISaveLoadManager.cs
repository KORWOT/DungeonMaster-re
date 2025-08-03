using DungeonMaster.Data;

namespace DungeonMaster.Core.Interfaces
{
    public interface ISaveLoadManager
    {
        PlayerData LoadedData { get; }
        void SaveData(PlayerData playerData);
        void LoadData();
    }
}
