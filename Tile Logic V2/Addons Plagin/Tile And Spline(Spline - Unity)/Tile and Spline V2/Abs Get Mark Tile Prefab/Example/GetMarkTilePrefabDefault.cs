using System;
using UnityEngine;

public class GetMarkTilePrefabDefault : AbsGetMarkTilePrefab
{
    [SerializeField] 
    private MarkTilePrefab _prefab;

    public override event Action OnInit;

    public override bool IsInit => true;

    private void Awake()
    {
        OnInit?.Invoke();
    }

    public override MarkTilePrefab GetTilePrefab()
    {
        return Instantiate(_prefab);
    }
}
