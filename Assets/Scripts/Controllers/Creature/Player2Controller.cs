using UnityEngine;

public class Player2Controller : PlayerController
{
    protected override void HandleActions()
    {
        // Jump
        if (Managers.Input.IsJumpingPressed && m_grounded && !m_rolling)
        {
            Animator.SetTrigger("Jump");
            m_grounded = false;
            Animator.SetBool("Grounded", m_grounded);
            Rigidbody2D.linearVelocity = new Vector2(Rigidbody2D.linearVelocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }
    }

    protected override void UpdateAnimationState()
    {
        // Run
        if (Mathf.Abs(Managers.Input.MoveInput.x) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            Animator.SetInteger("AnimState", 1);
        }
        // Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                Animator.SetInteger("AnimState", 0);
        }
    }
}