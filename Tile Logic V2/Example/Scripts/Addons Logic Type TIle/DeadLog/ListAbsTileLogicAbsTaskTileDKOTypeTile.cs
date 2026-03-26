using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Варианты действий в зависимости от типа таиле
/// (пока исп. только для логики смерти таила)
/// </summary>
public class ListAbsTileLogicAbsTaskTileDKOTypeTile : MonoBehaviour
{
    public event Action OnInit;
    public bool IsInit => _isInit;
    private bool _isInit = false;
    
    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetTypeTile;
    
    [SerializeField] 
    private TL_AbsTaskLogicDKO _default;

    [SerializeField]
    private List<AbsKeyData<GetDataSO_KeyExampleTipeTile, TL_AbsTaskLogicDKO>> _exception;

    private Dictionary<KeyExampleTipeTile, TL_AbsTaskLogicDKO> _exceptionData = new Dictionary<KeyExampleTipeTile, TL_AbsTaskLogicDKO>();

    private void Awake()
    {
        foreach (var VARIABLE in _exception)
        {
            _exceptionData.Add(VARIABLE.Key.GetData(),VARIABLE.Data);
        }
        
        _isInit = true;
        OnInit?.Invoke();  
    }
    
    
    public void TriggerDeadTile(DKOKeyAndTargetAction tileDKO)
    {
        //Если по какой то причине не будет получаться получать черезе DKO тип у таилов,
        //то значит нужно будет сделать отдельный скрипт в который засовывать тип таила который
        //был затригерен последним  
        var data = (DKODataGetKeyTipeTile)tileDKO.KeyRun(_keyGetTypeTile.GetData());

        var typeNextTile = data.GetKey();
        
        if (_exceptionData.ContainsKey(typeNextTile) == true)
        {
             _exceptionData[typeNextTile].StartLogic(tileDKO);
             return;
        }

         _default.StartLogic(tileDKO);
         return;
    }
}
