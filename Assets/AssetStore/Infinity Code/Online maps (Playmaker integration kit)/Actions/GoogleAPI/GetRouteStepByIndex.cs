using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Gets step from route.")]
    public class GetRouteStepByIndex : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Route object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject routeObject;

        [Tooltip("Step index.")]
        public FsmInt stepIndex = 0;

        [Tooltip("Step object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject storeStepObject;

        public override void Reset()
        {
            routeObject = null;
            stepIndex = null;
            storeStepObject = null;
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

            if (routeObject.IsNone) return;

            if (routeObject.Value.GetType() != typeof(RouteWrapper))
            {
                Debug.Log("Its not RouteWrapper instance");
                return;
            }

            RouteWrapper wrapper = routeObject.Value as RouteWrapper;
            if (stepIndex.Value < 0 || stepIndex.Value >= wrapper.count) return;
            storeStepObject.Value = new RouteStepWrapper(wrapper.steps[stepIndex.Value]);
        }
    }
}