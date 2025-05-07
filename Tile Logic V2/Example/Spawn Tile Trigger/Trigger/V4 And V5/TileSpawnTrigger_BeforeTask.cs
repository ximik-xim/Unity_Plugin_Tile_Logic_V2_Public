using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Триггер сообщающий о том, что пора спавнить следующий таил.
/// Имеет список задач, выполняющихся до момента оповещения об найденном коллайдере
/// !ВНИМАНИЕ, В ЭТОЙ РЕАЛИЗАЦИИ НЕ ПРЕДУСМОТРЕН МОМЕН, КОГДА TASK НАДО БУДЕТ ЖДАТЬ ДО ЗАВЕРШЕИЯ !!! ВСЕ ДОЛЖНО БЫТЬ СИНХРОННЫМ
/// </summary>
public class TileSpawnTrigger_BeforeTask : AbsTileSpawnTrigger
{
    public override event Action OnInit
    {
        add
        {
            _beforeTask.OnInit += value;
        }
        remove
        {
            _beforeTask.OnInit -= value;
        }
    }
    public override bool IsInit => _beforeTask.IsInit;
    
    public override event Action<DKOKeyAndTargetAction> OnTriggerSpawnTile;
    
    [SerializeField] 
    private bool _triggerEnter;

    [SerializeField] 
    private bool _triggerExit;
    
    [SerializeField] 
    private bool _triggerEnter2D;

    [SerializeField] 
    private bool _triggerExit2D;

    [SerializeField] 
    private LogicListTaskDKO _beforeTask;
    
#if  UNITY_EDITOR
    [SerializeField] 
    private Color _colorGizmo = Color.blue;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _colorGizmo;
        Gizmos.DrawCube(this.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
    }
    
#endif
    

    private void OnTriggerEnter(Collider other)
    {
        if (_triggerEnter == true) 
        {
            GetTileDKO(other.gameObject);    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_triggerExit == true) 
        {
            GetTileDKO(other.gameObject);    
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            if (_triggerEnter2D == true) 
            {
                GetTileDKO(other.gameObject);    
            }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_triggerExit2D == true) 
        {
            GetTileDKO(other.gameObject);    
        }
    }

    private void GetTileDKO(GameObject other)
    {
        AbsTileSpawnMarker marker = other.GetComponent<AbsTileSpawnMarker>();
        
        if (marker != null)
        {
            var tileDKO = marker.GetTileDKO();

            _beforeTask.OnCompleted -= OnCompletedTask;
            _beforeTask.OnCompleted += OnCompletedTask;
            
            _beforeTask.StartAction(tileDKO);
            
            void OnCompletedTask()
            {
                _beforeTask.OnCompleted -= OnCompletedTask;
        
                OnTriggerSpawnTile?.Invoke(tileDKO);
            }
        }
    }

   
}
