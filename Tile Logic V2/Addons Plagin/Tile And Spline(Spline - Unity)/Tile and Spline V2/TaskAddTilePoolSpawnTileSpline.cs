using System;
using UnityEngine;

public class TaskAddTilePoolSpawnTileSpline : TL_AbsTaskLogicDKO
{
   public override event Action OnInit;
   public override bool IsInit => true;
    
   public override event Action OnCompletedLogic;
   public override bool IsCompletedLogic => _isCompletedLogic;
   private bool _isCompletedLogic = true;
   
    
   [SerializeField] 
   private GetDataSODataDKODataKey _keyGetData;

   [SerializeField] 
   private PoolTileType _poolTileType;
    
#if  UNITY_EDITOR
   [SerializeField] 
   private Color _colorGizmo = Color.blue;
    
   private void OnDrawGizmos()
   {
      Gizmos.color = _colorGizmo;
      Gizmos.DrawCube(this.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
   }
    
#endif
    
   private void Awake()
   {
      OnInit?.Invoke();
   }

   public override void StartLogic(DKOKeyAndTargetAction tileDKO)
   {
      _isCompletedLogic = false;
      
      var data = (DKODataInfoT<PoolTileTypeEvent>)tileDKO.KeyRun(_keyGetData.GetData());
      _poolTileType.AddPoolElement(data.Data,true);
      
      _isCompletedLogic = true;
      OnCompletedLogic?.Invoke();
   }
   
  
   
}
