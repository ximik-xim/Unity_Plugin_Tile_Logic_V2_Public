using System;
using UnityEngine;

public class StartLogicSpawn : MonoBehaviour
{
    [SerializeField] 
    private SpawnLogicTileSpline _spawnLogic;
    
    
    private void Awake()
    {
        if (_spawnLogic.IsInit == false)
        {
            _spawnLogic.OnInit += OnInit;
            return;
        }

        Init();
    }

    private void OnInit()
    {
        _spawnLogic.OnInit -= OnInit;
        Init();
    }
    
    private void Init()
    {
        _spawnLogic.StartLogic();
    }
}
