using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    public static Managers Instance { get { Init(); return s_instance; } }

    #region Contents
    ObjectManager _obj = new ObjectManager();
    PlayerManager _playerManager = new PlayerManager();

    public static ObjectManager Object { get { return Instance._obj; } }
    public static PlayerManager PlayerManager { get { return Instance._playerManager; } }
    #endregion

    #region Core
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    InputManager _input = new InputManager();
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static InputManager Input { get { return Instance._input; } }
    #endregion

    void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        _input.OnUpdate();
        _playerManager.OnUpdate();
    }

    static void Init()
    {
        if (s_instance != null) return;

        GameObject go = GameObject.Find("@Managers");
        if (go == null)
        {
            go = new GameObject { name = "@Managers" };
            go.AddComponent<Managers>();
        }

        DontDestroyOnLoad(go);
        s_instance = go.GetComponent<Managers>();

        PlayerManager.Init();
    }

    public static void Clear()
    {

    }
}
