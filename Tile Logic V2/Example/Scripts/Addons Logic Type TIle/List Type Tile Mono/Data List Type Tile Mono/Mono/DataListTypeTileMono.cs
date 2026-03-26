using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monobeh которое содержит в себе список всех таилов по типу
/// </summary>
public class DataListTypeTileMono : MonoBehaviour
{
    public event Action OnInit;
    public bool IsInit => _isInit;
    private bool _isInit = false;
    
    [SerializeField]
    private List<AbsKeyData<GetDataSO_KeyExampleTipeTile, List<MarkTilePrefab> >> _list;

    private Dictionary<string, List<MarkTilePrefab>> _data = new Dictionary<string, List<MarkTilePrefab>>();
        
    private void Awake()
    {
        if (_isInit == false)
        {
            _data.Clear();

             foreach (var VARIABLE in _list)
             {
                 _data.Add(VARIABLE.Key.GetData().GetKey(), VARIABLE.Data);
             }
            
            _isInit = true;
            OnInit?.Invoke();
        }
        
    }
        
    public List<MarkTilePrefab> GetTiles(KeyExampleTipeTile key)
    {
        return _data[key.GetKey()];
    }

    public List<KeyExampleTipeTile> GetAllType()
    {
        List<KeyExampleTipeTile> data = new List<KeyExampleTipeTile>();
        foreach (var VARIABLE in _list)
        {
            data.Add(VARIABLE.Key.GetData());
        }

        return data;
    }
    
}
