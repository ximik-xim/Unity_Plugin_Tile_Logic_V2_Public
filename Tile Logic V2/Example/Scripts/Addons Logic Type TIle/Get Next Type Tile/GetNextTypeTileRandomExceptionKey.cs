using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GetNextTypeTileRandomExceptionKey : AbsGetNextTypeTile
{
    [SerializeField] 
    private AbsGetDataListTypeTile _typeTile;

    [SerializeField]
    private List<GetDataSO_KeyExampleTipeTile> _exception;
    
    [SerializeField] 
    private bool _saveExceptionAwake = true;

    private List<KeyExampleTipeTile> _listType;
    private void Awake()
    {
        if (_saveExceptionAwake == true)
        {
            _listType = GetListType();
        }
    }

    public override KeyExampleTipeTile GetTypeTile(KeyExampleTipeTile lastKey)
    {
        if (_saveExceptionAwake == true) 
        {
            var typeId= Random.Range(0, _listType.Count);
            return _listType[typeId];
        }
        else
        {
            List<KeyExampleTipeTile> listType = GetListType();
            var typeId= Random.Range(0, _listType.Count);
            return listType[typeId];
        }
    }

    private List<KeyExampleTipeTile> GetListType()
    {
        List<KeyExampleTipeTile> data = new List<KeyExampleTipeTile>();
        foreach (var VARIABLE in  _typeTile.GetAllType())
        {
            foreach (var VARIABLE2 in _exception)
            {
                if (VARIABLE.GetKey() == VARIABLE2.GetData().GetKey())
                {
                    continue;
                }
                else
                {
                    data.Add(VARIABLE);
                }
                    
            }
        }

        return data;
    }
}
