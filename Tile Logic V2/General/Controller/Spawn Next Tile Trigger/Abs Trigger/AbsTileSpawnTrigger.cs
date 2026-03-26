using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Логика обнаружения того, что пора спавнить новый таил(обнаруживает последний актуальный таил)
/// </summary>
public abstract class AbsTileSpawnTrigger : MonoBehaviour
{
    public abstract event Action OnInit;
    public abstract bool IsInit { get; }
    
    public abstract event Action<DKOKeyAndTargetAction> OnTriggerSpawnTile;
}
