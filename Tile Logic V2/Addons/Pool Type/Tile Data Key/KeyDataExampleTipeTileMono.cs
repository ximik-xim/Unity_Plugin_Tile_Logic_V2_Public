using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Будет хранит в себе тип таила
/// (находиться на самом таиле)
/// </summary>
public class KeyDataExampleTipeTileMono : MonoBehaviour
{
    [SerializeField]
    private KeyExampleTipeTile _key;
    
    public void SetKey(KeyExampleTipeTile key)
    {
        _key = key;
    }
    
    public KeyExampleTipeTile GetKey()
    {
        return _key;
    }
}
