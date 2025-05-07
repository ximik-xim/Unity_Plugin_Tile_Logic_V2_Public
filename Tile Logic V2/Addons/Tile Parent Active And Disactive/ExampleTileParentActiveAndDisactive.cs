using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleTileParentActiveAndDisactive : AbsTileParentActiveAndDisactive
{
    [SerializeField] 
    private MeshRenderer _renderer;
    
    public override void ActiveTile()
    {
        _renderer.enabled = true;
    }

    public override void DisactiveTile()
    {
        _renderer.enabled = false;
    }
}
