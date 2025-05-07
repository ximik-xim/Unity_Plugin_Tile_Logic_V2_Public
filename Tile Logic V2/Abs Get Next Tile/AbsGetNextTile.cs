
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsGetNextTile : MonoBehaviour
{
   public abstract event Action OnInit;
   public abstract bool IsInit { get; }
   
   //если буду делать через event, то нужно будет сделать возращаемый класс с этим event и со ссылкой на возращаемые данные, когда event сработает, значит пришли данные
   //не обезательно, но как возможность, можно передать DKO таила который вызвал триггер о том что пора спавнить след. таил(получаеться будет DKO последнего в цепочке таила)
   public abstract MarkTilePrefab GetNextTile(DKOKeyAndTargetAction lastTileDKO = null);
}
