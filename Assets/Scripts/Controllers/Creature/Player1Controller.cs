using UnityEngine;

public class Player1Controller : PlayerController
{

    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
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

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

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
            //print(this.transform.position + " " + inputX * m_speed +" "+ _rigidbody2D.linearVelocity);
            this.transform.Translate(new Vector2(inputX,0) * m_speed * Time.deltaTime);
        }

        //Set AirSpeed in animator
        Animator.SetFloat("AirSpeedY", Rigidbody2D.linearVelocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        Animator.SetBool("WallSlide", m_isWallSliding);

        //Death
        if (Input.GetKeyDown(KeyCode.E) && !m_rolling)
        {
            Animator.SetBool("noBlood", m_noBlood);
            Animator.SetTrigger("Death");
        }

        //Hurt
        else if (Input.GetKeyDown(KeyCode.Q) && !m_rolling)
            Animator.SetTrigger("Hurt");

        //Attack
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
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
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            Animator.SetTrigger("Block");
            Animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            Animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown(KeyCode.LeftShift) && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            Animator.SetTrigger("Roll");
            Rigidbody2D.linearVelocity = new Vector2(m_facingDirection * m_rollForce, Rigidbody2D.linearVelocity.y);
        }

        //Jump
        else if (Input.GetKeyDown(KeyCode.Space) && m_grounded && !m_rolling)
        {
            Animator.SetTrigger("Jump");
            m_grounded = false;
            Animator.SetBool("Grounded", m_grounded);
            Rigidbody2D.linearVelocity = new Vector2(Rigidbody2D.linearVelocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            Animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                Animator.SetInteger("AnimState", 0);
        }
    }
}