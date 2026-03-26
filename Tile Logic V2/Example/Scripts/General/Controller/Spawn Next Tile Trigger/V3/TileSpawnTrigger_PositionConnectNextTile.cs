using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnTrigger_PositionConnectNextTile : AbsTileSpawnTrigger
{
    public override event Action OnInit;
    public override bool IsInit => true;
    
    public override event Action<DKOKeyAndTargetAction> OnTriggerSpawnTile;
    
    [SerializeField] 
    private bool _triggerEnter;

    [SerializeField] 
    private bool _triggerExit;

    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetDataPositionConnect;
  
    [SerializeField] 
    private AbsGetInfoPositionConnectNextTile _positionConnectInfo;
    
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

    private void OnTriggerEnter(Collider other)
    {
        if (_triggerEnter == true) 
        {
            GetTileDKO(other);    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_triggerExit == true) 
        {
            GetTileDKO(other);    
        }
    }

    private void GetTileDKO(Collider other)
    {
        AbsTileSpawnMarker marker = other.GetComponent<AbsTileSpawnMarker>();
        
        if (marker != null)
        {
            var tileDKO = marker.GetTileDKO();

            var dataPositionConnect = (DKODataGetInfoPositionConnectNextTile)tileDKO.KeyRun(_keyGetDataPositionConnect.GetData());
            var positionConnect = dataPositionConnect.PositionConnectNextTileInfo.GetPositionConnectNextTile();
            
            _positionConnectInfo.SetPositionConnectNextTile(positionConnect);
            
            
            OnTriggerSpawnTile?.Invoke(tileDKO);

            //для тестов
            //Time.timeScale = 0;
        }
    }
}
