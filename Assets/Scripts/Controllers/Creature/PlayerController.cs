using UnityEngine;

public class PlayerController : CreatureController
{
    [SerializeField] protected float m_speed = 1.0f;
    [SerializeField] protected float m_jumpForce = 7.5f;
    [SerializeField] protected float m_rollForce = 6.0f;
    [SerializeField] protected bool m_noBlood = false;
    [SerializeField] protected GameObject m_slideDust;

    protected Sensor_HeroKnight m_groundSensor;
    protected Sensor_HeroKnight m_wallSensorR1;
    protected Sensor_HeroKnight m_wallSensorR2;
    protected Sensor_HeroKnight m_wallSensorL1;
    protected Sensor_HeroKnight m_wallSensorL2;
    protected bool m_isWallSliding = false;
    protected bool m_grounded = false;
    protected bool m_rolling = false;
    protected int m_facingDirection = 1;
    protected int m_currentAttack = 0;
    protected float m_timeSinceAttack = 0.0f;
    protected float m_delayToIdle = 0.0f;
    protected float m_rollDuration = 8.0f / 14.0f;
    protected float m_rollCurrentTime;

    protected override void Init()
    {
        base.Init();

        m_groundSensor = Util.FindChild(this.gameObject, "GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = Util.FindChild(this.gameObject, "WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = Util.FindChild(this.gameObject, "WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = Util.FindChild(this.gameObject, "WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = Util.FindChild(this.gameObject, "WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    protected override void UpdateController()
    {
        // 현재 활성화된 플레이어가 아니면 업데이트하지 않음
        if (gameObject != Managers.PlayerManager.CurrentPlayer)
            return;

        base.UpdateController();

        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if (m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            Animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            Animator.SetBool("Grounded", m_grounded);
        }

        HandleMovement();
        HandleActions();
        UpdateAnimationState();
    }

    protected virtual void HandleMovement()
    {
        float inputX = Managers.Input.MoveInput.x;

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            SpriteRenderer.flipX = false;
            m_facingDirection = 1;
        }
        else if (inputX < 0)
        {
            SpriteRenderer.flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling)
        {
            transform.Translate(new Vector2(inputX, 0) * m_speed * Time.deltaTime);
        }

        // Set AirSpeed in animator
        Animator.SetFloat("AirSpeedY", Rigidbody2D.linearVelocity.y);
    }

    protected virtual void HandleActions()
    {
        // Attack
        if (Managers.Input.IsAttackPressed && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            Animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        // Block
        if (Managers.Input.IsDefensePressed && !m_rolling)
        {
            Animator.SetTrigger("Block");
            Animator.SetBool("IdleBlock", true);
        }
        else if (!Managers.Input.IsDefensePressed)
        {
            Animator.SetBool("IdleBlock", false);
        }

        // Roll
        if (Managers.Input.IsRollingPressed && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            Animator.SetTrigger("Roll");
            Rigidbody2D.linearVelocity = new Vector2(m_facingDirection * m_rollForce, Rigidbody2D.linearVelocity.y);
        }

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

    protected virtual void UpdateAnimationState()
    {
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        Animator.SetBool("WallSlide", m_isWallSliding);

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
