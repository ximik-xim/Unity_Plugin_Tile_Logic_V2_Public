using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Логика пула для таилов
/// В данной реализации, все немного замудренно, с целью в дальнейшем полностью отделить логику пула
/// Тут из префаба таила, через его DKO буду получать уже целевой скрипт
/// (и обратно, через то же DKO, буду получать скрипт который являеться префабом таила)
/// !!!ВНИМАНИЕ В ЭТОЙ РЕАЛИЗАЦИИ, В СЛУЧАЕ ИСПОЛЬЗОВАНИЯ СТАРТОВЫХ ТАИЛОВ, НЕОБХОДИМО УБЕДИТЬСЯ, ЧТО ИХ DKO МАКСИМАЛЬНО БЫСТРО ПРОИНИЦИАЛИЗАРУЕТСЯ ПОСЛЕ ЗАПУСКА ПРОЕКАТ!!!
/// (нужно это т.к инициализация скрипта происходит после инициализации стартовых таилов)
/// </summary>
public class PoolTileDefault : MonoBehaviour
{
    public event Action OnInit;
    public bool IsInit => _isInit;
    private bool _isInit = false;

    /// <summary>
    /// Список стартовых префабов
    /// (нужен, т.к стартовые префабы не спавняться через эту логику)
    /// </summary>
    [SerializeField] 
    private List<MarkTilePrefab> _listStartTilePrefab;
    
    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetDataMarkTile;
    
    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetDataTilePool;
    
    [SerializeField]
    private CustomPool<PoolTileEvent> _examplePoolKey;
    
    [SerializeField] 
    private MarkTilePrefab _tilePrefab;

    private bool _isStart = false;

    private void Awake()
    {
#if UNITY_EDITOR
        bool lastStatusInspector = false;
        if (_examplePoolKey != null)
        {
            lastStatusInspector = _examplePoolKey.UseListInspector;
        }
#endif
        
        _examplePoolKey = new CustomPool<PoolTileEvent>(CreateData, null, null);

#if UNITY_EDITOR
        _examplePoolKey.UseListInspector = lastStatusInspector;
#endif
        
        StartAction();
        
        // _isInit = true;
        // OnInit?.Invoke(); 

    }


    private void StartAction()
    {
        _isStart = true;
        
        foreach (var VARIABLE in _listStartTilePrefab)
        {
            if (VARIABLE.GetTileDKO().IsInit == false)
            {
                VARIABLE.GetTileDKO().OnInit += CheckCompleted;
            }
        }

        _isStart = false;

        CheckCompleted();
    }


    private void CheckCompleted()
    {
        if (_isStart == false)
        {
            //Отписываюсь ото всех кто закончил выполнение операции
            for (int i = 0; i < _listStartTilePrefab.Count; i++)
            {
                if (_listStartTilePrefab[i].GetTileDKO().IsInit == true)
                {
                    _listStartTilePrefab[i].GetTileDKO().OnInit -= CheckCompleted;
                
                }
            }
        
            //Еще раз прохожусь и ищю есть ли те кого еще ожидаем пока выполниться операция
            for (int i = 0; i < _listStartTilePrefab.Count; i++)
            {
                if (_listStartTilePrefab[i].GetTileDKO().IsInit == false)
                {
                    return;
                }
            }
        
            //Если нет тех, кто не закончил свою операцию, значит все конец(хэпи енд)
            Completed();
        }
    }
    

    private void Completed()
    {
        if (_isInit == false)
        {
            foreach (var VARIABLE in _listStartTilePrefab)
            {
                var dataDKO = (DKODataInfoT<PoolTileEvent>) VARIABLE.GetTileDKO().KeyRun(_keyGetDataTilePool.GetData());
                PoolTileEvent data = dataDKO.Data;
            
                _examplePoolKey.AddPoolElement(data,true);
            }
            
            _isInit = true;
            OnInit?.Invoke();  
        }
    }
    


    private PoolTileEvent CreateData()
    {
        var tile =  Instantiate(_tilePrefab);
        var tileDKO = tile.GetTileDKO();
        
        var dataDKO = (DKODataInfoT<PoolTileEvent>) tileDKO.KeyRun(_keyGetDataTilePool.GetData());
        PoolTileEvent data = dataDKO.Data;

        return data;
    }

    /// <summary>
    /// Вернет обьект из пула дизактивированных
    /// (если в пуле будет пусто, то автоматически создасть экземпляр обьекта,
    /// занесет обьеки в пулл активированных обьектов и в итоге вернет созданных экземпляр обьекта)
    /// </summary>
    public MarkTilePrefab GetTilePrefab()
    {
         var tile= _examplePoolKey.GetObject();
         var tileDKO = tile.GetTileDKO();
         
          var dataDKO = (DKODataInfoT<MarkTilePrefab>) tileDKO.KeyRun(_keyGetDataMarkTile.GetData());
          MarkTilePrefab data = dataDKO.Data;
          
         return data;
    }
    
    /// <summary>
    /// Вернет обьект в пулл
    /// (если обьект не был в пулле до этого, то добавит его)
    /// </summary>
    public void ReleaseObject(PoolTileEvent tilePrefab)
    {
        _examplePoolKey.ReleaseObject(tilePrefab);
    }

    public void AddPoolElement(PoolTileEvent tilePrefab, bool isActive)
    {
        _examplePoolKey.AddPoolElement(tilePrefab, isActive);
    }
}
