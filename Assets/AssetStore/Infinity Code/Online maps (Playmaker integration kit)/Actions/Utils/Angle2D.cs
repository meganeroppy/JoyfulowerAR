using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.UTILS)]
    [Tooltip("Angle between two points.")]
    public class Angle2D : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Point 1")]
        public FsmVector2 point1;

        [RequiredField]
        [Tooltip("Point 2")]
        public FsmVector2 point2;

        [UIHint(UIHint.Variable)]
        [Tooltip("Angle")]
        public FsmFloat storeAngle;

        public override void Reset()
        {
            point1 = null;
            point2 = null;
            storeAngle = null;
        }

        public override void OnEnter()
        {
            if (point1.IsNone || point2.IsNone)
            {
                Debug.Log("Points connot be null.");
                return;
            }
            storeAngle.Value = OnlineMapsUtils.Angle2D(point1.Value, point2.Value);
            Finish();
        }
    }
}