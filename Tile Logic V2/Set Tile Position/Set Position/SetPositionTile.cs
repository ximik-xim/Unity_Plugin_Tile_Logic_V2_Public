using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Логика определяющая куда(по каким координатам) будет установлен следующий таил
/// (учитывать или не учитывать размеры таила это вопрос конкретной реализации)
/// </summary>
public class SetPositionTile : TileLogicTaskGroupTilePositionInfo
{
    private void Awake()
    {
        StartInit();
    }
}
