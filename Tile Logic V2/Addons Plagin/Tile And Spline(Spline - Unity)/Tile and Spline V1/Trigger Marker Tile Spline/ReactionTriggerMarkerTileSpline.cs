using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

public class ReactionTriggerMarkerTileSpline : MonoBehaviour
{
    [SerializeField] 
    private TriggerMarkerTileSpline _triggerSpline;

    [SerializeField] 
    private SplineOffsetSplineContainer _offsetSplineContainer;

    [SerializeField] 
    private SplineContainer _setContainer;

    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetData;
    
    private void Awake()
    {
        _triggerSpline.OnTrigger += OnTrigger;
    }

    private void OnTrigger(AbsTileSplineMarker marker)
    {
        if (marker.GetTileSplineMarker() == TypeTileSplineMarker.BeginTile)
        {
            var data = (DKODataInfoT<AbsGetSplineContainer>)marker.GetTileDKO().KeyRun(_keyGetData.GetData());
            var getContainer = data.Data.GetContainer();
            
            _offsetSplineContainer.AddSpline(_setContainer, getContainer);
        }
        
        if (marker.GetTileSplineMarker() == TypeTileSplineMarker.EndTile)
        {
            var data = (DKODataInfoT<AbsGetSplineContainer>)marker.GetTileDKO().KeyRun(_keyGetData.GetData());
            var getContainer = data.Data.GetContainer();
            
            _offsetSplineContainer.RemoveSpline(_setContainer, getContainer);
        }
    }

    private void OnDestroy()
    {
        _triggerSpline.OnTrigger -= OnTrigger;
    }
}
