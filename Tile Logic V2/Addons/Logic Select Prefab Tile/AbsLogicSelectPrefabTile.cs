
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Наследники определяют логику выбора конкретного префаба из списка префабов
/// </summary>
public abstract class AbsLogicSelectPrefabTile : MonoBehaviour
{
  public abstract MarkTilePrefab GetTile(List<MarkTilePrefab> listPrefab);
}
