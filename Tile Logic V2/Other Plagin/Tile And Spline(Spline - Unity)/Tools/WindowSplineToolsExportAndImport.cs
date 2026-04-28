using System;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
/// <summary>
/// Пока тестовый набросок окна с инструментами для работы со Spline
/// </summary>
public class WindowSplineToolsExportAndImport : EditorWindow
{
    private SplineContainer _container;
    private string _splineData = "";
    Vector2 _mainScrollPos = Vector2.zero;

    private bool _statusToggleSelectAllSpline;
    private int _selectIdSpline = 0;
    
    [MenuItem("Tools/Spline Tools")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<WindowSplineToolsExportAndImport>("Экспорт/Импорт");
        EditorWindow.GetWindow<WindowSplineToolGeneral>("Инструменты");
    }

    private void OnGUI()
    {
        GUILayout.Label("Spline Tools", EditorStyles.boldLabel);
        _container = (SplineContainer)EditorGUILayout.ObjectField("Spline Container", _container, typeof(SplineContainer), true);

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

        if (GUILayout.Button("Экспорт данных об Spline как текст"))
        {
            ExportSpline();
        }

        if (GUILayout.Button("Экспорт данных об Spline как кастомный JSON"))
        {
            ExportSplineCustomJSON();
        }
        
        if (GUILayout.Button("Импорт данных об Spline как текст"))
        {
            ImportSpline(_splineData);
        }
        
        if (GUILayout.Button("Импорт данных об Spline как кастомный JSON"))
        {
            ImportSplineCutomJSON(_splineData);
        }

        GUILayout.Space(10);
        
        GUILayout.Label("Данные об Spline");
        
        _mainScrollPos = EditorGUILayout.BeginScrollView(_mainScrollPos, GUILayout.Height(300));

        _splineData = EditorGUILayout.TextArea(_splineData, GUILayout.ExpandHeight(true));

        EditorGUILayout.EndScrollView();
    }

    void ExportSpline()
    {
        if (_container == null)
        {
            return;
        }

        _splineData = "";

        StringBuilder sb = new StringBuilder();
        
        if (_statusToggleSelectAllSpline == false)
        {
            sb.AppendLine($"--- ID SPLINE = {_selectIdSpline} ---");
                
            var spline = _container.Splines[_selectIdSpline];

            for (int i = 0; i < spline.Count; i++)
            {
                var knot = spline[i];

                Vector3 pos = knot.Position;
                Vector3 tanIn = knot.TangentIn;
                Vector3 tanOut = knot.TangentOut;
                Quaternion rot = knot.Rotation;

                Vector3 euler = rot.eulerAngles;

                sb.AppendLine($"--- POINT {i} ---");
                sb.AppendLine($"Pos: {pos.x} {pos.y} {pos.z}");
                sb.AppendLine($"Rot: {euler.x} {euler.y} {euler.z}");
                sb.AppendLine($"TanIn: {tanIn.x} {tanIn.y} {tanIn.z}");
                sb.AppendLine($"TanOut: {tanOut.x} {tanOut.y} {tanOut.z}");
            }
        }
        else
        {
            for (int j = 0; j < _container.Splines.Count; j++)
            {
                sb.AppendLine($"--- ID SPLINE = {j} ---");
                
                var spline = _container.Splines[j];

                for (int i = 0; i < spline.Count; i++)
                {
                    var knot = spline[i];

                    Vector3 pos = knot.Position;
                    Vector3 tanIn = knot.TangentIn;
                    Vector3 tanOut = knot.TangentOut;
                    Quaternion rot = knot.Rotation;

                    Vector3 euler = rot.eulerAngles;

                    sb.AppendLine($"--- POINT {i} ---");
                    sb.AppendLine($"Pos: {pos.x} {pos.y} {pos.z}");
                    sb.AppendLine($"Rot: {euler.x} {euler.y} {euler.z}");
                    sb.AppendLine($"TanIn: {tanIn.x} {tanIn.y} {tanIn.z}");
                    sb.AppendLine($"TanOut: {tanOut.x} {tanOut.y} {tanOut.z}");
                }
            }
        }
        
        _splineData += sb.ToString();
    }

    private void ExportSplineCustomJSON()
    {
        if (_container == null)
        {
            return;
        }

        _splineData = "";
        
        StringBuilder sb = new StringBuilder();

        if (_statusToggleSelectAllSpline == false)
        {
            string text = $"splineId : {_selectIdSpline}\n";
            sb.AppendLine(text);

            var spline = _container.Splines[_selectIdSpline];
            for (int i = 0; i < spline.Count; i++)
            {
                text = $"knotId : {i}\n" +
                       $"knotData :" + JsonUtility.ToJson(spline[i], true);

                sb.AppendLine(text);
            }
        }
        else
        {
            for (int j = 0; j < _container.Splines.Count; j++)
            {
                string text = $"splineId : {_selectIdSpline}\n";
                sb.AppendLine(text);

                var spline = _container.Splines[j];

                for (int i = 0; i < spline.Count; i++)
                {
                    text = $"knotId : {i}\n" +
                           $"knotData :" + JsonUtility.ToJson(spline[i], true);

                    sb.AppendLine(text);
                }
            }
        }

        _splineData += sb.ToString();
    }
    
    private void ImportSplineCutomJSON(string customJson)
    {
        if (_container == null)
        {
            return;
        }
    
        // текст по строкам
        string[] splineBlocks = customJson.Split(new[] { "splineId" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var block in splineBlocks)
        {
            Spline newSpline = new Spline();
            _container.AddSpline(newSpline);

            // Ищем все вхождения JSON объектов в этом блоке сплайна
            // Мы ищем текст между "{" и "}"
            int currentIndex = 0;
            while ((currentIndex = block.IndexOf('{', currentIndex)) != -1)
            {
                int bracketCount = 1;
                int endSearch = currentIndex + 1;

                // Находим парную закрывающую скобку для текущего объекта
                while (bracketCount > 0 && endSearch < block.Length)
                {
                    if (block[endSearch] == '{') bracketCount++;
                    else if (block[endSearch] == '}') bracketCount--;
                    endSearch++;
                }

                if (bracketCount == 0)
                {
                    // Вырезаем чистый JSON
                    string jsonKnot = block.Substring(currentIndex, endSearch - currentIndex);
                
                    try
                    {
                        // BezierKnot - это структура, пробуем парсить напрямую
                        BezierKnot knot = JsonUtility.FromJson<BezierKnot>(jsonKnot);
                        newSpline.Add(knot);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Ошибка в блоке JSON: {e.Message}\nСодержимое: {jsonKnot}");
                    }
                
                    currentIndex = endSearch;
                }
                else
                {
                    break; // Не нашли закрывающую скобку
                }
            }
        }
    }

    private void ImportSpline(string splineData)
    {
        if (_container == null)
        {
            return;
        }

        string[] lines = splineData.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        Spline currentSpline = null;
    
        // Временные переменные для сборки одного узла
        Vector3 pos = Vector3.zero;
        Vector3 tanIn = Vector3.zero;
        Vector3 tanOut = Vector3.zero;
        Quaternion rot = Quaternion.identity;

        foreach (string line in lines)
        {
            string trimmed = line.Trim();

            // 1. Обнаружили начало нового сплайна
            if (trimmed.StartsWith("--- ID SPLINE"))
            {
                currentSpline = new Spline();
                _container.AddSpline(currentSpline);
                continue;
            }

            // 2. Считываем данные координат
            if (trimmed.StartsWith("Pos:"))
            {
                pos = ParseVector(trimmed.Replace("Pos:", ""));
            }
            
            else if (trimmed.StartsWith("Rot:"))
            {
                rot = Quaternion.Euler(ParseVector(trimmed.Replace("Rot:", "")));
            }
            
            else if (trimmed.StartsWith("TanIn:"))
            {
                tanIn = ParseVector(trimmed.Replace("TanIn:", ""));
            }
            
            else if (trimmed.StartsWith("TanOut:"))
            {
                tanOut = ParseVector(trimmed.Replace("TanOut:", ""));
            
                // TanOut — последняя строка в блоке точки, 
                // значит узел собран и его можно добавлять
                if (currentSpline != null)
                {
                    currentSpline.Add(new BezierKnot(pos, tanIn, tanOut, rot));
                }
            }
        }
    }

    Vector3 ParseVector(string line)
    {
        string[] parts = line.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 3) return Vector3.zero;

        float.TryParse(parts[0], out float x);
        float.TryParse(parts[1], out float y);
        float.TryParse(parts[2], out float z);

        return new Vector3(x, y, z);
    }
}
