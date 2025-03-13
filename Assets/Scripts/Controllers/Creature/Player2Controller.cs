using UnityEngine;

public class Player2Controller : CreatureController
{
    protected override void UpdateController()
    {
        // 현재 활성화된 플레이어가 아니면 업데이트하지 않음
        if (gameObject != Managers.PlayerManager.CurrentPlayer)
            return;

        base.UpdateController();
    }
}