using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Варианты действий в зависимости от типа таиле
/// (некоторые будут спавниться через пул и работать с ним, а не которые просто спавниться и отключаться) 
/// </summary>
public class ListAbsGetExampleTileTypeTile : MonoBehaviour
{
    public event Action OnInit;
    public bool IsInit => _isInit;
    private bool _isInit = false;
    
    [SerializeField] 
    private AbsGetExampleTileType _default;
    
    [SerializeField]
    private List<AbsKeyData<GetDataSO_KeyExampleTipeTile, AbsGetExampleTileType>> _exception;

    private Dictionary<KeyExampleTipeTile, AbsGetExampleTileType> _exceptionData = new Dictionary<KeyExampleTipeTile, AbsGetExampleTileType>();

    private void Awake()
    {
        foreach (var VARIABLE in _exception)
        {
            _exceptionData.Add(VARIABLE.Key.GetData(),VARIABLE.Data);
        }
        
        _isInit = true;
        OnInit?.Invoke();  
    }
    
    
    public MarkTilePrefab GetNextTile(KeyExampleTipeTile typeNextTile)
    {
        //Если по какой то причине не будет получаться получать черезе DKO тип у таилов,
        //то значит нужно будет сделать отдельный скрипт в который засовывать тип таила который
        //был затригерен последним  

        if (_exceptionData.ContainsKey(typeNextTile) == true)
        {
            return _exceptionData[typeNextTile].GetTileExample(typeNextTile);
        }

        return _default.GetTileExample(typeNextTile);
    }
}
