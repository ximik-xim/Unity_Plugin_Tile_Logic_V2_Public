using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Обертка через DKO для инкопсуляции действий
/// (по идеи установить значение типа (ключа) сможет, тот кто имеет ключ для получени класса)
/// </summary>
public class DKOSetKeyTipeTile : DKOTargetAction
{
    [SerializeField] 
    private KeyDataExampleTipeTileMono _data;

    private DKODataSetKeyTipeTile _dataGet;

    private void Awake()
    {
        _dataGet = new DKODataSetKeyTipeTile(_data);
        LocalAwake();
    }

    protected override DKODataRund InvokeRun()
    {
        if (_dataGet == null)
        {
            _dataGet = new DKODataSetKeyTipeTile(_data);
        }

        return _dataGet;
    }

}


public class DKODataSetKeyTipeTile : DKODataRund
{
    public DKODataSetKeyTipeTile(KeyDataExampleTipeTileMono data)
    {
        _data = data;
    }
    
    private KeyDataExampleTipeTileMono _data;

    public void SetKey(KeyExampleTipeTile key)
    {
        _data.SetKey(key);
    }
}