using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGameSubsystem<T> : IListenerScriptableObject<T> where T : IListenerInterface
{
    public abstract void GameStart();

    public abstract void GameTick();
    public abstract void GameFinish();
}
