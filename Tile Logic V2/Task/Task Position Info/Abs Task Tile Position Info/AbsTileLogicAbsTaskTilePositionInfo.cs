using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsTileLogicAbsTaskTilePositionInfo : MonoBehaviour
{
    public abstract event Action OnInit;
    public abstract bool IsInit { get; }
    
    public abstract event Action OnCompletedLogic;
    public abstract bool IsCompletedLogic { get; }

    public abstract void StartLogic(SetPositionTileData tileInfo);
}
