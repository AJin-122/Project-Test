using System;
using UnityEngine;

public class ObjectInfo
{
    public string name;
    public PosInfo posInfo;
    public StatInfo statInfo;
    public GameObjectType objectType;
}

public class StatInfo : IEquatable<StatInfo>
{
    public int hp;
    public int maxHp;
    public int sp;
    public int maxSp;
    public float changeCooltime;

    // IEquatable<T> 인터페이스 구현
    public bool Equals(StatInfo other)
    {
        if (other == null) return false;
        return hp == other.hp && maxHp == other.maxHp &&
               sp == other.sp && maxSp == other.maxSp &&
               Math.Abs(changeCooltime - other.changeCooltime) < 0.0001f;
    }

    public override bool Equals(object obj) => Equals(obj as StatInfo);

    public override int GetHashCode()
    {
        return HashCode.Combine(hp, maxHp, sp, maxSp, changeCooltime);
    }
}


public class PosInfo : IEquatable<PosInfo>
{
    public CreatureState state;
    public Vector2 pos;

    public bool Equals(PosInfo other)
    {
        if (other == null) return false;
        return state == other.state && pos == other.pos;
    }

    public override bool Equals(object obj) => Equals(obj as PosInfo);

    public override int GetHashCode() => HashCode.Combine(state, pos);
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