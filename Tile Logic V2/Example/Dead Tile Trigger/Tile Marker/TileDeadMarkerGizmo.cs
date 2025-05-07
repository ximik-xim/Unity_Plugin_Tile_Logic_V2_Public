using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDeadMarkerGizmo : AbsTileDeadMarker
{
#if  UNITY_EDITOR
    
    [SerializeField] 
    private Color _colorGizmo = Color.red;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _colorGizmo;
        Gizmos.DrawCube(this.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
    }
    
#endif
}
