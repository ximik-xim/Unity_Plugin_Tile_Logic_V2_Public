using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLogicListTaskTilePositionInfo : MonoBehaviour
{
    public bool IsInit => _isInit;
    private bool _isInit = false;
    
    public event Action OnInit;
    
    /// <summary>
    /// Тут определяю проекцию поведения
    /// (по очереди будут выполняться задачи или как получиться)
    /// </summary>
    [SerializeField]
    private TL_AbsTaskReplicationWrapperMono _replicationTask;

    /// <summary>
    /// тут определяем список каких то задач(может быть любой класс)
    /// (любых, и в любом виде, главное что бы можно было узнать, выполнена ли она или нет, и вызвать выполнение)
    /// </summary>
    [SerializeField]
    private List<AbsTileLogicAbsTaskTilePositionInfo> _listTask;
    
    public event Action OnCompleted
    {
        add
        {
            _replicationTask.OnCompleted += value;
        }
        remove
        {
            _replicationTask.OnCompleted -= value;
        }
    }
    public bool IsCompleted => _replicationTask.IsCompleted;

    /// <summary>
    /// Ссылка на DKO таила
    /// (сохраняеться до конца действий)
    /// </summary>
    private SetPositionTileData _bufferTile;

    private void Awake()
    {
        _replicationTask.OnCheckCompleted += OnCheckCompleted;
        _replicationTask.OnStartAction += OnStartAction;
        _replicationTask.OnCompleted += OnCompletedLogic;
        _replicationTask.OnCompletedElement += OnElementCompleted;

        StartInit();
    }
    
    /// <summary>
    /// Логика проверки готовности задач при включении скрипта
    /// </summary>
    private void StartInit()
    {
        List<AbsTileLogicAbsTaskTilePositionInfo> _buffer = new List<AbsTileLogicAbsTaskTilePositionInfo>();
        bool _isStart = false;
        
        StartLogic();

        void StartLogic()
        {
            if (_isInit == false)
            {
                _isStart = true;

                foreach (var VARIABLE in _listTask)
                {
                    //нужно именно 2 проверки
                    //1 - что бы не вызвать Init просто так(обьект может сам инициализироваться отдельно)
                    //2 - что бы проверить инициализировался ли обьект или его нужно ждать
                    if (VARIABLE.IsInit == false)
                    {
                        if (VARIABLE.IsInit == false)
                        {
                            _buffer.Add(VARIABLE);
                            VARIABLE.OnInit += CheckInitCompleted;
                        }
                    }
                }

                _isStart = false;

                CheckInitCompleted();
            }
        }

        void CheckInitCompleted()
        {
            if (_isStart == false) 
            {
                int targetCount = _buffer.Count;
                for (int i = 0; i < targetCount; i++)
                {
                    if (_buffer[i].IsInit == true)
                    {
                        _buffer[i].OnInit -= CheckInitCompleted;
                        _buffer.RemoveAt(i);
                        i--;
                        targetCount--;
                    }
                }

                if (_buffer.Count == 0)
                {
                    InitCompleted();
                }
            }
        }
    }

    private void InitCompleted()
    {
        _isInit = true;
        OnInit?.Invoke();
    }
    
    public void StartAction(SetPositionTileData tileInfo)
    {
        //Только если все операции были выполнены, тогда разрешаю запуск
        if (IsCompleted == true)
        {
            _bufferTile = tileInfo;
            
            //тут ищу задачи которые еще не закончили выполнение, и сохраняю их Id из списка 
            List<int> id = new List<int>();
 
            for (int i = 0; i < _listTask.Count; i++)
            {
                id.Add(i);
               
                _listTask[i].OnCompletedLogic += OnCompletedElement;
            }
        
        
            //Запускаю выполнение задачи
            _replicationTask.StartAction(id);
        }
    }

        
    private void OnElementCompleted(int id)
    {
        _listTask[id].OnCompletedLogic -= OnCompletedElement;
    }
   
    /// <summary>
    ///Когда задача выполниться, она сообщит об этом в логику по выполнению задач
    /// </summary>
    private void OnCompletedElement()
    {
        _replicationTask.ActionCompleted();
    }
    
    /// <summary>
    /// Тут, логика выполнение задач определяет какой будет вызван следующий элемент
    /// </summary>
    private void OnStartAction(int id)
    {
        _listTask[id].StartLogic(_bufferTile);
    }

    /// <summary>
    /// Тут логика задач проверяет какая из задач уже выполнена, а какие еще нет
    /// </summary>
    private bool OnCheckCompleted(int id)
    {
        return _listTask[id].IsCompletedLogic;
    }
    
    /// <summary>
    /// сработает когда все задачи будут выполнены в логике задач
    /// </summary>
    private void OnCompletedLogic()
    {

    }

    private void OnDestroy()
    {
        _replicationTask.OnCheckCompleted -= OnCheckCompleted;
        _replicationTask.OnStartAction -= OnStartAction;
        _replicationTask.OnCompleted -= OnCompletedLogic;
        _replicationTask.OnCompletedElement -= OnElementCompleted;

    }
    
}
