using UnityEngine;

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