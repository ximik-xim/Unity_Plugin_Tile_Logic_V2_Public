using System;
using UnityEngine;

/// <summary>
/// Маркер коллайдеров которые будут ограничивать угол поворота 
/// </summary>
public class MarkerLimitRotation : MonoBehaviour
{
   /// <summary>
   /// Отразит направление диапазона  
   /// </summary>
   [SerializeField]
   private bool _backForward;

   public bool BackForward => _backForward;
   
   private void OnDrawGizmos()
   {
      //Берем направление, куда смотрит объект
      Vector3 forward = transform.forward;
      Quaternion leftRotation;
      
      if (_backForward == true) 
      {
         //Создаем поворот на -90 градусов по оси Y
         leftRotation = Quaternion.Euler(0, -90, 0);
      }
      else
      {
         //Создаем поворот на 90 градусов по оси Y
         leftRotation = Quaternion.Euler(0, 90, 0);
      }
     

      //Умножаем поворот на вектор, чтобы получить новое направление
      Vector3 leftDirection = leftRotation * forward;

      //Рисуем линию направления
      Gizmos.color = new Color(1f, 0.5f, 0f);
      Gizmos.DrawRay(transform.position, leftDirection * 10f);
   }
}
