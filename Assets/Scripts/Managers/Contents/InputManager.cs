using UnityEngine;

public class InputManager
{
    public Vector2 MoveInput { get; private set; }
    public bool IsMoving => MoveInput != Vector2.zero;
    public bool IsPlayerSwitchPressed { get; private set; }

    public void OnUpdate()
    {
        // 이동 입력 처리
        MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        // 플레이어 전환 입력 처리
        IsPlayerSwitchPressed = Input.GetKeyDown(KeyCode.R);
    }
} 