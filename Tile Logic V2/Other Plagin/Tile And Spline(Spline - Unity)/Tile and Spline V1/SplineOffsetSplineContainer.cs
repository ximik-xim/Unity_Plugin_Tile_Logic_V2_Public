using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineOffsetSplineContainer : MonoBehaviour
{

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


    public void AddSpline(SplineContainer SetContainer, SplineContainer GetContainer)
    {
        foreach (var VARIABLE in GetContainer.Splines)
        {
            var spline = VARIABLE;
            
            //если нужно с учетом смещения, то прийдеться создовать новый Spline с новыми координатами(которые учитывают это смещение)
            if (_globalOffset == true)
            {
                var offset = GetContainer.transform.position - SetContainer.transform.position;
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

                if (_buffer.ContainsKey(spline) == false) 
                {
                    SetContainer.AddSpline(dataSpline);
                
                    _buffer.Add(spline, dataSpline);
                }
                else
                {
                    Debug.Log("ВНИМАНИЕ ПОТЕНЦИАЛЬНАЯ ОШИБКА, СПЛАИН УЖЕ ЕСТЬ В СПИСКЕ");
                }
                
   
       
            }
            else
            {
                //если смещение не нужно учитывать, то тупо сразу ссылку передаем
                
                SetContainer.AddSpline(spline);
            }
            
        }
    }
    
    public void RemoveSpline(SplineContainer SetContainer,SplineContainer GetContainer)
    {
        if (_globalOffset == true)
        {
            foreach (var VARIABLE in GetContainer.Splines)
            {
                if (_buffer.ContainsKey(VARIABLE) == true)
                {
                    SetContainer.RemoveSpline(_buffer[VARIABLE]);
                    _buffer.Remove(VARIABLE);
                }
            }
            
        }
        else
        {
            foreach (var VARIABLE in GetContainer.Splines)
            {
                SetContainer.RemoveSpline(VARIABLE);
            }
            
        }
        
    }
}
