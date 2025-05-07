using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Основная логика управления таилами
/// (когда спавнить, когда отключать таилы и т.д)
/// !!! ЛОГИКА СОЗДАНИЯ ПЕРВЫХ ТАИЛОВ БУДЕТ ДЛЯ КАЖДОЙ РЕАЛИЗАЦИИ РАЗНАЯ, ТАК ЖЕ КАК И ЛОГИКА ДВИЖЕНИЯ ТАИЛОВ,
/// ПО ЭТОМУ ЭТИ ДВЕ ЛОГИКИ ИДУТ ПОЛНОСТЬЮ САМОСТОЯТЕЛЬНЫМИ И ОТДЕЛЬНЫМИ 
/// </summary>
public class TileLogicMono : MonoBehaviour
{
    public event Action OnInit;
    public bool IsInit => _isInit;
    private bool _isInit = false;
    
    [SerializeField]
    private AbsTileDeadTrigger _tileDeadTrigger;

    [SerializeField]
    private TileLogicGroupLogicTileDead _tileLogicGroupLogicTileDead;

    [SerializeField]
    private AbsTileSpawnTrigger _tileSpawnTrigger;

    [SerializeField]
    private TileLogicGroupLogicTileSpawn _tileLogicGroupLogicTileSpawn;

    [SerializeField]
    private AbsGetNextTile _getNextTile;
    
    //пока так, не буду тут делать группу, посмотрю нужно ли в дальнейшем ....
    [SerializeField]
    private SetPositionTile _setPositionTile;

    private List<DKOKeyAndTargetAction> _bufferTileDead = new List<DKOKeyAndTargetAction>();
    
    private List<MarkTilePrefab> _bufferTileSpawn = new List<MarkTilePrefab>();
    private MarkTilePrefab _bufferCurrentTileSpawn;
    
    private List<SetPositionTileData> _bufferTileSetPosition = new List<SetPositionTileData>();
    

    private void Awake()
    {
        if (_tileDeadTrigger.IsInit == false)
        {
            _tileDeadTrigger.OnInit += OnInit1;
        }
        
        if (_tileLogicGroupLogicTileDead.IsInit == false)
        {
            _tileLogicGroupLogicTileDead.OnInit += OnInit2;
        }
        
        if (_tileSpawnTrigger.IsInit == false)
        {
            _tileSpawnTrigger.OnInit += OnInit3;
        }
        
        if (_tileLogicGroupLogicTileSpawn.IsInit == false)
        {
            _tileLogicGroupLogicTileSpawn.OnInit += OnInit4;
        }
        
        if (_getNextTile.IsInit == false)
        {
            _getNextTile.OnInit += OnInit5;
        }
        
        if (_setPositionTile.IsInit == false)
        {
            _setPositionTile.OnInit += OnInit6;
        }

        CheckInit();
    }

    private void OnInit1()
    {
        _tileDeadTrigger.OnInit -= OnInit1;
        CheckInit();
    }

    private void OnInit2()
    {
        _tileLogicGroupLogicTileDead.OnInit -= OnInit2;
        CheckInit();
    }

    private void OnInit3()
    {
        _tileSpawnTrigger.OnInit -= OnInit3;
        CheckInit();
    }

    private void OnInit4()
    {
        _tileLogicGroupLogicTileSpawn.OnInit -= OnInit4;
        CheckInit();
    }

    private void OnInit5()
    {
        _getNextTile.OnInit -= OnInit5;
        CheckInit();
    }

    private void OnInit6()
    {
        _setPositionTile.OnInit -= OnInit6;
        CheckInit();
    }


    private void CheckInit()
    {
        if (_isInit == false) 
        {
            if (_tileDeadTrigger.IsInit == true && _tileLogicGroupLogicTileDead.IsInit == true && _tileSpawnTrigger.IsInit == true && _tileLogicGroupLogicTileSpawn.IsInit == true && _getNextTile.IsInit == true && _setPositionTile.IsInit == true)
            {
                Init();
                
                _isInit = true;
                OnInit?.Invoke();
            }
        }
    }


    private void Init()
    {
        _tileDeadTrigger.OnTriggerDeadTile += OnTriggerDeadTile;
        _tileSpawnTrigger.OnTriggerSpawnTile += OnTriggerSpawnTile;
    }

    private void OnTriggerDeadTile(DKOKeyAndTargetAction tileDKO)
    {
        if (_tileLogicGroupLogicTileDead.IsCompleted == false)
        {
            _tileLogicGroupLogicTileDead.OnCompleted -= OnCompletedLogicTileDead;
            _tileLogicGroupLogicTileDead.OnCompleted += OnCompletedLogicTileDead;
                
            _bufferTileDead.Add(tileDKO);
            return;
        }
                
        _tileLogicGroupLogicTileDead.StartAction(tileDKO);
    }

    private void OnCompletedLogicTileDead()
    {
        if (_tileLogicGroupLogicTileDead.IsCompleted == true)
        {
            DKOKeyAndTargetAction tileDKO = _bufferTileDead[0];
            _bufferTileDead.RemoveAt(0);
            
            if (_bufferTileDead.Count == 0)
            {
                _tileLogicGroupLogicTileDead.OnCompleted -= OnCompletedLogicTileDead;    
            }
            
            _tileLogicGroupLogicTileDead.StartAction(tileDKO);
        }
    }

    private void OnTriggerSpawnTile(DKOKeyAndTargetAction lastTileDKO)
    {
        MarkTilePrefab prefabTile = _getNextTile.GetNextTile(lastTileDKO);
        
        if (_tileLogicGroupLogicTileSpawn.IsCompleted == false)
        {
            _tileLogicGroupLogicTileSpawn.OnCompleted -= OnCheckCompletedLogicTileSpawn;
            _tileLogicGroupLogicTileSpawn.OnCompleted += OnCheckCompletedLogicTileSpawn;
                
            _bufferTileSpawn.Add(prefabTile);
            return;
        }
        
        _bufferCurrentTileSpawn = prefabTile;
        
        _tileLogicGroupLogicTileSpawn.OnCompleted += OnCompletedLogicTileSpawn;
        _tileLogicGroupLogicTileSpawn.StartAction(prefabTile.GetTileDKO());
      
    }

    private void OnCheckCompletedLogicTileSpawn()
    {
        if (_tileLogicGroupLogicTileSpawn.IsCompleted == true)
        {
            MarkTilePrefab tilePrefab = _bufferTileSpawn[0];
            _bufferTileSpawn.RemoveAt(0);
            
            if (_bufferTileSpawn.Count == 0)
            {
                _tileLogicGroupLogicTileSpawn.OnCompleted -= OnCheckCompletedLogicTileSpawn;    
            }

            _bufferCurrentTileSpawn = tilePrefab;
            
            _tileLogicGroupLogicTileSpawn.OnCompleted += OnCompletedLogicTileSpawn;
            _tileLogicGroupLogicTileSpawn.StartAction(tilePrefab.GetTileDKO());
        }
    }

    private void OnCompletedLogicTileSpawn()
    {
        _tileLogicGroupLogicTileSpawn.OnCompleted -= OnCompletedLogicTileSpawn;

        SetPositionTileData positionTileData = new SetPositionTileData(_bufferCurrentTileSpawn.GetTileDKO(), _bufferCurrentTileSpawn.GetTilePositionInfo());
        if (_setPositionTile.IsCompleted == false)
        {
            _setPositionTile.OnCompleted -= OnCheckCompletedLogicTileSetPosition;
            _setPositionTile.OnCompleted += OnCheckCompletedLogicTileSetPosition;
                
            _bufferTileSetPosition.Add(positionTileData);
            return;
        }
        
        _setPositionTile.OnCompleted += OnCompletedLogicTileSetPosition;
        _setPositionTile.StartAction(positionTileData);
    }

    private void OnCheckCompletedLogicTileSetPosition()
    {
        if (_setPositionTile.IsCompleted == true)
        {
            SetPositionTileData tileInfo = _bufferTileSetPosition[0];
            _bufferTileSetPosition.RemoveAt(0);
            
            if (_bufferTileSetPosition.Count == 0)
            {
                _setPositionTile.OnCompleted -= OnCheckCompletedLogicTileSetPosition;    
            }
            
            _setPositionTile.OnCompleted += OnCompletedLogicTileSetPosition;
            _setPositionTile.StartAction(tileInfo);
        }
    }

    private void OnCompletedLogicTileSetPosition()
    {
        _setPositionTile.OnCompleted -= OnCompletedLogicTileSetPosition;
    }


    private void OnDestroy()
    {
        _tileDeadTrigger.OnTriggerDeadTile -= OnTriggerDeadTile;
        _tileSpawnTrigger.OnTriggerSpawnTile -= OnTriggerSpawnTile;
    }
}
