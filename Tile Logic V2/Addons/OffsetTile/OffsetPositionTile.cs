using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetPositionTile : AbsOffsetPositionTile
{
    private Vector3 _offset;

    public override void SetOffset(Vector3 offset)
    {
        _offset = offset;
    }

    public override Vector3 GetOffset()
    {
        return _offset;
    }
}
