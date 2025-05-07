using UnityEngine;
using UnityEngine.Splines;

public class GetSplineContainerDefault : AbsGetSplineContainer
{
   [SerializeField] 
   private SplineContainer _splineContainer;

   public override SplineContainer GetContainer()
   {
      return _splineContainer;
   }
}
