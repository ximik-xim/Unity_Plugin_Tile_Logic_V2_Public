using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Просто спавнит указанный префаб и возращает экземпляр
/// </summary>
public class FabricTile : MonoBehaviour
{
    public MarkTilePrefab GetExample(MarkTilePrefab prefab)
    {
        var examplePrefab = Instantiate(prefab);
            
        return examplePrefab;
    }
}
