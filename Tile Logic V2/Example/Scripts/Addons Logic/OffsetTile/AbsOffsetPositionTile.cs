using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsOffsetPositionTile : MonoBehaviour
{
   public abstract void SetOffset(Vector3 offset);
   public abstract Vector3 GetOffset();

}
