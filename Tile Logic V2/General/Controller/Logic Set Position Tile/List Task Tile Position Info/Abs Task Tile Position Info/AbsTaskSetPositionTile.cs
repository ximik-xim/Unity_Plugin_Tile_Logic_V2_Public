using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Наследники этого класса, это задачи которые будут вызываться из списка задач при установки позиции тайла
/// </summary>
public abstract class AbsTaskSetPositionTile : MonoBehaviour
{
    public abstract event Action OnInit;
    public abstract bool IsInit { get; }
    
    public abstract event Action OnCompletedLogic;
    public abstract bool IsCompletedLogic { get; }

    public abstract void StartLogic(SetPositionTileData tileInfo);
}
