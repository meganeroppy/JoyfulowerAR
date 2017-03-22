using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Draw route on map by FindDirection response index.")]
    public class DrawRouteByIndex : FsmStateAction
    {
        [RequiredField]
        [Tooltip("FindDirection response object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject responseObject;

        [RequiredField]
        [Tooltip("Route index.")]
        public FsmInt routeIndex = 0;

        [Tooltip("Line color.")]
        public FsmColor color = Color.green;

        [Tooltip("Line weight.")]
        public FsmFloat weight = 1;

        [UIHint(UIHint.Variable)]
        [Tooltip("Line drawing element.")]
        public FsmObject storeDrawingElement;

        public override void Reset()
        {
            responseObject = null;
            routeIndex = 0;
            color = Color.green;
            weight = 1;
            storeDrawingElement = null;
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
                Debug.Log("Response Object is not FindDirectionResponseWrapper instance");
                return;
            }

            FindDirectionResponseWrapper wrapper = responseObject.Value as FindDirectionResponseWrapper;
            if (routeIndex.Value < 0 && routeIndex.Value >= wrapper.count || wrapper.routes == null) return;

            List<Vector2> points = OnlineMapsDirectionStep.GetPoints(wrapper.routes[routeIndex.Value]);
            OnlineMapsDrawingLine line = new OnlineMapsDrawingLine(points, color.Value, weight.Value);
            OnlineMaps.instance.AddDrawingElement(line);
            storeDrawingElement.Value = new DrawingElementWrapper(line);
        }
    }
}