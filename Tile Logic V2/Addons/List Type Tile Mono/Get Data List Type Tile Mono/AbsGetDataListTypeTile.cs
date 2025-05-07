
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Нужно, что бы мог получить список талов по типу как с monobeh, так и с SO(но тоже через переходник в виде monobeh)
/// </summary>
public abstract class AbsGetDataListTypeTile : MonoBehaviour
{
    public abstract event Action OnInit;
    public abstract bool IsInit { get; }
    
    public abstract List<MarkTilePrefab> GetTiles(KeyExampleTipeTile key);
    public abstract List<KeyExampleTipeTile> GetAllType();
}
