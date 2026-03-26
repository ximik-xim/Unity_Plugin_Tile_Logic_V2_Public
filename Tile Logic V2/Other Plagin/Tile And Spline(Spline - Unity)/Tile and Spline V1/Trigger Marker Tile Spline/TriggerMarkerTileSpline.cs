using System;
using UnityEngine;

public class TriggerMarkerTileSpline : MonoBehaviour
{
    public event Action<AbsTileSplineMarker> OnTrigger;
    
  private void OnTriggerEnter(Collider other)
  {
      AbsTileSplineMarker marker = other.GetComponent<AbsTileSplineMarker>();

      if (marker != null)
      {
          OnTrigger?.Invoke(marker);
      }
  }
}
