
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Логика пула по типу для таилов с типами
/// В данной реализации, все немного замудренно, с целью в дальнейшем полностью отделить логику пула
/// Тут из префаба таила, через его DKO буду получать уже целевой скрипт
/// (и обратно, через то же DKO, буду получать скрипт который являеться префабом таила)
/// !!!ВНИМАНИЕ В ЭТОЙ РЕАЛИЗАЦИИ, В СЛУЧАЕ ИСПОЛЬЗОВАНИЯ СТАРТОВЫХ ТАИЛОВ, НЕОБХОДИМО УБЕДИТЬСЯ, ЧТО ИХ DKO МАКСИМАЛЬНО БЫСТРО ПРОИНИЦИАЛИЗАРУЕТСЯ ПОСЛЕ ЗАПУСКА ПРОЕКАТ!!!
/// (нужно это т.к инициализация скрипта происходит после инициализации стартовых таилов)
/// </summary>
public class PoolTileType : MonoBehaviour
{
    public event Action OnInit;
    public bool IsInit => _isInit;
    private bool _isInit = false;

    [SerializeField] 
    private List<AbsKeyData<GetDataSO_KeyExampleTipeTile, List<MarkTilePrefab>>> _listStartTilePrefab;
    private List<MarkTilePrefab> _buffer = new List<MarkTilePrefab>();
    
    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetDataMarkTile;
    
    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetDataTilePool;
    
    [SerializeField] 
    private CustomPoolKey<string, PoolTileTypeEvent> _examplePoolKey;

    [SerializeField] 
    private AbsLogicSelectPrefabTile _logicSelectPrefabTile;

    [SerializeField] 
    private AbsGetDataListTypeTile _typePrefabs;

    [SerializeField]
    private FabricTile _fabric;
    
    [SerializeField] 
    private GetDataSODataDKODataKey _keySetTypeTile;

    private bool _isStart = false;
    
    private void Awake()
    {
        _examplePoolKey = new CustomPoolKey<string, PoolTileTypeEvent>(CreateData, null, null);
#if UNITY_EDITOR
        _examplePoolKey.UseListInspector = true;
#endif
        StartAction();
    }


    private void StartAction()
    {
        //теперь буду просто блокировать проверку готовности задачи пока не отработает метод StartLogic у всех задач
        _isStart = true;

        foreach (var VARIABLE in _listStartTilePrefab)
        {
            foreach (var VARIABLE2 in VARIABLE.Data)
            {
                if (VARIABLE2.GetTileDKO().IsInit == false)
                {
                    _buffer.Add(VARIABLE2);
                    VARIABLE2.GetTileDKO().OnInit += CheckCompleted;
                }
                else
                {
                    var dataDKO =
                        (DKODataInfoT<PoolTileTypeEvent>)VARIABLE2.GetTileDKO().KeyRun(_keyGetDataTilePool.GetData());
                    PoolTileTypeEvent dataPool = dataDKO.Data;

                    _examplePoolKey.AddPoolElement(dataPool, true);
                }

            }
        }

        _isStart = false;

        CheckCompleted();
    }

    private void CheckCompleted()
    {
        if (_isStart == false)
        {
            int targetCount = _buffer.Count;
            for (int i = 0; i < targetCount; i++)
            {
                if (_buffer[i].GetTileDKO().IsInit == true)
                {
                    var dataDKO = (DKODataInfoT<PoolTileTypeEvent>) _buffer[i].GetTileDKO().KeyRun(_keyGetDataTilePool.GetData());
                    PoolTileTypeEvent dataPool = dataDKO.Data;
                    
                    _examplePoolKey.AddPoolElement(dataPool,true);
                    
                    _buffer[i].GetTileDKO().OnInit -= CheckCompleted;
                    _buffer.RemoveAt(i);
                    i--;
                    targetCount--;
                }
            }

            if (_buffer.Count == 0)
            {
                Completed();
            }
        }
    }

    private void Completed()
    {
        if (_typePrefabs.IsInit == false)
        {
            _typePrefabs.OnInit += OnInitData;
            return;
        }

        Init();
    }
    
    private void OnInitData()
    {
        _typePrefabs.OnInit -= OnInitData;
        Init();
    }

    private void Init()
    {
        if (_isInit == false)
        {
            _isInit = true;
            OnInit?.Invoke();
        }
    }

    private PoolTileTypeEvent CreateData(string key)
    {
        KeyExampleTipeTile type = new KeyExampleTipeTile(key);
        
        List<MarkTilePrefab> listTiles = _typePrefabs.GetTiles(type);
        var prefabTile = _logicSelectPrefabTile.GetTile(listTiles);
        var examplePrefab= _fabric.GetExample(prefabTile);
        
        var data = (DKODataSetKeyTipeTile)examplePrefab.GetTileDKO().KeyRun(_keySetTypeTile.GetData());
        data.SetKey(type);

        var dataDKO = (DKODataInfoT<PoolTileTypeEvent>) examplePrefab.GetTileDKO().KeyRun(_keyGetDataTilePool.GetData());
        PoolTileTypeEvent dataPool = dataDKO.Data;
        
        return dataPool;
    }

    /// <summary>
    /// Вернет обьект из пула дизактивированных
    /// (ключ береться сразу с самого обьекта)
    /// (если в пуле будет пусто, то автоматически создасть экземпляр обьекта,
    /// занесет обьеки в пулл активированных обьектов и в итоге вернет созданных экземпляр обьекта)
    /// </summary>
    public MarkTilePrefab GetTileExample(KeyExampleTipeTile type)
    {
        var tile= _examplePoolKey.GetObject(type.GetKey());
        
        var tileDKO = tile.GetTileDKO();
         
        var dataDKO = (DKODataInfoT<MarkTilePrefab>) tileDKO.KeyRun(_keyGetDataMarkTile.GetData());
        MarkTilePrefab data = dataDKO.Data;
          
        return data;
    }
    
    /// <summary>
    /// Вернет обьект в пулл
    /// (ключ береться сразу с самого обьекта)
    /// (если обьект не был в пулле до этого, то добавит его)
    /// </summary>
    public void ReleaseObject(PoolTileTypeEvent tilePrefab)
    {
        _examplePoolKey.ReleaseObject(tilePrefab);
    }

    public void AddPoolElement(PoolTileTypeEvent tilePrefab, bool isActive)
    {
        _examplePoolKey.AddPoolElement(tilePrefab, isActive);
    }

    public List<AbsKeyData<KeyExampleTipeTile, List<MarkTilePrefab>>> GetAllActiveTile()
    {
        var activeTilePool= _examplePoolKey.GetAllActiveElementPool();

        List<AbsKeyData<KeyExampleTipeTile, List<MarkTilePrefab>>> activeTilePoolMark = new List<AbsKeyData<KeyExampleTipeTile, List<MarkTilePrefab>>>();
        foreach (var VARIABLE in activeTilePool)
        {
            KeyExampleTipeTile keyType = new KeyExampleTipeTile(VARIABLE.Key);
            activeTilePoolMark.Add(new AbsKeyData<KeyExampleTipeTile, List<MarkTilePrefab>>(keyType,new List<MarkTilePrefab>()));
            
            foreach (var VARIABLE2 in VARIABLE.Data)
            {
                var tileDKO = VARIABLE2.GetTileDKO();
                    
                var dataDKO = (DKODataInfoT<MarkTilePrefab>) tileDKO.KeyRun(_keyGetDataMarkTile.GetData());
                MarkTilePrefab dataMarkTile = dataDKO.Data;
                
                activeTilePoolMark[activeTilePoolMark.Count - 1].Data.Add(dataMarkTile);
            }
        }

        return activeTilePoolMark;
    }

    public void DisactiveAllTile()
    {
        _examplePoolKey.DisactiveAllElement();
    }
}
