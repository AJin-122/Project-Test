using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Scene SceneType { get; protected set; } = Scene.UnknownScene;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        //Object obj = GameObject.FindAnyObjectByType(typeof(EventSystem));
        //if (obj == null)
            //Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();
}