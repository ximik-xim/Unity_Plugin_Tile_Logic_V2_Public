using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Методы для работы со Spline
/// </summary>
public class SplineTools : MonoBehaviour
{
    /// <summary>
    /// Отражает Spline по горизонтали(зеркалит объект относительно плоскости YZ, инвертирует X)
    /// </summary>
    public static Spline MirrorHorizontalSpline(Spline spline)
    {
        Spline newSpline = new Spline();
        
        for (int i = 0; i < spline.Count; i++)
        {
            BezierKnot knot = spline[i];
            
            Vector3 pos = knot.Position;
            pos.x *= -1f;
            
            Vector3 tanIn = knot.TangentIn;
            Vector3 tanOut = knot.TangentOut;
            
            Quaternion rot = knot.Rotation;
            Vector3 euler = rot.eulerAngles;
            euler.y = -euler.y;

            // Нормализация угла (чтобы не было -30 = 330)
            if (euler.x < 0f)
            {
                euler.x += 360f;
            }
            
            if (euler.y < 0f)
            {
                euler.y += 360f;
            }
            
            if (euler.z < 0f)
            {
                euler.z += 360f;
            }
            
            rot = Quaternion.Euler(euler);
            
            knot.Position = pos;
            knot.TangentIn = tanIn;
            knot.TangentOut = tanOut;
            knot.Rotation = rot;

            newSpline.Add(knot);
        }

        return newSpline;
    }
    
    /// <summary>
    /// Отражает Spline по вертикале (зеркалит объект относительно плоскости XZ, инвертирует Y)
    /// </summary>
    public static Spline MirrorVerticalSpline(Spline spline)
    {
        Spline newSpline = new Spline();
        
        for (int i = 0; i < spline.Count; i++)
        {
            BezierKnot knot = spline[i];
            
            Vector3 pos = knot.Position;
            pos.y *= -1f;
            
            Vector3 tanIn = knot.TangentIn;
            Vector3 tanOut = knot.TangentOut;
            
            Quaternion rot = knot.Rotation;
            Vector3 euler = rot.eulerAngles;
            euler.x = -euler.x;
            euler.z = -euler.z;

            // Нормализация угла (чтобы не было -30 = 330)
            if (euler.x < 0f)
            {
                euler.x += 360f;
            }
            
            if (euler.y < 0f)
            {
                euler.y += 360f;
            }
            
            if (euler.z < 0f)
            {
                euler.z += 360f;
            }
            
            rot = Quaternion.Euler(euler);
            
            knot.Position = pos;
            knot.TangentIn = tanIn;
            knot.TangentOut = tanOut;
            knot.Rotation = rot;

            newSpline.Add(knot);
        }

        return newSpline;
    }
    
    /// <summary>
    /// Отражает Spline по оси Z (зеркалит объект относительно плоскости XY, инвертирует Z)
    /// </summary>
    public static Spline MirrorForwardSpline(Spline spline)
    {
        Spline newSpline = new Spline();
        
        for (int i = spline.Count - 1; i >= 0; i--)
        {
            BezierKnot knot = spline[i];
            
            Vector3 pos = knot.Position;
            pos.z *= -1f;
            
            Vector3 tanIn = knot.TangentOut;
            Vector3 tanOut = knot.TangentIn;

            tanIn.z *= -1f;
            tanOut.z *= -1f;
            
            Quaternion rot = knot.Rotation;
            Vector3 euler = rot.eulerAngles;

            euler.y = 360 - euler.y;
            
            euler.x = -euler.x;
            euler.z = -euler.z;
            
            // Нормализация угла (чтобы не было -30 = 330)
            if (euler.x < 0f)
            {
                euler.x += 360f;
            }
            
            if (euler.y < 0f)
            {
                euler.y += 360f;
            }
            
            if (euler.z < 0f)
            {
                euler.z += 360f;
            }

            knot.Position = pos;
            knot.TangentIn = tanIn;
            knot.TangentOut = tanOut;
            knot.Rotation = Quaternion.Euler(euler);

            newSpline.Add(knot);
        }
        
        return newSpline;
    }


    /// <summary>
    /// Переместит стартовую точку Spline (и весь остальной Spline) в указанную точку
    /// (локальные коорд)
    /// </summary>
    public static void MoveSplineTargetPoint(Spline spline, Vector3 targetPos)
    {
        if (spline.Count > 0)
        {
            Vector3 offset = new Vector3(targetPos.x - spline[0].Position.x, targetPos.y - spline[0].Position.y, targetPos.z - spline[0].Position.z);
            for (int i = 0; i < spline.Count; i++)
            {
                BezierKnot knot = spline[i];
                knot.Position = new Vector3(spline[i].Position.x + offset.x, spline[i].Position.y + offset.y, spline[i].Position.z + offset.z);
                spline[i] = knot;
            }
        }
    }

    /// <summary>
    /// Отразит на 180 градусов направления Spline
    /// </summary>
    public static void ReverseSplineDirection(Spline spline)
    {
        for (int i = 0; i < spline.Count; i++)
        {
            BezierKnot knot = spline[i];
            
            Vector3 pos = knot.Position;
            
            Vector3 tanIn = knot.TangentOut;
            Vector3 tanOut = knot.TangentIn;
            
            Quaternion rot = knot.Rotation * Quaternion.Euler(0, 180, 0);
            
            knot.Position = pos;
            knot.TangentIn = tanIn;
            knot.TangentOut = tanOut;
            knot.Rotation = rot;

            spline[i] = knot;
        }
    }

    /// <summary>
    /// Повернет все Knot в указ. напровлении(но линию при этом не сломает, т.к пересчитает все knot)
    /// </summary>
    public static void SetKnotRotation(Spline spline, float rotX, float rotY, float rotZ)
    {
        Quaternion targetRot = Quaternion.Euler(new Vector3(rotX, rotY, rotZ));

        for (int i = 0; i < spline.Count; i++)
        {
            BezierKnot knot = spline[i];
            
            Quaternion diff = targetRot * Quaternion.Inverse(knot.Rotation);
            Quaternion invDiff = Quaternion.Inverse(diff);
            
            knot.Rotation = targetRot;
            
            knot.TangentIn = invDiff * knot.TangentIn;
            knot.TangentOut = invDiff * knot.TangentOut;

            spline[i] = knot;
        }
    }
    
    /// <summary>
    /// Прибавит указ. укгол ко всем Knot(но линию при этом не сломает, т.к пересчитает все knot)
    /// </summary>
    public static void SetKnotAddRotation(Spline spline, float rotX, float rotY, float rotZ)
    {
        Quaternion addRot = Quaternion.Euler(new Vector3(rotX, rotY, rotZ));
        
        Quaternion invAddRot = Quaternion.Inverse(addRot);

        for (int i = 0; i < spline.Count; i++)
        {
            BezierKnot knot = spline[i];
            
            knot.Rotation = knot.Rotation * addRot;
            
            knot.TangentIn = invAddRot * knot.TangentIn;
            knot.TangentOut = invAddRot * knot.TangentOut;

            spline[i] = knot;
        }
    }
    

    public delegate Vector3 MyDelegate3D(float xValue);
    
    /// Строит Spline по указанной функции (хоть 3D, хоть 2D)
    /// startT - стартовое значение времени (ну или x)
    /// endT - конечное значение времени (ну или x)
    /// stepT - шаг с которым будут вычесляться точки  
    /// functionDerivative - скорость изменения функции. Она показывает, под каким углом идет линия в конкретной точке
    public static Spline CreateFuncSpline(MyDelegate3D funcPos, float startT, float endT, float stepT, MyDelegate3D funcDeriv = null)
    {
        Spline newSpline = new Spline();
        float t = startT;

        while (t <= endT)
        {
            Vector3 position = funcPos(t);
    
            Vector3 direction;
            if (funcDeriv == null)
            {
                float delta = 0.001f;
                // Используем центральную разность для большей точности в 3D
                direction = (funcPos(t + delta) - funcPos(t - delta)) / (2 * delta);
            }
            else
            {
                direction = funcDeriv(t);
            }

            // 1. Направление
            Vector3 normalizedDir = direction.normalized;

            // 2. Вращение (Z смотрит вдоль производной)
            Quaternion rotation = Quaternion.LookRotation(normalizedDir, Vector3.up);

            // 3. Локальные касательные
            // Используем 0.333f от физического расстояния между точками.
            // Если stepT — это время, а не расстояние, то магнитуда производной 
            // как раз подскажет нам скорость (расстояние в секунду).
            float magnitude = direction.magnitude; 
            float tangentLength = (magnitude * stepT) * 0.3333f;

            // В локальном пространстве LookRotation ось Z — это "вперед"
            Vector3 tanOut = new Vector3(0, 0, tangentLength);
            Vector3 tanIn = new Vector3(0, 0, -tangentLength);

            newSpline.Add(new BezierKnot(position, tanIn, tanOut, rotation));

            t += stepT;
            if (t > endT && t - stepT < endT)
            {
                t = endT;
            }
        }

        return newSpline;
    }
    
    /// <summary>
    /// Пример использования 3D создателя Spline по указанной функции
    /// Через указанный шаг
    /// С указанием производной для данной функции
    /// </summary>
    public static Spline ExampleCreateSplineFunction3D()
    {
        return CreateFuncSpline
        ((t) => { return ExampleFuncSpiral(9f, 5f, t); },
            0, 20, 0.5f,
            (t) => { return GetSpiralDerivative(9f, 5f, t); }
        );
    }
    
    /// <summary>
    /// Пример использования 2D создателя Spline по указанной функции
    /// Через указанный шаг
    /// С авто вычислением производной
    /// </summary>
    public static Spline ExampleCreateSplineFunction3DAutoDerivative()
    {
        return CreateFuncSpline
        ((t) => { return ExampleFuncSpiral(9f, 5f, t); },
            0, 20, 0.5f
        );
    }

    /// <summary>
    /// Пример использования 2D создателя Spline по указанной функции
    /// Через указанный шаг
    /// С указанием производной для данной функции
    /// </summary>
    public static Spline ExampleCreateSplineFunction2D()
    {
        // Т.к это 2D функция f(x), то вместо X передаем T
        return CreateFuncSpline
        ((t) => { return new Vector3(t, ExampleFuncFadingSine(t), 0); },
            0, 20, 0.5f,
            //а тут исп 1, в x т.к выше мы исп t вместо X, а производная от T это 1 
            (t) => { return new Vector3(1f, FadingSineDerivative(t), 0); }
        );
    }
    
    /// <summary>
    /// Пример использования 2D создателя Spline по указанной функции
    /// Через указанный шаг
    /// С авто вычислением производной
    /// </summary>
    public static Spline ExampleCreateSplineFunction2DAutoDerivative()
    {
        // Т.к это 2D функция f(x), то вместо X передаем T
        return CreateFuncSpline
        ((t) => { return new Vector3(t, ExampleFuncFadingSine(t), 0); },
            0, 20, 0.5f
        );
    }
    
    /// <summary>
    /// Пример функции спирали
    /// radius - радиус
    /// heightPerTurn - Высота подьема спирали
    /// T - это по сути точка, при котором будет вычеслено все остальное
    /// </summary>
    private static Vector3 ExampleFuncSpiral(float radius, float heightPerTurn, float t)
    {
        float x = math.cos(t) * radius;
        float z = math.sin(t) * radius;
        float y = t * heightPerTurn;

        return new Vector3(x, y, z);
    }
    
    /// <summary>
    /// Пример производной функции спирали
    /// radius - радиус
    /// heightPerTurn - Высота подьема спирали
    /// T - это по сути точка, при котором будет вычеслено все остальное
    /// </summary>
    private static Vector3 GetSpiralDerivative(float radius, float heightPerTurn, float t)
    {
        float dx = -math.sin(t) * radius;
        float dz = math.cos(t) * radius;
        float dy = heightPerTurn; // Просто константа из формулы y = t * heightPerTurn

        return new Vector3(dx, dy, dz);
    }
        
    /// <summary>
    ///Пример функции для 2D метода создание Spline
    /// </summary>
    /// <returns></returns>
    private static float ExampleFuncFadingSine(float x)
    {
        // Амплитуда 5, частота 1.0, затухание через экспоненту
        return 5f * math.sin(x) * math.exp(-0.2f * x);
    }
    
    /// <summary>
    /// Производная функции выше
    /// </summary>
    private static float FadingSineDerivative(float x)
    {
        // Математика: производная произведения (u*v)' = u'v + uv'
        float amplitude = 5f;
        float decay = math.exp(-0.2f * x);
        float sinVal = math.sin(x);
        float cosVal = math.cos(x);
    
        return amplitude * (cosVal * decay - 0.2f * sinVal * decay);
    }

    /// <summary>
    /// Соединит 2 Spline в 1
    /// </summary>
    public Spline ConnectSpline(Spline setConnect, Spline getSpline)
    {
        Spline spline = new Spline();
            
        foreach (var VARIABLE in setConnect)
        {
            spline.Add(VARIABLE);
        }
            
        foreach (var VARIABLE in getSpline)
        {
            spline.Add(VARIABLE);
        }

        return spline;
    }
    

}
