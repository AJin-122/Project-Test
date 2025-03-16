using UnityEngine;

public class InputManager
{
    public Vector2 MoveInput { get; private set; }
    public bool IsMoving => MoveInput != Vector2.zero;
    public bool IsPlayerSwitchPressed { get; private set; }
    public bool IsAttackPressed { get; private set; }
    public bool IsDefensePressed { get; private set; }
    public bool IsRollingPressed { get; private set; }
    public bool IsJumpingPressed { get; private set; }

    public void OnUpdate()
    {
        // 이동 입력 처리 (WASD)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        MoveInput = new Vector2(h, v).normalized;

        // 플레이어 전환 입력 처리 (R)
        IsPlayerSwitchPressed = Input.GetKeyDown(KeyCode.R);

        // 공격 입력 처리 (마우스 좌클릭)
        IsAttackPressed = Input.GetMouseButtonDown(0);

        // 방어 입력 처리 (마우스 우클릭)
        IsDefensePressed = Input.GetMouseButtonDown(1);

        // 구르기 입력 처리(왼쪽 쉬프트)
        IsRollingPressed = Input.GetKeyDown(KeyCode.LeftShift);

        // 점프 입력 처리(스페이스)
        IsJumpingPressed = Input.GetKeyDown(KeyCode.Space);
    }
} 