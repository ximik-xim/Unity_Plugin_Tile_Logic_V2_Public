using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DKOGetInfoPositionCenterCollider : DKOTargetAction
{
    [SerializeField] 
    private GetInfoPositionCenterCollider _infoPositionCenterCollider;

    private DKODataGetInfoPositionCenterCollider _data;
    
    private void Awake()
    {
        _data = new DKODataGetInfoPositionCenterCollider(_infoPositionCenterCollider);
        LocalAwake();
    }

    protected override DKODataRund InvokeRun()
    {
        if (_data == null)
        {
            _data = new DKODataGetInfoPositionCenterCollider(_infoPositionCenterCollider);
        }
        
        return _data;
    }
    
}

public class DKODataGetInfoPositionCenterCollider : DKODataRund
{
    public DKODataGetInfoPositionCenterCollider(GetInfoPositionCenterCollider infoPositionCenterCollider)
    {
        _infoPositionCenterCollider = infoPositionCenterCollider;
    }
    
    private GetInfoPositionCenterCollider _infoPositionCenterCollider;

    public GetInfoPositionCenterCollider InfoPositionCenterCollider => _infoPositionCenterCollider;
}

