using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Gets route step point by index.")]
    public class GetRouteStepPointByIndex : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Step object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject stepObject;

        [Tooltip("Index of point.")]
        [UIHint(UIHint.Variable)]
        public FsmInt pointIndex;

        [Tooltip("Point.")]
        [UIHint(UIHint.Variable)]
        public FsmVector2 storePoint;

        public override void Reset()
        {
            stepObject = null;
            pointIndex = null;
            storePoint = null;
        }

        public override void OnEnter()
        {
            DoEnter();
            Finish();
        }

        private void DoEnter()
        {
            if (OnlineMaps.instance == null)
            {
                Debug.LogError("Online Maps not found.");
                return;
            }

            if (stepObject.IsNone) return;

            if (stepObject.Value.GetType() != typeof(RouteStepWrapper))
            {
                Debug.Log("Its not RouteStepWrapper instance");
                return;
            }

            RouteStepWrapper wrapper = stepObject.Value as RouteStepWrapper;

            int i = pointIndex.Value;
            if (i < 0 || i >= wrapper.step.points.Count)
            {
                Debug.Log("The index out of bounds.");
                return;
            }

            storePoint.Value = wrapper.step.points[i];
        }
    }
}