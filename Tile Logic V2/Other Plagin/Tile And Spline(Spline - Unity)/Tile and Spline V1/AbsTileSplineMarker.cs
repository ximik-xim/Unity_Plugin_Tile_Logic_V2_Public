using UnityEngine;

public class AbsTileSplineMarker : MonoBehaviour
{
    [SerializeField] 
    protected DKOKeyAndTargetAction _keyAndTargetAction;

    /// <summary>
    /// Тип маркера у тала
    /// Нужен для определения, это начало таила или конец таила
    /// </summary>
    [SerializeField] 
    protected TypeTileSplineMarker _typeTileSplineMarker;
    
    public DKOKeyAndTargetAction GetTileDKO()
    {
        return _keyAndTargetAction;
    }

    public TypeTileSplineMarker GetTileSplineMarker()
    {
        return _typeTileSplineMarker;
    }
    
}

public enum TypeTileSplineMarker
{
    BeginTile,
    EndTile
}
