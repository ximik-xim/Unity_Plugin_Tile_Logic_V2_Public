using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyExampleTipeTile 
{
    public KeyExampleTipeTile()
    {
        
    }
    
    public KeyExampleTipeTile(string key)
    {
        _key = key;
    }
    
    [SerializeField]
    private string _key;

    public string GetKey()
    {
        return _key;
    }
}
