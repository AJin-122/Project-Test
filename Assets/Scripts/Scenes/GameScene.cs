using UnityEngine;

public class GameScene : BaseScene
{
    //UI_GameScene _sceneUI;

    protected override void Init()
    {
        base.Init();

        SceneType = Scene.GameScene;

        Screen.SetResolution(640, 480, false);

        //_sceneUI = Managers.UI.ShowSceneUI<UI_GameScene>();

        //PlayerController Player = Managers.Object.Add<PlayerController>(new PlayerController()
        //{
        //    Info = new ObjectInfo()
        //    {
        //        name = "Player",
        //        posInfo = new PosInfo() { pos = new Vector2(-10, -3), state = CreatureState.Idle },
        //        statInfo = new StatInfo() { hp = 10, maxHp = 10, sp = 10, maxSp = 10, changeCooltime = 0.2f },
        //        objectType = GameObjectType.Player
        //    }
        //});
    }

    public override void Clear()
    {

    }
}