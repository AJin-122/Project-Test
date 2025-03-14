using System.Collections.Generic;
using System;
using UnityEngine;

public class ObjectManager
{
    Dictionary<int, BaseController> _objects = new Dictionary<int, BaseController>();
    int _counter = 0;

    public static GameObjectType GetObjectTypeById(int id)
    {
        int type = (id >> 24) & 0x7F;
        return (GameObjectType)type;
    }

    int GenerateId(GameObjectType type)
    {
        return ((int)type << 24) | (_counter++);
    }

    public T Add<T>(T bc) where T : BaseController
    {
        if (_objects.ContainsKey(bc.Id))
            return bc as T;

        bc.Id = GenerateId(bc.Info.objectType);

        switch (bc.Info.objectType)
        {
            case GameObjectType.Player:

                break;
        }

        return bc as T;
    }

    public void Remove(int id)
    {
        if (_objects.ContainsKey(id) == false)
            return;

        BaseController bc = FindById(id);
        if (bc == null)
            return;

        _objects.Remove(id);
        Managers.Resource.Destroy(bc.gameObject);
    }

    public BaseController FindById(int id)
    {
        BaseController bc = null;
        _objects.TryGetValue(id, out bc);
        return bc;
    }

    public BaseController FindCreature(Vector2 pos)
    {
        foreach (BaseController bc in _objects.Values)
        {
            CreatureController cc = bc.gameObject.GetComponent<CreatureController>();
            if (cc == null)
                continue;
            if (cc.PosInfo.pos == pos)
                return bc;
        }

        return null;
    }

    public BaseController Find(Func<BaseController, bool> condition)
    {
        foreach (BaseController bc in _objects.Values)
        {
            if (condition.Invoke(bc))
                return bc;
        }

        return null;
    }

    public void Clear()
    {
        foreach (BaseController bc in _objects.Values)
            Managers.Resource.Destroy(bc.gameObject);

        _objects.Clear();
    }
}