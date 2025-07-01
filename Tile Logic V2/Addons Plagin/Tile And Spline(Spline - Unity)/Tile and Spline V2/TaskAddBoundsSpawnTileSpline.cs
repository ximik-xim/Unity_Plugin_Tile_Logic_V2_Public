using System;
using UnityEngine;

public class TaskAddBoundsSpawnTileSpline : TL_AbsTaskLogicDKO
{
    public override event Action OnInit
    {
        add
        {
            _bufferUseTile.OnInit += value;
        }
        remove
        {
            _bufferUseTile.OnInit -= value;
        }
    }
    public override bool IsInit => _bufferUseTile.IsInit;
    
    public override event Action OnCompletedLogic;
    public override bool IsCompletedLogic => _isCompletedLogic;
    private bool _isCompletedLogic = true;
    
    [SerializeField] 
    private BufferUseTile _bufferUseTile;
    

    public override void StartLogic(DKOKeyAndTargetAction tileDKO)
    {
        _isCompletedLogic = false;
        
        _bufferUseTile.OnCompletedAddTile -= OnCompletedAddTile;
        _bufferUseTile.OnCompletedAddTile += OnCompletedAddTile;
        _bufferUseTile.AddTile(tileDKO);
    }

    private void OnCompletedAddTile()
    {
        _bufferUseTile.OnCompletedAddTile -= OnCompletedAddTile;
        
        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
