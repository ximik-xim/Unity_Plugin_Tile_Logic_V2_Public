using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnTriggerColliderGizmo2D : AbsTileSpawnTrigger
{
    public override event Action OnInit;
    public override bool IsInit => true;
    
    public override event Action<DKOKeyAndTargetAction> OnTriggerSpawnTile;
    
    [SerializeField] 
    private bool _triggerEnter;

    [SerializeField] 
    private bool _triggerExit;
    
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggerEnter == true) 
        {
            GetTileDKO(other);    
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_triggerExit == true) 
        {
            GetTileDKO(other);    
        }
    }

    private void GetTileDKO(Collider2D other)
    {
        AbsTileSpawnMarker marker = other.GetComponent<AbsTileSpawnMarker>();
        
        if (marker != null)
        {
            var tileDKO = marker.GetTileDKO();
            OnTriggerSpawnTile?.Invoke(tileDKO);
        }
    }
    
}
