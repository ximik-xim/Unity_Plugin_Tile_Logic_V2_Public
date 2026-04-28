using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

public class WindowSplineToolGeneral : EditorWindow
{
    private Vector2 _mainScrollPos;
    private SplineContainer _container;
    
    private bool _statusToggleSelectAllSpline;
    private int _selectIdSpline = 0;

    private Vector3 _targetStartPointSpline;
    
    private Vector3 _targetRotSpline;
    private Vector3 _targetAddRotSpline;
        
    private SplineContainer _containerConnectGet;
    private SplineContainer _containerConnectSet;

    private int _idSplineContainerConnectGet;
    private int _idSplineContainerConnectSet;
    // [MenuItem("Tools/Spline Tools 2")]
    // public static void ShowWindow()
    // {
    //     EditorWindow.GetWindow<WindowSplineToolTest>("Инструменты");
    // }

    private void OnGUI()
    {
        _mainScrollPos = EditorGUILayout.BeginScrollView(_mainScrollPos);
        
        // Начало отслеживания изменений
        EditorGUI.BeginChangeCheck();
        
        GUILayout.Label("Spline Tools", EditorStyles.boldLabel); _container = (SplineContainer)EditorGUILayout.ObjectField("Spline Container", _container, typeof(SplineContainer), true);

        GUILayout.Space(10);

        _statusToggleSelectAllSpline = GUILayout.Toggle(_statusToggleSelectAllSpline, "Выбрать все Spline");

        if (_statusToggleSelectAllSpline == false)
        {
            GUILayout.Space(10);

            GUILayout.Label("Id Spline:");
            string strId = GUILayout.TextField(_selectIdSpline.ToString());

            if (strId == "" || int.TryParse(strId, out _) == false)
            {
                _selectIdSpline = 0;
            }
            else
            {
                _selectIdSpline = int.Parse(strId);
            }
        }
        
        if (GUILayout.Button("Отразить Spline по горизонтали"))
        {
            if (_statusToggleSelectAllSpline == true)
            {
                for (int i = 0; i < _container.Splines.Count; i++)
                {
                    var spline = SplineTools.MirrorHorizontalSpline(_container.Splines[i]);
                    _container.Splines[i].Clear();
                    foreach (var knot in spline)
                    {
                        _container.Splines[i].Add(knot);
                    }
                }
            }
            else
            {
                var spline = SplineTools.MirrorHorizontalSpline(_container.Splines[_selectIdSpline]);
                _container.Splines[_selectIdSpline].Clear();
                foreach (var knot in spline)
                {
                    _container.Splines[_selectIdSpline].Add(knot);
                }
            }
        }
        
        GUILayout.Space(7);
        
        if (GUILayout.Button("Отразить Spline по вертикале"))
        {
            if (_statusToggleSelectAllSpline == true)
            {
                for (int i = 0; i < _container.Splines.Count; i++)
                {
                    var spline = SplineTools.MirrorVerticalSpline(_container.Splines[i]);
                    _container.Splines[i].Clear();
                    foreach (var knot in spline)
                    {
                        _container.Splines[i].Add(knot);
                    }
                }
            }
            else
            {
                var spline = SplineTools.MirrorVerticalSpline(_container.Splines[_selectIdSpline]);
                _container.Splines[_selectIdSpline].Clear();
                foreach (var knot in spline)
                {
                    _container.Splines[_selectIdSpline].Add(knot);
                }
            }
        }
        
        GUILayout.Space(7);

        if (GUILayout.Button("Отразить Spline по оси Z"))
        {
            if (_statusToggleSelectAllSpline == true)
            {
                for (int i = 0; i < _container.Splines.Count; i++)
                {
                    var spline = SplineTools.MirrorForwardSpline(_container.Splines[i]);
                    _container.Splines[i].Clear();
                    foreach (var knot in spline)
                    {
                        _container.Splines[i].Add(knot);
                    }
                }
            }
            else
            {
                var spline = SplineTools.MirrorForwardSpline(_container.Splines[_selectIdSpline]);
                _container.Splines[_selectIdSpline].Clear();
                foreach (var knot in spline)
                {
                    _container.Splines[_selectIdSpline].Add(knot);
                }
            }
        }
        
        GUILayout.Space(7);
        
        _targetStartPointSpline = PrintVisibleVector(_targetStartPointSpline);
        
        if (GUILayout.Button("Переместит стартовую точку Spline в указанную точку"))
        {
            if (_statusToggleSelectAllSpline == true)
            {
                for (int i = 0; i < _container.Splines.Count; i++)
                {
                    SplineTools.MoveSplineTargetPoint(_container.Splines[i], _targetStartPointSpline);
                }
            }
            else
            {
                SplineTools.MoveSplineTargetPoint(_container.Splines[_selectIdSpline], _targetStartPointSpline);
            }
        }
        
        GUILayout.Space(7);

        if (GUILayout.Button("Отразит на 180 градусов направления точек на Spline"))
        {
            if (_statusToggleSelectAllSpline == true)
            {
                for (int i = 0; i < _container.Splines.Count; i++)
                {
                    SplineTools.ReverseSplineDirection(_container.Splines[i]);
                }
            }
            else
            {
                SplineTools.ReverseSplineDirection(_container.Splines[_selectIdSpline]);
            }
        }
        
        GUILayout.Space(7);

        _targetRotSpline = PrintVisibleVector(_targetRotSpline, "Rot X:", "Rot Y:", "Rot Z:");
        
        if (GUILayout.Button("Повернет все точки на Spline в указ. напр."))
        {
            if (_statusToggleSelectAllSpline == true)
            {
                for (int i = 0; i < _container.Splines.Count; i++)
                {
                    SplineTools.SetKnotRotation(_container.Splines[i], _targetRotSpline.x, _targetRotSpline.y, _targetRotSpline.z);
                }
            }
            else
            {
                SplineTools.SetKnotRotation(_container.Splines[_selectIdSpline], _targetRotSpline.x, _targetRotSpline.y, _targetRotSpline.z);
            }
        }
        
        GUILayout.Space(7);
        
        _targetAddRotSpline = PrintVisibleVector(_targetAddRotSpline, "Rot X:", "Rot Y:", "Rot Z:");
        
        if (GUILayout.Button("Прибавит указ. угол ко всем точками на Spline"))
        {
            if (_statusToggleSelectAllSpline == true)
            {
                for (int i = 0; i < _container.Splines.Count; i++)
                {
                    SplineTools.SetKnotAddRotation(_container.Splines[i], _targetAddRotSpline.x, _targetAddRotSpline.y, _targetAddRotSpline.z);
                }
            }
            else
            {
                SplineTools.SetKnotAddRotation(_container.Splines[_selectIdSpline], _targetAddRotSpline.x, _targetAddRotSpline.y, _targetAddRotSpline.z);
            }
        }
        
        GUILayout.Space(7);

        if (GUILayout.Button("Пример создание 2D Spline по функции"))
        {
            _container.AddSpline(SplineTools.ExampleCreateSplineFunction2D());
        }
        
        GUILayout.Label($"Пример смотрите в скрипте {nameof(SplineTools)} в методе {nameof(SplineTools.ExampleCreateSplineFunction2D)}");
        
        GUILayout.Space(7);
        
        if (GUILayout.Button("Пример создание 2D Spline по функции, c авто вычислением производной"))
        {
            _container.AddSpline(SplineTools.ExampleCreateSplineFunction2DAutoDerivative());
        }
        
        GUILayout.Label($"Пример смотрите в скрипте {nameof(SplineTools)} в методе {nameof(SplineTools.ExampleCreateSplineFunction2DAutoDerivative)}");
        
        GUILayout.Space(7);
        
        if (GUILayout.Button("Пример создание 3D Spline по функции"))
        {
            _container.AddSpline(SplineTools.ExampleCreateSplineFunction3D());
        }
        
        GUILayout.Label($"Пример смотрите в скрипте {nameof(SplineTools)} в методе {nameof(SplineTools.ExampleCreateSplineFunction3D)}");
        
        GUILayout.Space(7);
        
        if (GUILayout.Button("Пример создание 3D Spline по функции, c авто вычислением производной"))
        {
            _container.AddSpline(SplineTools.ExampleCreateSplineFunction3DAutoDerivative());
        }
        
        GUILayout.Label($"Пример смотрите в скрипте {nameof(SplineTools)} в методе {nameof(SplineTools.ExampleCreateSplineFunction3DAutoDerivative)}");
        
        GUILayout.Label("container Get", EditorStyles.boldLabel); _containerConnectGet = (SplineContainer)EditorGUILayout.ObjectField("Spline Container", _containerConnectGet, typeof(SplineContainer), true);
        
        GUILayout.Label("Id Spline Get:");
        string strIdSelect = GUILayout.TextField(_idSplineContainerConnectGet.ToString());

        if (strIdSelect == "" || int.TryParse(strIdSelect, out _) == false)
        {
            _idSplineContainerConnectGet = 0;
        }
        else
        {
            _idSplineContainerConnectGet = int.Parse(strIdSelect);
        }
        
        GUILayout.Label("container Set Connect", EditorStyles.boldLabel); _containerConnectSet = (SplineContainer)EditorGUILayout.ObjectField("Spline Container", _containerConnectSet, typeof(SplineContainer), true);
        
        GUILayout.Label("Id Spline Connect:");
        strIdSelect = GUILayout.TextField(_idSplineContainerConnectSet.ToString());

        if (strIdSelect == "" || int.TryParse(strIdSelect, out _) == false)
        {
            _idSplineContainerConnectSet = 0;
        }
        else
        {
            _idSplineContainerConnectSet = int.Parse(strIdSelect);
        }
        
        if (GUILayout.Button("Соединить 2 Spline"))
        {
            for (int i = 0; i < _containerConnectGet[_idSplineContainerConnectGet].Count; i++)
            {
                _containerConnectSet[_idSplineContainerConnectSet].Add(_containerConnectGet[_idSplineContainerConnectGet][i]);
            }
        }

        
        // Если что-то изменилось внутри этого блока
        if (EditorGUI.EndChangeCheck())
        {
            // Помечаем объект грязным, чтобы Unity сохранила изменения
            EditorUtility.SetDirty(this); 
        }
        
        EditorGUILayout.EndScrollView();
    }
    

    /// <summary>
    /// Нужен что бы отобразить 3 поля для ввода на одной линии
    /// </summary>
    /// <param name="myVector"></param>
    /// <returns></returns>
    private Vector3 PrintVisibleVector(Vector3 myVector, string textLab1 = "X:", string textLab2 = "Y:", string textLab3 = "Z:")
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label(textLab1, GUILayout.Width(40));
        myVector.x = EditorGUILayout.FloatField(myVector.x, GUILayout.MinWidth(40));

        GUILayout.Space(5); 

        GUILayout.Label(textLab2, GUILayout.Width(40));
        myVector.y = EditorGUILayout.FloatField(myVector.y, GUILayout.MinWidth(40));

        GUILayout.Space(5);

        GUILayout.Label(textLab3, GUILayout.Width(40));
        myVector.z = EditorGUILayout.FloatField(myVector.z, GUILayout.MinWidth(40));

        GUILayout.EndHorizontal();

        return myVector;
    }
}
