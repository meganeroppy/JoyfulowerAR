using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Gets route from FindDirection response.")]
    public class GetRouteByIndex : FsmStateAction
    {
        [RequiredField]
        [Tooltip("FindDirection response object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject responseObject;

        [RequiredField]
        [Tooltip("Route index.")]
        public FsmInt routeIndex = 0;

        [Tooltip("Route object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject storeRouteObject;

        [Tooltip("Steps count.")]
        [UIHint(UIHint.Variable)]
        public FsmInt storeStepCount;

        public override void Reset()
        {
            responseObject = null;
            routeIndex = 0;
            storeRouteObject = null;
            storeStepCount = null;
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

            if (responseObject.IsNone) return;

            if (responseObject.Value.GetType() != typeof(FindDirectionResponseWrapper))
            {
                Debug.Log("Its not FindDirectionResponseWrapper instance");
                return;
            }

            FindDirectionResponseWrapper wrapper = responseObject.Value as FindDirectionResponseWrapper;

            if (routeIndex.Value < 0 || routeIndex.Value >= wrapper.count) return;

            storeRouteObject.Value = new RouteWrapper(wrapper.routes[routeIndex.Value]);
            storeStepCount.Value = wrapper.routes[routeIndex.Value].Count;
        }
    }
}