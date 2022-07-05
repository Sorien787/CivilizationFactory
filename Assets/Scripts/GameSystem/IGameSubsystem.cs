using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGameSubsystem : ScriptableObject
{
    public virtual void GameStart() { }

    public virtual void GameTick() { }
    public virtual void GameFinish() { }
}
