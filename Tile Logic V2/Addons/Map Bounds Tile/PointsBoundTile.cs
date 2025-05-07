using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Нужен для определения границ таила
/// (точками устанавливаю границы таила)
/// (точки можно выстовлять в произвольном порядке, не обращая внимания на их координаты x,y,z)
/// </summary>
public class PointsBoundTile : DKOTargetAction
{
   [SerializeField] private List<GameObject> _point = new List<GameObject>();

   private void Awake()
   {
      LocalAwake();
   }

   protected override DKODataRund InvokeRun()
   {
      List<Transform> list = new List<Transform>();
      foreach (var VARIABLE in _point)
      {
         list.Add(VARIABLE.transform);
      }

      PointsBoundTileData data = new PointsBoundTileData();
      data.PointPosition = list;

      return data;
   }

#if UNITY_EDITOR

   [SerializeField] 
   private bool _isActiveGizmoPoint = true;
   [SerializeField] 
   private Color _colorGizmoPoint = Color.yellow;
   [SerializeField] 
   private bool _isActiveGizmoBounds = true;
   [SerializeField] 
   private Color _colorGizmoBounds = Color.cyan;

   private Vector2 _positionX;
   private Vector2 _positionY;
   private Vector2 _positionZ;

   private void OnDrawGizmos()
   {
      if (_point.Count > 0)
      {
         if (_isActiveGizmoPoint == true)
         {
            Gizmos.color = _colorGizmoPoint;
            foreach (var VARIABLE in _point)
            {
               Gizmos.DrawCube(VARIABLE.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
            }
         }


         if (_isActiveGizmoBounds == true)
         {
            CalculatingPositionX();
            CalculatingPositionY();
            CalculatingPositionZ();


            float _sizeX = _positionX.x - _positionX.y;
            float _centerPosX = _positionX.x - _sizeX / 2;

            float _sizeY = _positionY.x - _positionY.y;
            float _centerPosY = _positionY.x - _sizeY / 2;

            float _sizeZ = _positionZ.x - _positionZ.y;
            float _centerPosZ = _positionZ.x - _sizeZ / 2;

            Gizmos.color = _colorGizmoBounds;
            Gizmos.DrawCube(new Vector3(_centerPosX, _centerPosY, _centerPosZ), new Vector3(_sizeX, _sizeY, _sizeZ));
         }
      }
   }

   void CalculatingPositionX()
   {
      float _positionLeft = _point[0].transform.position.x;

      //нахожу самую левую точку
      for (int i = 1; i < _point.Count; i++)
      {
         if (_point[i].transform.position.x < _positionLeft)
         {
            _positionLeft = _point[i].transform.position.x;
         }
      }

      float _positionRight = _positionLeft;

      //нахожу самую правую точку
      for (int i = 0; i < _point.Count; i++)
      {
         if (_point[i].transform.position.x > _positionRight)
         {
            _positionRight = _point[i].transform.position.x;
         }
      }

      _positionX = new Vector2(_positionLeft, _positionRight);
   }

   void CalculatingPositionY()
   {
      float _positionDown = _point[0].transform.position.y;

      //нахожу самую левую точку
      for (int i = 1; i < _point.Count; i++)
      {
         if (_point[i].transform.position.y < _positionDown)
         {
            _positionDown = _point[i].transform.position.y;
         }
      }

      float _positionUp = _positionDown;

      //нахожу самую правую точку
      for (int i = 0; i < _point.Count; i++)
      {
         if (_point[i].transform.position.y > _positionUp)
         {
            _positionUp = _point[i].transform.position.y;
         }
      }

      _positionY = new Vector2(_positionDown, _positionUp);
   }

   void CalculatingPositionZ()
   {
      float _positionBack = _point[0].transform.position.z;

      //нахожу самую левую точку
      for (int i = 1; i < _point.Count; i++)
      {
         if (_point[i].transform.position.z < _positionBack)
         {
            _positionBack = _point[i].transform.position.z;
         }
      }

      float _positionFront = _positionBack;

      //нахожу самую правую точку
      for (int i = 0; i < _point.Count; i++)
      {
         if (_point[i].transform.position.z > _positionFront)
         {
            _positionFront = _point[i].transform.position.z;
         }
      }

      _positionZ = new Vector2(_positionBack, _positionFront);
   }

#endif

}

public class PointsBoundTileData : DKODataRund
{
   public List<Transform> PointPosition;
}
