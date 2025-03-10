using UnityEngine;

public class BaseController : MonoBehaviour
{
    public int Id { get; set; }
    StatInfo _stat = new StatInfo();
    ObjectInfo _info;

    public virtual StatInfo Stat
    {
        get { return _stat; }
        set
        {
            if (_stat.Equals(value))
                return;
            _stat = value;
        }
    }

    public virtual ObjectInfo Info { get; set; } = new ObjectInfo();

    public virtual float ChangeCooltime
    {
        get { return Stat.changeCooltime; }
        set { Stat.changeCooltime = value; }
    }

    PosInfo _posInfo = new PosInfo();
    public PosInfo PosInfo
    {
        get { return _posInfo; }
        set
        {
            if (_posInfo.Equals(value))
                return;
            PosInfo = value;
        }
    }

    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rigidbody2D;
    protected BoxCollider2D _boxCollider2D;

    public SpriteRenderer SpriteRenderer
    {
        get { return _spriteRenderer; }
    }

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

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        this._animator = this.GetComponent<Animator>();
        this._spriteRenderer = this.GetComponent<SpriteRenderer>();
        this._rigidbody2D = this.GetComponent<Rigidbody2D>();
        this._boxCollider2D = this.GetComponent<BoxCollider2D>();

        UpdateAnimation();
    }

    public virtual bool Active
    {
        get { return this._spriteRenderer.enabled && this._rigidbody2D.simulated; }
        set
        {
            if (_spriteRenderer == null || _rigidbody2D == null)
                return;

            this._spriteRenderer.enabled = value;
            this._rigidbody2D.simulated = value;
        }
    }

    void Update()
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

    // �̵� ������ ���� �� �� ���� ��ǥ�� �̵�
    protected virtual void UpdateIdle()
    {

    }

    // �ε巴�� �̵� ó��
    protected virtual void UpdateMoving()
    {

    }

    protected virtual void UpdateSkill()
    {

    }

    protected virtual void UpdateDead()
    {

    }
}