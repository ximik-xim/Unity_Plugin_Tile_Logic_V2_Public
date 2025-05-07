using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Будет находиться на префабе таила и именно этот скрпит будет служить префабом для таила
/// </summary>
public class MarkTilePrefab : MonoBehaviour
{
   /// <summary>
   /// вынес его отдельно, т.к в отличии от остальной логики этот скрипт обязательно должен быть на таиле
   /// </summary>
   [SerializeField] 
   private AbsTilePositionInfo _tilePositionInfo;

   [SerializeField] 
   private DKOKeyAndTargetAction _tileDKO;

   public AbsTilePositionInfo GetTilePositionInfo()
   {
      return _tilePositionInfo;
   }

   public DKOKeyAndTargetAction GetTileDKO()
   {
      return _tileDKO;
   }
}
