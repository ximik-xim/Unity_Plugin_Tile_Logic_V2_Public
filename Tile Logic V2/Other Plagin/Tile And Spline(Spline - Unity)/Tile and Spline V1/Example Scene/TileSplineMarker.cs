using UnityEngine;

public class TileSplineMarker : AbsTileSplineMarker
{
#if  UNITY_EDITOR
    
    [SerializeField] 
    private Color _colorGizmo = Color.cyan;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _colorGizmo;
        Gizmos.DrawCube(this.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
    }
    
#endif
}
