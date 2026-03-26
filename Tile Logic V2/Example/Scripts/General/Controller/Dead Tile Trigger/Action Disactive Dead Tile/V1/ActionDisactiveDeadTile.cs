using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пример отключение отработаного таила
/// (в этой реализации таил сам отвечает за свое отключение)
/// (скрипт будет находиться на таиле)
/// </summary>
public class ActionDisactiveDeadTile : DKOTargetAction
{
    [SerializeField] 
    private GameObject _tile;

    private void Awake()
    {
        LocalAwake();
    }
    

    protected override DKODataRund InvokeRun()
    {
        _tile.gameObject.SetActive(false);
        return null;
    }
}
