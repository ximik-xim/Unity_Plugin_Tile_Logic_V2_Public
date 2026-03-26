using System;
using UnityEngine;
using UnityEngine.Splines;

public class TaskSetPositionGGSpawnTileSpline : TL_AbsTaskLogicDKO
{
    public override event Action OnInit;
    public override bool IsInit => true;
    
    public override event Action OnCompletedLogic;
    public override bool IsCompletedLogic => _isCompletedLogic;
    private bool _isCompletedLogic = true;
    
    [SerializeField] 
    private SplineContainer _setContainer;

    [SerializeField] 
    private SplineOffsetSplineContainer _splineOffset;
    
    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetData;

    [SerializeField] 
    private SetGMPositionSpliteAndRb _setGmPosition;
    
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

    public override void StartLogic(DKOKeyAndTargetAction tileDKO)
    {
        _isCompletedLogic = false;
        
        var data = (DKODataInfoT<AbsGetSplineContainer>)tileDKO.KeyRun(_keyGetData.GetData());
        var getContainer = data.Data.GetContainer();

        _splineOffset.AddSpline(_setContainer, getContainer);

        _setGmPosition.StartLogic();
        
        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
