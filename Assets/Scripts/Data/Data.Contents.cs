using System;
using UnityEngine;

public class ObjectInfo
{
    public string name;
    public PosInfo posInfo;
    public StatInfo statInfo;
    public GameObjectType objectType;
}

public class StatInfo
{
	public int hp;
	public int maxHp;
	public int sp;
	public int maxSp;
	public float changeCooltime;
}

public class PosInfo
{
	public CreatureState state;
	public Vector2 pos;
}

public enum CreatureState
{
    Idle = 0,
    Moving = 1,
    Skill = 2,
    Dead = 3
}

public enum GameObjectType
{
    None = 0,
    Player = 1,
    Monster = 2,
    Projectile = 3
}

public enum Scene
{
    UnknownScene,
    LoginScene,
    LobbyScene,
    GameScene,
}