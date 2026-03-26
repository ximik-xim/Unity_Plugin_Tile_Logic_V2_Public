using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Логика обнаружения того, что пора уничтожать(выключать) этот таил
/// </summary>
public abstract class AbsTileDeadTrigger : MonoBehaviour
{
    public abstract event Action OnInit;
    public abstract bool IsInit { get; }
    
    public abstract event Action<DKOKeyAndTargetAction> OnTriggerDeadTile;
}
