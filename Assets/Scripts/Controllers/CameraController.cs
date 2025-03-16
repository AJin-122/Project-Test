using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("추적 설정")]
    [SerializeField] private float smoothSpeed = 5f;  // 카메라 이동 부드러움 정도
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -10);  // 카메라와 타겟의 거리

    private void LateUpdate()
    {
        // 현재 활성화된 플레이어를 따라감
        GameObject target = Managers.PlayerManager.CurrentPlayer;
        if (target == null) return;

        // 목표 위치 계산 (플레이어 위치 + offset)
        Vector3 desiredPosition = target.transform.position + offset;
        
        // 부드러운 이동을 위한 Lerp 사용
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        // 카메라 위치 업데이트
        transform.position = smoothedPosition;
    }
} 