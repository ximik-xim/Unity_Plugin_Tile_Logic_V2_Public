using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Через тип получает список возможных префабов таилов.
/// Затем через логику выбора конкретного экземпляра префаба, выбирает префаб
/// И отправляет его на спавн в фабрику 
/// </summary>
public class GetExampleTileTypeDefault : AbsGetExampleTileType
{
    [SerializeField] 
    private AbsLogicSelectPrefabTile _logicSelectPrefabTile;

    [SerializeField] 
    private AbsGetDataListTypeTile _typePrefabs;

    [SerializeField]
    private FabricTile _fabric;
    
    [SerializeField] 
    private GetDataSODataDKODataKey _keySetTypeTile;
    
    /// <summary>
    /// Вернет заспавненый экземпляр таила
    /// </summary>
    public override MarkTilePrefab GetTileExample(KeyExampleTipeTile type)
    {
        List<MarkTilePrefab> listTiles = _typePrefabs.GetTiles(type);
        var prefabTile = _logicSelectPrefabTile.GetTile(listTiles);
        var examplePrefab= _fabric.GetExample(prefabTile);
        
        var data = (DKODataSetKeyTipeTile)examplePrefab.GetTileDKO().KeyRun(_keySetTypeTile.GetData());
        data.SetKey(type);
        
        return examplePrefab;
    }
}
