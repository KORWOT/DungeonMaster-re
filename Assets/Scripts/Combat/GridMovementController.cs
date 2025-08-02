using System.Collections;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// 그리드 기반 환경에서 유닛의 이동을 담당하는 컴포넌트입니다.
    /// </summary>
    public class GridMovementController : MonoBehaviour
    {
        [Tooltip("유닛의 이동 속도 (그리드 단위/초)")]
        public float MoveSpeed = 2.0f;

        private Coroutine _moveCoroutine;

        /// <summary>
        /// 지정된 목표 그리드 좌표로 이동을 시작합니다.
        /// </summary>
        /// <param name="targetGridPosition">목표 그리드 좌표</param>
        public void MoveToTarget(Vector2Int targetGridPosition)
        {
            // TODO: 그리드 좌표를 월드 좌표로 변환하는 로직 필요
            Vector3 targetWorldPosition = new Vector3(targetGridPosition.x, targetGridPosition.y, 0);

            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            _moveCoroutine = StartCoroutine(MoveRoutine(targetWorldPosition));
        }

        private IEnumerator MoveRoutine(Vector3 targetPosition)
        {
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, MoveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPosition; // 정확한 위치 보정
            _moveCoroutine = null;
            Debug.Log("이동 완료.");
        }

        // TODO: 향후 A* 등의 경로 탐색 알고리즘을 사용하여 경로를 따라 이동하는 기능 추가
        /*
        public void MoveAlongPath(List<Vector2Int> path)
        {
            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
            _moveCoroutine = StartCoroutine(PathMoveRoutine(path));
        }
        */
    }
}
