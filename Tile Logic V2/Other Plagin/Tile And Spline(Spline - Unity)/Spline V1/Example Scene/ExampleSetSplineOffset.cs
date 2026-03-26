using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Добавляет в указанный общ. хран Spline, все Spline из другого хран.
/// При этом можно как учитывать смещение между хранилещами, так и не учитывать
/// </summary>
public class ExampleSetSplineOffset : MonoBehaviour
{
    [SerializeField]
    private SplineContainer _splineContainerSet;

    [SerializeField]
    private SplineContainer _splineContainerGet;
    
    /// <summary>
    /// Учитывать ли глобальное смещение
    /// (разницу между координатами позиции контейнера с которого беру данные и контейнера в который ложу данные)
    /// Это нужно, т.к позиции ломанной линии(Spline) отчитываються оносительно GameObject на котором находиться общеее хран. с ломанными линиями(SplineContainer),
    /// и координаты там локальные
    /// </summary>
    [SerializeField] 
    private bool _globalOffset = true;

    /// <summary>
    /// нужен, т.к если создам новый Spline с измененными координатами, то при вызове удаления, нужна будет ссылка на сплаин который создал
    /// </summary>
    private Dictionary<Spline, Spline> _buffer = new Dictionary<Spline, Spline>(); 
    
#if UNITY_EDITOR
    [SerializeField]
    private bool _add = false;
    
    [SerializeField]
    private bool _remove = false;
#endif
    
    private void SetData()
    {
        foreach (var VARIABLE in _splineContainerGet.Splines)
        {
            var spline = VARIABLE;
            
            //если нужно с учетом смещения, то прийдеться создовать новый Spline с новыми координатами(которые учитывают это смещение)
            if (_globalOffset == true)
            {
                var offset = _splineContainerGet.transform.position - _splineContainerSet.transform.position;
                var data = spline.Knots;

                List<BezierKnot> knotsData = new List<BezierKnot>();
                foreach (var VARIABLE2 in data)
                {
                    BezierKnot knot = new BezierKnot(VARIABLE2.Position, VARIABLE2.TangentIn, VARIABLE2.TangentOut, VARIABLE2.Rotation);
            
                    knot.Position.x += offset.x;
                    knot.Position.y += offset.y;
                    knot.Position.z += offset.z;
            
                    knotsData.Add(knot);
            
                }

            
                
                Spline dataSpline = new Spline(knotsData);
                _splineContainerSet.AddSpline(dataSpline);

                _buffer.Add(VARIABLE, dataSpline);
            }
            else
            {
                //если смещение не нужно учитывать, то тупо сразу ссылку передаем
                _splineContainerSet.AddSpline(spline);
            }
        }

        
    }
    
    private void RemoveData()
    {
        if (_globalOffset == true)
        {
            foreach (var VARIABLE in _splineContainerGet.Splines)
            {
                if (_buffer.ContainsKey(VARIABLE) == true)
                {
                    _splineContainerSet.RemoveSpline(_buffer[VARIABLE]);
                    _buffer.Remove(VARIABLE);
                }
            }
            
        }
        else
        {
            foreach (var VARIABLE in _splineContainerGet.Splines)
            {
                _splineContainerSet.RemoveSpline(VARIABLE);
            }
            
        }
        
        
        
        
    }

    private void OnValidate()
    {
        #if UNITY_EDITOR
            
        if (_add == true)
        {
            SetData();
        }
        
        if (_remove == true)
        { 
            RemoveData();
        }
        
        #endif
    }
}
