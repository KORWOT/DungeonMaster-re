using UnityEngine;

namespace Data
{
    /// <summary>
    /// 한 웨이브에 등장하는 적의 구성과 스폰 설정을 정의하는 ScriptableObject입니다.
    /// </summary>
    [CreateAssetMenu(fileName = "New WaveData", menuName = "Data/Wave Data")]
    public class WaveData : ScriptableObject
    {
        [System.Serializable]
        public struct SpawnInfo
        {
            [Tooltip("스폰할 적 몬스터의 프리팹")]
            public GameObject EnemyPrefab;
            [Tooltip("스폰할 몬스터의 수")]
            public int Count;
            [Tooltip("이전 몬스터 스폰 후 다음 몬스터 스폰까지의 시간 간격")]
            public float SpawnInterval;
        }

        [Tooltip("이 웨이브에 포함된 적 스폰 정보 목록")]
        public SpawnInfo[] SpawnList;
        
        [Tooltip("이 웨이브가 끝난 후 다음 웨이브까지의 대기 시간")]
        public float TimeToNextWave;
    }
}
