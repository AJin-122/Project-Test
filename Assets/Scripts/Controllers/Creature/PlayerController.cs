using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CreatureController
{
    [SerializeField]
    float _changeCooltime;
    GameObject _player1go;
    GameObject _player2go;
    Player1Controller _player1con;
    Player2Controller _player2con;
    Vector3 _player2offset = new Vector3(0f,0.87f,0f);
    

    protected override void Init()
    {
        base.Init();

        //_player1go = Util.FindChild(this.gameObject, "Player1");
        //_player2go = Util.FindChild(this.gameObject, "Player2");

        _player1go = Managers.Resource.Instantiate("Creature/Player1", this.transform);
        _player2go = Managers.Resource.Instantiate("Creature/Player2", this.transform);

        _player1con = _player1go.GetOrAddComponent<Player1Controller>();
        _player2con = _player2go.GetOrAddComponent<Player2Controller>();

        if (_player1go != null && _player2go != null)
        {
            //_player1go.SetActive(true);
            //_player2go.SetActive(false);
            _player1con.Active = true;
            _player2con.Active = false;
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
            if (_player1go.GetComponent<CreatureController>().Active == true)
            {
                //_player2go.SetActive(true);
                //_player2go.transform.position = _player1go.transform.position + _player2offset;
                //_player1go.SetActive(false);

                _player1con.Active = false;
                _player2con.Active = true;
                _player2go.transform.position = _player1go.transform.position + _player2offset;
            }
            else
            {
                //_player1go.SetActive(true);
                //_player1go.transform.position = _player2go.transform.position - _player2offset;
                //_player2go.SetActive(false);

                _player1con.Active = true;
                _player2con.Active = false;
                _player1go.transform.position = _player2go.transform.position - _player2offset;
            }

            _coChangeCooltime = StartCoroutine("CoChangeCooltime", _changeCooltime);
        }
    }

    Coroutine _coChangeCooltime;
    IEnumerator CoChangeCooltime(float time)
    {
        yield return new WaitForSeconds(time);
        _coChangeCooltime = null;
    }

}