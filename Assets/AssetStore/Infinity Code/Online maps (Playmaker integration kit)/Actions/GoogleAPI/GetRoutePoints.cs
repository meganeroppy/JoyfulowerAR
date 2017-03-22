using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Gets points from route.")]
    public class GetRoutePoints : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Route object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject routeObject;

        [Tooltip("Points object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject storePoints;

        [Tooltip("Count points.")]
        [UIHint(UIHint.Variable)]
        public FsmInt storeCountPoints;

        public override void Reset()
        {
            routeObject = null;
            storePoints = null;
            storeCountPoints = null;
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
            storePoints.Value = new RoutePointsWrapper(OnlineMapsDirectionStep.GetPoints(wrapper.steps));
            storeCountPoints.Value = (storePoints.Value as RoutePointsWrapper).count;
        }
    }
}