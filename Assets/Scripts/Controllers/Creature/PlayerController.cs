using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CreatureController
{
    [SerializeField]
    float _changeCooltime;
    GameObject _player1go;
    GameObject _player2go;
    Vector3 _player2offset = new Vector3(0f,0.87f,0f);
    

    protected override void Init()
    {
        base.Init();

        _player1go = Util.FindChild(this.gameObject, "Player1");
        _player2go = Util.FindChild(this.gameObject, "Player2");

        if(_player1go != null && _player2go != null)
        {
            _player1go.SetActive(true);
            _player2go.SetActive(false);
        }

        State = CreatureState.Idle;
        ChangeCooltime = _changeCooltime;
    }

    protected override void UpdateController()
    {
        base.UpdateController();
    }

    protected override void UpdateIdle()
    {
        if (_coChangeCooltime == null && Input.GetKey(KeyCode.Space) && _player1go != null && _player2go != null)
        {
            if (_player1go.activeSelf == true)
            {
                _player2go.SetActive(true);
                _player2go.transform.position = _player1go.transform.position + _player2offset;
                _player1go.SetActive(false);

            }
            else
            {
                _player1go.SetActive(true);
                _player1go.transform.position = _player2go.transform.position - _player2offset;
                _player2go.SetActive(false);
            }

            _coChangeCooltime = StartCoroutine("CoChangeCooltime", 0.2f);
        }
    }

    Coroutine _coChangeCooltime;
    IEnumerator CoChangeCooltime(float time)
    {
        yield return new WaitForSeconds(time);
        _coChangeCooltime = null;
    }

    bool Active
    {
        get { return this._spriteRenderer.enabled && this._boxCollider2D.enabled; }
        set
        {
            if (_spriteRenderer == null || _boxCollider2D == null)
                return;

            this._spriteRenderer.enabled = value;
            this._boxCollider2D.enabled = value;
        }
    }
}