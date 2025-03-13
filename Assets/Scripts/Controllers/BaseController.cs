using UnityEngine;

public class BaseController : MonoBehaviour
{
    public int Id { get; set; }
    private StatInfo _stat = new();
    private ObjectInfo _info = new();
    private PosInfo _posInfo = new();

    public virtual StatInfo Stat
    {
        get => _stat;
        set
        {
            if (value == null) return;
            if (_stat.Equals(value)) return;
            _stat = value;
        }
    }

    public virtual ObjectInfo Info
    {
        get => _info;
        set
        {
            if (value == null) return;
            if (_info.Equals(value)) return;
            _info = value;
        }
    }

    public PosInfo PosInfo
    {
        get => _posInfo;
        set
        {
            if (value == null) return;
            if (_posInfo.Equals(value)) return;
            _posInfo = value;
        }
    }

    public virtual float ChangeCooltime
    {
        get => Stat.changeCooltime;
        set => Stat.changeCooltime = value;
    }

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;

    public Animator Animator => _animator;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public BoxCollider2D BoxCollider2D => _boxCollider2D;

    public virtual CreatureState State
    {
        get { return PosInfo.state; }
        set
        {
            if (PosInfo.state == value)
                return;
            PosInfo.state = value;
            UpdateAnimation();
        }
    }

    protected virtual void UpdateAnimation()
    {
        if (_animator == null || _spriteRenderer == null)
            return;
        switch (State)
        {
            case CreatureState.Idle:

                break;
            case CreatureState.Moving:

                break;
            case CreatureState.Skill:

                break;
            case CreatureState.Dead:
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        CacheComponents();
        Init();
    }

    private void CacheComponents()
    {
        if (_animator == null) _animator = GetComponent<Animator>();
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
        if (_boxCollider2D == null) _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    protected virtual void Init()
    {
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        UpdateController();
    }

    protected virtual void UpdateController()
    {
        switch (State)
        {
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Moving:
                UpdateMoving();
                break;
            case CreatureState.Skill:
                UpdateSkill();
                break;
            case CreatureState.Dead:
                UpdateDead();
                break;
            default:
                break;
        }
    }

    // 이동 방향으로 목표 지점까지 이동
    protected virtual void UpdateIdle()
    {

    }

    // 입력받은 이동 처리
    protected virtual void UpdateMoving()
    {
        // InputManager를 통한 이동 처리
        Rigidbody2D.linearVelocity = Managers.Input.MoveInput * 5f; // 기본 이동 속도 5

        // 이동 방향에 따라 스프라이트 방향 전환
        if (Managers.Input.MoveInput.x != 0)
            SpriteRenderer.flipX = Managers.Input.MoveInput.x < 0;
    }

    protected virtual void UpdateSkill()
    {

    }

    protected virtual void UpdateDead()
    {

    }
}