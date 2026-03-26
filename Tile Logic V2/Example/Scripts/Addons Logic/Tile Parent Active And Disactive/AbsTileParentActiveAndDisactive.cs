using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Нужен, т.к на самом родители таила всегда будут уникальные компоненты(и скрипты), которые отдельно не получиться вынести
/// (да тупо, но только так)
/// </summary>
public abstract class AbsTileParentActiveAndDisactive : MonoBehaviour
{
   public abstract void ActiveTile();
   public abstract void DisactiveTile();
}
