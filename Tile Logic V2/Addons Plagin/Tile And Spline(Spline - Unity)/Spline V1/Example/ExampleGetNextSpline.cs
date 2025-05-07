using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;


/// <summary>
/// крч работать это должно так.
/// Есть общий контейнер.
/// Когда игрок (обьект) будет касаться таила, то в общее хранилеще со всеми линиями будет добавляться линия
/// и добавляться она будет в конец списка(а т.к я иду по порядку, то и изменять _id возращаемого сплаина НЕ НУЖНО)
/// 
/// А вот когда буду удалять таил который уже отработал, то тут индекс нужно будет поменять, т.к по логике он будет выше в списке текущего выбранного id
/// (но для универсальности, эту функцию сделаю отключаймой)
/// </summary>
public class ExampleGetNextSpline : AbsGetNextSpline
{
    public override bool IsInit => _isInit;
    private bool _isInit = false;
    public override event Action OnInit;
    
    [SerializeField]
    private SplineContainer _splineContainer;

    //начинаю с -1, что бы при первом запросе вернуть первый(0) элемент в списке
    private int _idSpline = -1;
    
    [SerializeField] 
    private bool _subtractIdRemoveElemt = true;
    
    private void Awake()
    {
        if (_subtractIdRemoveElemt == true)
        {
            SplineContainer.SplineRemoved += SplineRemoved;    
        }
        
        
        _isInit = true;
        OnInit?.Invoke();
    }

    private void SplineRemoved(SplineContainer splineContainer, int arg2)
    {
        if (splineContainer == _splineContainer)
        {
            _idSpline--;
        }
    }




    public override Spline GetNextSpline()
    {
        _idSpline++;
        //Если дойдем до конца списка сплеинов, то начнем заного путь
        if (_splineContainer.Splines.Count - 1 < _idSpline)
        {
            _idSpline = 0;
        }

        if (_idSpline < 0) 
        {
            _idSpline = 0;
        }
        
        return _splineContainer.Splines[_idSpline];
    }

    private void OnDestroy()
    {
        SplineContainer.SplineRemoved -= SplineRemoved;
    }
}
