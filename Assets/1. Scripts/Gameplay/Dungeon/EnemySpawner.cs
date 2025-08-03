using System.Collections;
using System.Collections.Generic;
using DungeonMaster.Data;
using UnityEngine;

namespace DungeonMaster.Gameplay.Dungeon
{
    /// <summary>
    /// 웨이브 데이터를 기반으로 적을 생성하고 배치하는 역할을 합니다.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        private DungeonGrid _dungeonGrid;
        private Coroutine _spawnCoroutine;

        public void Initialize(DungeonGrid dungeonGrid)
        {
            _dungeonGrid = dungeonGrid;
        }

        /// <summary>
        /// 주어진 웨이브 목록에 따라 순차적으로 적 스폰을 시작합니다.
        /// </summary>
        /// <param name="waves">스폰할 웨이브 데이터 목록</param>
        public void StartSpawning(List<WaveDataData> waves)
        {
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
            }
            _spawnCoroutine = StartCoroutine(SpawnWavesRoutine(waves));
        }

        /// <summary>
        /// 모든 스폰 활동을 중단합니다.
        /// </summary>
        public void StopSpawning()
        {
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
        }

        private IEnumerator SpawnWavesRoutine(List<WaveDataData> waves)
        {
            foreach (var waveData in waves)
            {
                yield return StartCoroutine(SpawnSingleWaveRoutine(waveData));
                yield return new WaitForSeconds(waveData.TimeToNextWave);
            }
            Debug.Log("모든 웨이브가 종료되었습니다.");
        }

        private IEnumerator SpawnSingleWaveRoutine(WaveDataData waveData)
        {
            Debug.Log($"웨이브 시작: {waveData.name}");
            foreach (var spawnInfo in waveData.SpawnList)
            {
                for (int i = 0; i < spawnInfo.Count; i++)
                {
                    SpawnEnemy(spawnInfo.EnemyPrefab);
                    yield return new WaitForSeconds(spawnInfo.SpawnInterval);
                }
            }
        }

        private void SpawnEnemy(GameObject enemyPrefab)
        {
            if (enemyPrefab == null) return;
            
            // TODO: 던전 입구 위치를 DungeonGrid 또는 다른 시스템에서 받아와야 함.
            // 현재는 임시로 (2, 1) 위치를 입구로 가정합니다.
            Vector2Int spawnGridPosition = new Vector2Int(2, 1);
            
            // TODO: 그리드 좌표를 실제 월드 좌표로 변환하는 로직 필요.
            Vector3 spawnWorldPosition = new Vector3(spawnGridPosition.x, spawnGridPosition.y, 0);

            Instantiate(enemyPrefab, spawnWorldPosition, Quaternion.identity, this.transform);
            Debug.Log($"{enemyPrefab.name} 생성 완료.");
        }
    }
}
