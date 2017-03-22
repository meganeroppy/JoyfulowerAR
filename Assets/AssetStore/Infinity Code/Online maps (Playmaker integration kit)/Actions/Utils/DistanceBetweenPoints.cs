using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.UTILS)]
    [Tooltip("The distance between two geographical coordinates in km.")]
    public class DistanceBetweenPoints : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Coordinate (X - Lng, Y - Lat)")]
        public FsmVector2 coordinate1;
        
        [RequiredField]
        [Tooltip("Coordinate (X - Lng, Y - Lat)")]
        public FsmVector2 coordinate2;

        [UIHint(UIHint.Variable)]
        [Tooltip("Distance in km (X - Lng distance, Y - Lat distance)")]
        public FsmVector2 storeDistance;

        [UIHint(UIHint.Variable)]
        [Tooltip("Distance in km")]
        public FsmFloat storeMagnitude;

        public override void Reset()
        {
            coordinate1 = null;
            coordinate2 = null;
            storeDistance = null;
            storeMagnitude = null;
        }

        public override void OnEnter()
        {
            storeDistance.Value = OnlineMapsUtils.DistanceBetweenPoints(coordinate1.Value, coordinate2.Value);
            storeMagnitude.Value = storeDistance.Value.magnitude;
            Finish();
        }
    }
}