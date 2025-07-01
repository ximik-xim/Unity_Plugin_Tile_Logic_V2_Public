using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BufferUseTileRemoveTile : TL_AbsTaskLogicDKO
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
        
        _bufferUseTile.OnCompletedRemoveTile -= OnCompletedRemoveTile;
        _bufferUseTile.OnCompletedRemoveTile += OnCompletedRemoveTile;
        _bufferUseTile.RemoveTile(tileDKO);
    }

    private void OnCompletedRemoveTile()
    {
        _bufferUseTile.OnCompletedRemoveTile -= OnCompletedRemoveTile;
        
        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
