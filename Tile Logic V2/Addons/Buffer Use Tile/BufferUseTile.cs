using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Тут тупо список используемых прямо сейчас таилов с индексацией
/// В качестве индексации используеться список, где 0-элемент это самый первый таил,
/// а последний элемент - это последний таил
/// </summary>
public class BufferUseTile : MonoBehaviour
{
    public bool IsInit => _isInit;
    private bool _isInit = false;
    
    public event Action OnInit;
    
    [SerializeField]
    private List<DKOKeyAndTargetAction> _tile;

    public event Action OnCompletedAddTile
    {
        add
        {
            _actionAddTile.OnCompleted += value;
        }
        remove
        {
            _actionAddTile.OnCompleted -= value;
        }
    }
    public bool IsCompletedAddTile => _actionAddTile.IsCompleted;
    //Учесть, что тут не получиться буффер, где бы ожидал пока action закончит выполнение
    [SerializeField]
    private LogicListTaskDKO _actionAddTile;
    
    public event Action OnCompletedRemoveTile
    {
        add
        {
            _actionRemoveTile.OnCompleted += value;
        }
        remove
        {
            _actionRemoveTile.OnCompleted -= value;
        }
    }
    public bool IsCompletedRemoveTile => _actionRemoveTile.IsCompleted;
    //Учесть, что тут не получиться буффер, где бы ожидал пока action закончит выполнение
    [SerializeField]
    private LogicListTaskDKO _actionRemoveTile;

    public event Action<DKOKeyAndTargetAction> OnAddTile;
    public event Action<DKOKeyAndTargetAction> OnRemoveTile;
    
    private void Awake()
    {
        if (_actionAddTile.IsInit == false)
        {
            _actionAddTile.OnInit += OnInitActionAddTile;
        }
        
        if (_actionRemoveTile.IsInit == false)
        {
            _actionRemoveTile.OnInit += OnInitActionRemoveTile;
        }
        
        InitCheck();
    }

    private void OnInitActionAddTile()
    {
        _actionAddTile.OnInit -= OnInitActionAddTile;
        InitCheck();
    }

    private void OnInitActionRemoveTile()
    {
        _actionRemoveTile.OnInit -= OnInitActionRemoveTile;
        InitCheck();
    }
    

    private void InitCheck()
    {
        if (_isInit == false) 
        {
            if (_actionAddTile.IsInit == true && _actionRemoveTile.IsInit == true)
            {
                _isInit = true;
                OnInit?.Invoke();
            }
        }
       
    }

    public void AddTile(DKOKeyAndTargetAction tile)
    {
        _tile.Add(tile);
        OnAddTile?.Invoke(tile);
        _actionAddTile.StartAction(tile);
    }

    public void RemoveTile(DKOKeyAndTargetAction tile)
    {
        for (int i = 0; i < _tile.Count; i++)
        {
            if (_tile[i] == tile)
            {
                _tile.RemoveAt(i);
                OnRemoveTile?.Invoke(tile);
                _actionRemoveTile.StartAction(tile);
                break;
            }
        }
    }

    public int GetCountTile()
    {
        return _tile.Count;
    }

    public DKOKeyAndTargetAction GetTile(int idTile)
    {
        return _tile[idTile];
    }
}
