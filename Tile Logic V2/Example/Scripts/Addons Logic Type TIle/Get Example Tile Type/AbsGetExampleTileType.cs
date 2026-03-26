using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Наследник должен возращать заспавненный экземпляр префаба, на основе переданного типа таила
/// </summary>
public abstract class AbsGetExampleTileType : MonoBehaviour
{
    public abstract MarkTilePrefab GetTileExample(KeyExampleTipeTile type);
}
