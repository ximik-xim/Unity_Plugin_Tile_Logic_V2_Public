using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Вычесляет границы карты с талами
/// </summary>
public class BoundsMapTile : MonoBehaviour
{
   [SerializeField] 
   private BufferUseTile _bufferUseTile;

   [SerializeField] 
   private GetDataSODataDKODataKey _keyGetData;
   
   [SerializeField] 
   private List<Transform> _positionPoint;

   private Vector2 _positionX;
   private Vector2 _positionY;
   private Vector2 _positionZ;

   [SerializeField] 
   private GameObject _pointLeft;
   [SerializeField] 
   private GameObject _pointRight;
   
   [SerializeField] 
   private GameObject _pointDown;
   [SerializeField] 
   private GameObject _pointUp;
   
   [SerializeField] 
   private GameObject _pointBack;
   [SerializeField] 
   private GameObject _pointFront;

   public event Action OnUpdateBounds; 
   
   private void Awake()
   {
      if (_bufferUseTile.IsInit == false)
      {
         _bufferUseTile.OnInit += OnInitActionAddTile;
         return;
      }

      InitActionAddTile();
   }

   private void OnInitActionAddTile()
   {
      _bufferUseTile.OnInit -= OnInitActionAddTile;
      InitActionAddTile();
   }
   
   private void InitActionAddTile()
   {
      _bufferUseTile.OnAddTile += OnAddTile;
      _bufferUseTile.OnRemoveTile += OnRemoveTile;

      UpdatePositonBounds();
   }

   private void OnRemoveTile(DKOKeyAndTargetAction tileDKO)
   {
      UpdatePositonBounds();
   }

   private void OnAddTile(DKOKeyAndTargetAction tileDKO)
   {
      UpdatePositonBounds();
   }

   private void UpdatePositonBounds()
   {
      int countId = _bufferUseTile.GetCountTile();
      
      if (countId > 0)
      {
         var pointPositionTileA = (PointsBoundTileData)_bufferUseTile.GetTile(0).KeyRun(_keyGetData.GetData());
         var pointPositionTileB = (PointsBoundTileData)_bufferUseTile.GetTile(countId - 1).KeyRun(_keyGetData.GetData());

         _positionPoint.Clear();

         foreach (var VARIABLE in pointPositionTileA.PointPosition)
         {
            _positionPoint.Add(VARIABLE);
         }
         
         foreach (var VARIABLE in pointPositionTileB.PointPosition)
         {
            _positionPoint.Add(VARIABLE);
         }
      }
      
      CalculatingPositionX();
      CalculatingPositionY();
      CalculatingPositionZ();
      
      float _sizeX = _positionX.x - _positionX.y;
      float _centerPosX = _positionX.x - _sizeX / 2;

      float _sizeY = _positionY.x - _positionY.y;
      float _centerPosY = _positionY.x - _sizeY / 2;
      
      float _sizeZ = _positionZ.x - _positionZ.y;
      float _centerPosZ = _positionZ.x - _sizeZ / 2;

      _pointLeft.transform.position = new Vector3(_positionX.x, _centerPosY, _centerPosZ);
      _pointRight.transform.position = new Vector3(_positionX.y, _centerPosY, _centerPosZ);
      
      _pointDown.transform.position = new Vector3(_centerPosX, _positionY.x, _centerPosZ);
      _pointUp.transform.position = new Vector3(_centerPosX, _positionY.y, _centerPosZ);
      
      _pointBack.transform.position = new Vector3(_centerPosX, _centerPosY, _positionZ.x);
      _pointFront.transform.position = new Vector3(_centerPosX, _centerPosY, _positionZ.y);
      
      OnUpdateBounds?.Invoke();
   }
   
   void CalculatingPositionX()
   {
      if (_positionPoint.Count > 0)
      {
         float _positionLeft = _positionPoint[0].transform.position.x;
      
         //нахожу самую левую точку
         for (int i = 1; i < _positionPoint.Count; i++)
         {
            if (_positionPoint[i].transform.position.x < _positionLeft) 
            {
               _positionLeft = _positionPoint[i].transform.position.x;
            }
         }
      
         float _positionRight = _positionLeft;

         //нахожу самую правую точку
         for (int i = 0; i < _positionPoint.Count; i++)
         {
            if (_positionPoint[i].transform.position.x > _positionRight)
            {
               _positionRight = _positionPoint[i].transform.position.x;
            }
         }

         _positionX = new Vector2(_positionLeft, _positionRight);
      }
   }
   
   void CalculatingPositionY()
   {
      if (_positionPoint.Count > 0)
      {
         float _positionDown = _positionPoint[0].transform.position.y;

         //нахожу самую левую точку
         for (int i = 1; i < _positionPoint.Count; i++)
         {
            if (_positionPoint[i].transform.position.y < _positionDown)
            {
               _positionDown = _positionPoint[i].transform.position.y;
            }
         }

         float _positionUp = _positionDown;

         //нахожу самую правую точку
         for (int i = 0; i < _positionPoint.Count; i++)
         {
            if (_positionPoint[i].transform.position.y > _positionUp)
            {
               _positionUp = _positionPoint[i].transform.position.y;
            }
         }

         _positionY = new Vector2(_positionDown, _positionUp);
      }
   }
   
   void CalculatingPositionZ()
   {
      if (_positionPoint.Count > 0)
      {
         float _positionBack = _positionPoint[0].transform.position.z;

         //нахожу самую левую точку
         for (int i = 1; i < _positionPoint.Count; i++)
         {
            if (_positionPoint[i].transform.position.z < _positionBack)
            {
               _positionBack = _positionPoint[i].transform.position.z;
            }
         }

         float _positionFront = _positionBack;

         //нахожу самую правую точку
         for (int i = 0; i < _positionPoint.Count; i++)
         {
            if (_positionPoint[i].transform.position.z > _positionFront)
            {
               _positionFront = _positionPoint[i].transform.position.z;
            }
         }

         _positionZ = new Vector2(_positionBack, _positionFront);
      }
   }

   /// <summary>
   /// Вернет X координаты леевой и правой границы 
   /// </summary>
   public Vector2 GetPositionBoundsLeftAndRight()
   {
      return new Vector2(_pointLeft.transform.position.x, _pointRight.transform.position.x);
   }
   
   /// <summary>
   /// Вернет Y координаты нижней и верхней границы 
   /// </summary>
   public Vector2 GetPositionBoundsDownAndUp()
   {
      return new Vector2(_pointDown.transform.position.y , _pointUp.transform.position.y);
   }
   
   /// <summary>
   /// Вернет Z координаты передней и задней границы 
   /// </summary>
   public Vector2 GetPositionBoundsBackAndFront()
   {
      return new Vector2(_pointBack.transform.position.z, _pointFront.transform.position.z);
   }

   /// <summary>
   /// Вернет координаты центра левой границы
   /// </summary>
   public Vector3 GetPositionBoundLeft()
   {
      return _pointLeft.transform.position;
   }
   
   /// <summary>
   /// Вернет координаты центра правой границы
   /// </summary>
   public Vector3 GetPositionBoundRight()
   {
      return _pointRight.transform.position;
   }
   
   /// <summary>
   /// Вернет координаты центра нижней границы
   /// </summary>
   public Vector3 GetPositionBoundDown()
   {
      return _pointDown.transform.position;
   }
   
   /// <summary>
   /// Вернет координаты центра верхней границы
   /// </summary>
   public Vector3 GetPositionBoundUp()
   {
      return _pointUp.transform.position;
   }
   
   /// <summary>
   /// Вернет координаты центра задней границы
   /// </summary>
   public Vector3 GetPositionBoundBack()
   {
      return _pointBack.transform.position;
   }
   
   /// <summary>
   /// Вернет координаты центра передней границы
   /// </summary>
   public Vector3 GetPositionBoundFront()
   {
      return _pointFront.transform.position;
   }
#if UNITY_EDITOR

   [SerializeField] 
   private bool _isActiveGizmoPoint = true;
   [SerializeField] 
   private Color _colorGizmoPoint = Color.yellow;
   [SerializeField] 
   private bool _isActiveGizmoBounds = true;
   [SerializeField] 
   private Color _colorGizmoBounds = Color.blue;

   private void OnDrawGizmos()
   {

      if (_isActiveGizmoPoint == true) 
      {
         Gizmos.color = _colorGizmoPoint;
         Gizmos.DrawCube(_pointLeft.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
         Gizmos.DrawCube(_pointRight.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
      
         Gizmos.DrawCube(_pointDown.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
         Gizmos.DrawCube(_pointUp.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
      
         Gizmos.DrawCube(_pointBack.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
         Gizmos.DrawCube(_pointFront.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
      }

      if (_isActiveGizmoBounds == true)
      {
         float _sizeX = _pointLeft.transform.position.x - _pointRight.transform.position.x;
         float _centerPosX = _pointLeft.transform.position.x - _sizeX / 2;

         float _sizeY = _pointDown.transform.position.y - _pointUp.transform.position.y;
         float _centerPosY = _pointDown.transform.position.y - _sizeY / 2;

         float _sizeZ = _pointBack.transform.position.z - _pointFront.transform.position.z;
         float _centerPosZ = _pointBack.transform.position.z - _sizeZ / 2;

         Gizmos.color = _colorGizmoBounds;
         Gizmos.DrawCube(new Vector3(_centerPosX, _centerPosY, _centerPosZ), new Vector3(_sizeX, _sizeY, _sizeZ));
      }
   }
#endif

   

   private void OnDestroy()
   {
      _bufferUseTile.OnAddTile -= OnAddTile;
      _bufferUseTile.OnRemoveTile -= OnRemoveTile;
   }
}
