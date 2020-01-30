using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;


public class GameManager : MonoBehaviour, ISingleton
{
    #region Instance 单例及初始化
    public static GameManager Instance
    {
        get { return MonoSingletonProperty<GameManager>.Instance; }
    }

    public void OnSingletonInit() {

    }
    #endregion

    public LevelManager LevelManager;
}
