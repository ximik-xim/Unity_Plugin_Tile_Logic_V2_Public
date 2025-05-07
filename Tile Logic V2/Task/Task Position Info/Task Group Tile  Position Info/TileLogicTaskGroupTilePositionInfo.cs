using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileLogicTaskGroupTilePositionInfo : MonoBehaviour
{
    public event Action OnInit;
    public bool IsInit => _isInit;
    private bool _isInit = false;
    
    public event Action OnCompleted;
    public bool IsCompleted => _isCompleted;
    private bool _isCompleted = true;
    
    [SerializeField]
    private TileLogicListTaskTilePositionInfo _beforeTask;

    [SerializeField]
    private TileLogicListTaskTilePositionInfo _mainTask;
    
    [SerializeField]
    private TileLogicListTaskTilePositionInfo _afterTask;
    
    private SetPositionTileData _bufferTile;

    /// <summary>
    /// Желательно вызывать в Awake
    /// </summary>
    protected void StartInit()
    {
        if (_beforeTask.IsInit == false)
        {
            _beforeTask.OnInit += OnInitBeforeTask;
        }
        
        if (_mainTask.IsInit == false)
        {
            _mainTask.OnInit += OnInitMainTask;
        }
        
        if (_afterTask.IsInit == false)
        {
            _afterTask.OnInit += OnInitAfterTask;
        }

        InitCheck();
    }

    private void OnInitBeforeTask()
    {
        _beforeTask.OnInit -= OnInitBeforeTask;
        InitCheck();
    }

    private void OnInitMainTask()
    {
        _mainTask.OnInit -= OnInitMainTask;
        InitCheck();
    }

    private void OnInitAfterTask()
    {
        _afterTask.OnInit -= OnInitAfterTask;
        InitCheck();
    }

    private void InitCheck()
    {
        if (_isInit == false) 
        {
            if (_beforeTask.IsInit == true && _mainTask.IsInit == true && _afterTask.IsInit == true)
            {
                _isInit = true;
                OnInit?.Invoke();
            }
        }
       
    }

    public void StartAction(SetPositionTileData tileInfo)
    {
        if (_isCompleted == true)
        {
            _isCompleted = false;
            _bufferTile = tileInfo;

            _beforeTask.OnCompleted += OnCompletedBeforeTask;
            _beforeTask.StartAction(_bufferTile);
            
        }
        
    }

    private void OnCompletedBeforeTask()
    {
        _beforeTask.OnCompleted -= OnCompletedBeforeTask;
        
        _mainTask.OnCompleted += OnCompletedMainTask;
        _mainTask.StartAction(_bufferTile);
        
    }

    private void OnCompletedMainTask()
    {
        _mainTask.OnCompleted -= OnCompletedMainTask;
        
        _afterTask.OnCompleted += OnCompletedAfterTask;
        _afterTask.StartAction(_bufferTile);
    }

    private void OnCompletedAfterTask()
    {
        _afterTask.OnCompleted -= OnCompletedAfterTask;

        _isCompleted = true;
        OnCompleted?.Invoke();
    }
}
