using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnMarkerGizmo : AbsTileSpawnMarker
{
#if  UNITY_EDITOR
    
    [SerializeField] 
    private Color _colorGizmo = Color.blue;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _colorGizmo;
        Gizmos.DrawCube(this.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
    }
    
#endif
}
