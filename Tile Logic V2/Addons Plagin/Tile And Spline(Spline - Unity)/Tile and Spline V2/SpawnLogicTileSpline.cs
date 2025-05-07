using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnLogicTileSpline : MonoBehaviour
{
   public event Action OnInit;
   public bool IsInit => _isInit;
   private bool _isInit = false;
   
   [SerializeField] 
   private GameObject _targetPosEndSpawn;

   [SerializeField] 
   private GameObject _startPos;


   /// <summary>
   /// Потом через абстракцию сдлеаю
   /// </summary>
   [SerializeField] 
   private AbsGetMarkTilePrefab _getPrefab;

   [SerializeField] 
   private GameObject _parent;

   [SerializeField] 
   private GetDataSODataDKODataKey _keyGetDataPositionConnect;
   
   [SerializeField] 
   private LogicListTaskDKO _startSpawnTileTask;
   
   [SerializeField] 
   private LogicListTaskDKO _recursionSpawnTileTask;

   [SerializeField] 
   private LogicListTaskDKO _endSpawnTileTask;
   
   private float _lastDistance;

   [SerializeField] 
    private bool _startAwake = false;

   private void Awake()
   {
      if (_getPrefab.IsInit == false)
      {
         _getPrefab.OnInit += OnInitData;
      }
      
      if (_startSpawnTileTask.IsInit == false)
      {
         _startSpawnTileTask.OnInit += OnInitListTaskStart;
      }
      
      if (_recursionSpawnTileTask.IsInit == false)
      {
         _recursionSpawnTileTask.OnInit += OnInitListTaskRecursion;
      }
      
      if (_endSpawnTileTask.IsInit == false)
      {
         _endSpawnTileTask.OnInit += OnInitListTaskEnd;
      }

      CheckInit();
   }
   
   private void OnInitData()
   {
      _getPrefab.OnInit -= OnInitData;
      CheckInit();
   }

   private void OnInitListTaskStart()
   {
      _startSpawnTileTask.OnInit -= OnInitListTaskStart;
      CheckInit();
   }

   private void OnInitListTaskRecursion()
   {
      _recursionSpawnTileTask.OnInit += OnInitListTaskRecursion;
      CheckInit();
   }

   private void OnInitListTaskEnd()
   {
      _endSpawnTileTask.OnInit -= OnInitListTaskEnd;
      CheckInit();
   }
   
   private void CheckInit()
   {
      if (_isInit == false)
      {
         if (_getPrefab.IsInit == true && _startSpawnTileTask.IsInit == true && _recursionSpawnTileTask.IsInit == true && _endSpawnTileTask.IsInit == true)
         {
            Init();
         }
      }

   }
   
   private void Init()
   {
      if (_startAwake == true)
      {
         StartLogic();   
      }

      _isInit = true;
      OnInit?.Invoke();
   }

   public void StartLogic()
   {
      var distance = Vector3.Distance(_startPos.transform.position, _targetPosEndSpawn.transform.position);
      var example = _getPrefab.GetTilePrefab();
      
      example.transform.position = _startPos.transform.position + example.GetTilePositionInfo().GetOffsetTile();
      example.transform.parent = _parent.transform;
      
      _lastDistance = distance;
      _startSpawnTileTask.StartAction(example.GetTileDKO());
      RecursionSpawn(example);
   }
   
   private void RecursionSpawn(MarkTilePrefab lastSpawn)
   {

      var dataPositionConnect = (DKODataGetInfoPositionConnectNextTile)lastSpawn.GetTileDKO().KeyRun(_keyGetDataPositionConnect.GetData());
      var positionConnect = dataPositionConnect.PositionConnectNextTileInfo.GetPositionConnectNextTile();
      
     var distance= Vector3.Distance(positionConnect, _targetPosEndSpawn.transform.position);

     _recursionSpawnTileTask.StartAction(lastSpawn.GetTileDKO());

     if (_lastDistance > distance)
     {
        var example = _getPrefab.GetTilePrefab();
        example.transform.position = positionConnect + example.GetTilePositionInfo().GetOffsetTile();
        example.transform.parent = _parent.transform;
            
        _lastDistance = distance;
            
                     
        RecursionSpawn(example);
     }
     else if(_lastDistance == distance)
     {
        Debug.LogError("Ошибка, не приблежаемся к заданной точке");
        return;
     }
     else if(_lastDistance < distance)
     {
        _endSpawnTileTask.StartAction(lastSpawn.GetTileDKO());
        Debug.Log("Закончили спавнить");
     }
          
   }
   
   
}
