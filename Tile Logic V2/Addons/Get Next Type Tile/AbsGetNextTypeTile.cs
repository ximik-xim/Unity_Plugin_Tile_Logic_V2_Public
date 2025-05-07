using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Наследник будет определять какой будет следующих тип таила
/// (может основываясь на последнем типе таила, а может еще как то)
/// </summary>
public abstract class AbsGetNextTypeTile : MonoBehaviour
{
    public abstract KeyExampleTipeTile GetTypeTile(KeyExampleTipeTile lastKey);
}
