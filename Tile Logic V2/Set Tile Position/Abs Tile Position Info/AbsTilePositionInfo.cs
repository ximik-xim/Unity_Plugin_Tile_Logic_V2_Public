using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// будет находиться на таиле и определять положение таила при установки его в цепочку из талов
/// </summary>
public abstract class AbsTilePositionInfo : MonoBehaviour
{ 
   [SerializeField] 
   protected GameObject _transformTile;
   
   public Transform GetTransformTile()
   {
      return _transformTile.transform;
   }

   /// <summary>
   /// нужна на случай если точка установки таила будет смещена 
   /// </summary>
   public abstract Vector3 GetOffsetTile();

}
