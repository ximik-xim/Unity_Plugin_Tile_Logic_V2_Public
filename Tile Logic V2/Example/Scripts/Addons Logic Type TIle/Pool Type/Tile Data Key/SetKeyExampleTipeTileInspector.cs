using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Нужен, что бы в ручную указывать ключ у таила
/// (это нужно для таилов, которые не спавняться через код, а сразу находяться на сцене(стартовые таилы))
/// </summary>
public class SetKeyExampleTipeTileInspector : MonoBehaviour
{
    [SerializeField] 
    private KeyDataExampleTipeTileMono _keyData;

    [SerializeField] 
    private GetDataSO_KeyExampleTipeTile _key;

    private void Awake()
    {
        _keyData.SetKey(_key.GetData());
    }
}
