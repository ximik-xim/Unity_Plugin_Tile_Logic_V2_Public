using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// SO которое содержит в себе список всех таилов по типу
/// </summary>
[CreateAssetMenu(menuName = "Tile Logic V2/Data List Type Tile SO")]
public class DataListTypeTileSO : ScriptableObject, IInitScripObj
{
    public event Action OnInit;
    public bool IsInit => _isInit;
    private bool _isInit = false;
    
    [SerializeField]
    private List<AbsKeyData<GetDataSO_KeyExampleTipeTile, List<MarkTilePrefab> >> _list = new List<AbsKeyData<GetDataSO_KeyExampleTipeTile, List<MarkTilePrefab>>>();

    private Dictionary<string, List<MarkTilePrefab>> _data = new Dictionary<string, List<MarkTilePrefab>>();
    
        public void InitScripObj()
        {
#if UNITY_EDITOR
        
            EditorApplication.playModeStateChanged -= OnUpdateStatusPlayMode;
            EditorApplication.playModeStateChanged += OnUpdateStatusPlayMode;

            //На случай если event playModeStateChanged не отработает при входе в режим PlayModeStateChange.EnteredPlayMode (такое может быть, и как минимум по этому нужна пер. bool _init)
            if (EditorApplication.isPlaying == true)
            {
                if (_isInit == false)
                {
                    Awake();
                }
            }
#endif
        }
        
#if UNITY_EDITOR
        private void OnUpdateStatusPlayMode(PlayModeStateChange obj)
        {
            //При выходе из Play Mode произвожу очистку данных(тем самым эмулирую что при след. запуске(вхождение в Play Mode) данные будут обнулены)
            if (obj == PlayModeStateChange.ExitingPlayMode)
            {
                if (_isInit == true)
                {
                    _isInit = false;
                }
            }
        
            // При запуске игры эмулирую иниц. SO(По идеи не совсем верно, т.к Awake должен произойти немного раньше, но пофиг)(как показала практика метод может не сработать)
            if (obj == PlayModeStateChange.EnteredPlayMode)
            {
                if (_isInit == false)
                {
                    Awake();
                }
            
            }
        }
#endif
        
    private void Awake()
    {
        if (_isInit == false)
        {
            _data.Clear();

             foreach (var VARIABLE in _list)
             {
                 _data.Add(VARIABLE.Key.GetData().GetKey(), VARIABLE.Data);
             }
            
            _isInit = true;
            OnInit?.Invoke();
        }
        
    }
        
    public List<MarkTilePrefab> GetTiles(KeyExampleTipeTile key)
    {
        return _data[key.GetKey()];
    }

    public List<KeyExampleTipeTile> GetAllType()
    {
        List<KeyExampleTipeTile> data = new List<KeyExampleTipeTile>();
        foreach (var VARIABLE in _list)
        {
            data.Add(VARIABLE.Key.GetData());
        }

        return data;
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= OnUpdateStatusPlayMode;
#endif
    }
}
