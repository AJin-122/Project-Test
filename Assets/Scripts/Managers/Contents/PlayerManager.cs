using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerManager
{
    private readonly Dictionary<Type, Action<Component, bool>> _componentActions;
    GameObject _player1go;
    GameObject _player2go;
    Player1Controller _player1con;
    Player2Controller _player2con;
    Transform playerSpawnPoint;
    bool isPlayer1Active = false;
    private static readonly Vector3 _player2offset = new Vector3(0f, 0.87f, 0f);

    public GameObject CurrentPlayer => isPlayer1Active ? _player1go : _player2go;

    public PlayerManager()
    {
        _componentActions = new Dictionary<Type, Action<Component, bool>>
        {
            { typeof(SpriteRenderer), (comp, isActive) => ((SpriteRenderer)comp).enabled = isActive },
            { typeof(Rigidbody2D), (comp, isActive) => ((Rigidbody2D)comp).simulated = isActive },
            { typeof(Collider2D), (comp, isActive) => ((Collider2D)comp).enabled = isActive },
            { typeof(Animator), (comp, isActive) => ((Animator)comp).enabled = isActive },
            //{ typeof(AudioSource), (comp, isActive) => ((AudioSource)comp).mute = !isActive }
        };
    }

    public void Init()
    {
        if (_player1go != null || _player2go != null) return;

        _player1go = Managers.Resource.Instantiate("Creature/Player1");
        _player2go = Managers.Resource.Instantiate("Creature/Player2");
        _player1con = _player1go.GetOrAddComponent<Player1Controller>();
        _player2con = _player2go.GetOrAddComponent<Player2Controller>();
        playerSpawnPoint = GameObject.FindGameObjectWithTag("spawnPoint").transform;

        _player1go.transform.position = playerSpawnPoint.position;
        _player2go.transform.position = playerSpawnPoint.position + _player2offset;

        ChangePlayer();
    }

    public bool SetActive(BaseController player, bool isActive)
    {
        bool success = true;
        var missingComponents = new List<string>();

        foreach (var entry in _componentActions)
        {
            if (player.TryGetComponent(entry.Key, out Component component))
            {
                try
                {
                    entry.Value(component, isActive);
                }
                catch (Exception e)
                {
                    Debug.LogError($"컴포넌트 활성화 중 오류 발생: {player.name}의 {entry.Key.Name} - {e.Message}");
                    success = false;
                }
            }
            else
            {
                missingComponents.Add(entry.Key.Name);
                success = false;
            }
        }

        if (missingComponents.Count > 0)
        {
            Debug.LogWarning($"SetActive 경고: {player.name}에 다음 컴포넌트가 없습니다: {string.Join(", ", missingComponents)}");
        }

        return success;
    }

    public void ChangePlayer()
    {
        // 현재 활성화된 플레이어의 위치 저장
        Vector3 currentActivePos = CurrentPlayer.transform.position;
        
        isPlayer1Active = !isPlayer1Active;
        
        // 위치 교환 (player2인 경우 offset 적용)
        CurrentPlayer.transform.position = isPlayer1Active ? currentActivePos - _player2offset : currentActivePos + _player2offset;
        
        // 현재 활성화된 플레이어 비활성화
        SetActive(isPlayer1Active ? _player2con : _player1con, false);
        
        // 새로 활성화할 플레이어 활성화
        SetActive(isPlayer1Active ? _player1con : _player2con, true);
    }

    public void OnUpdate()
    {
        // InputManager를 통해 입력 체크
        if (Managers.Input.IsPlayerSwitchPressed)
        {
            ChangePlayer();
        }
    }
}