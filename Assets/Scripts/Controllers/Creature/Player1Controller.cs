using UnityEngine;

public class Player1Controller : CreatureController
{
    [SerializeField] float m_speed = 1.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isWallSliding = false;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;

    public GameObject camera;

    protected override void Init()
    {
        base.Init();

        m_groundSensor = Util.FindChild(this.gameObject,"GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = Util.FindChild(this.gameObject,"WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = Util.FindChild(this.gameObject,"WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = Util.FindChild(this.gameObject,"WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = Util.FindChild(this.gameObject,"WallSensor_L2").GetComponent<Sensor_HeroKnight>();
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
            _animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            _animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            _spriteRenderer.flipX = false;
            m_facingDirection = 1;
        }

        else if (inputX < 0)
        {
            _spriteRenderer.flipX = true;
            m_facingDirection = -1;
        }



        // Move
        if (!m_rolling)
        {
            //print(this.transform.position + " " + inputX * m_speed +" "+ _rigidbody2D.linearVelocity);
            this.transform.Translate(new Vector2(inputX,0) * m_speed * Time.deltaTime);
        }

        //Set AirSpeed in animator
        _animator.SetFloat("AirSpeedY", _rigidbody2D.linearVelocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        _animator.SetBool("WallSlide", m_isWallSliding);

        //Death
        if (Input.GetKeyDown(KeyCode.E) && !m_rolling)
        {
            _animator.SetBool("noBlood", m_noBlood);
            _animator.SetTrigger("Death");
        }

        //Hurt
        else if (Input.GetKeyDown(KeyCode.Q) && !m_rolling)
            _animator.SetTrigger("Hurt");

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
            _animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            _animator.SetTrigger("Block");
            _animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            _animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown(KeyCode.LeftShift) && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            _animator.SetTrigger("Roll");
            _rigidbody2D.linearVelocity = new Vector2(m_facingDirection * m_rollForce, _rigidbody2D.linearVelocity.y);
        }


        //Jump
        else if (Input.GetKeyDown(KeyCode.Space) && m_grounded && !m_rolling)
        {
            _animator.SetTrigger("Jump");
            m_grounded = false;
            _animator.SetBool("Grounded", m_grounded);
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            _animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                _animator.SetInteger("AnimState", 0);
        }
    }
}