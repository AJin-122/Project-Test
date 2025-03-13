using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerManager
{
    GameObject _player1go;
    GameObject _player2go;
    Player1Controller _player1con;
    Player2Controller _player2con;
    Transform playerSpawnPoint;
    bool isPlayer1Active = true;

    public GameObject CurrentPlayer { get { return _player1con.SpriteRenderer.enabled ? _player1go : _player2go; } }

    public void Init()
    {
        _player1go = Managers.Resource.Instantiate("Creature/Player1");
        _player2go = Managers.Resource.Instantiate("Creature/Player2");
        _player1con = _player1go.GetOrAddComponent<Player1Controller>();
        _player2con = _player2go.GetOrAddComponent<Player2Controller>();
        playerSpawnPoint = GameObject.FindGameObjectWithTag("spawnPoint").transform;
    }

    public bool SetActive(BaseController player, bool isActive)
    {
        if (player == null)
            return false;

        Dictionary<Type, Action<Component>> componentActions = new Dictionary<Type, Action<Component>>
        {
            { typeof(SpriteRenderer), (comp) => ((SpriteRenderer)comp).enabled = isActive },
            { typeof(Rigidbody2D), (comp) => ((Rigidbody2D)comp).simulated = isActive },
            { typeof(Collider2D), (comp) => ((Collider2D)comp).enabled = isActive },
            { typeof(Animator), (comp) => ((Animator)comp).enabled = isActive },
            { typeof(AudioSource), (comp) => ((AudioSource)comp).mute = !isActive }
        };

        bool success = true;

        foreach (var entry in componentActions)
        {
            if (player.TryGetComponent(entry.Key, out Component component))
            {
                entry.Value(component); // Ȱ��ȭ/��Ȱ��ȭ ����
            }
            else
            {
                Debug.LogWarning($"TrySetActive ���: {player.name}�� {entry.Key.Name} ������Ʈ�� �����ϴ�.");
                success = false;
            }
        }

        return success;
    }

    public bool TrySetActive(BaseController player, bool isActive)
    {
        if (player == null)
        {
            Debug.LogWarning("TrySetActive ����: player�� null�Դϴ�.");
            return false;
        }

        if (player.TryGetComponent(out SpriteRenderer spriteRenderer))
            spriteRenderer.enabled = isActive;

        if (player.TryGetComponent(out Rigidbody2D rigidbody))
            rigidbody.simulated = isActive;

        return true;
    }


    public void ChangePlayer()
    {


        isPlayer1Active = !isPlayer1Active;
    }
}