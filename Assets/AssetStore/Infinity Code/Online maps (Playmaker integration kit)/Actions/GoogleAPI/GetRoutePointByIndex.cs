using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Gets point from route, step or points object.")]
    public class GetRoutePointByIndex : FsmStateAction
    {
        [Tooltip("Route object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject routeObject;

        [Tooltip("Points object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject pointsObject;

        [Tooltip("Step object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject stepObject;

        [RequiredField]
        [Tooltip("Point index.")]
        public FsmInt pointIndex;

        [Tooltip("Point.")]
        [UIHint(UIHint.Variable)]
        public FsmVector2 storePoint;

        public override void Reset()
        {
            routeObject = null;
            pointsObject = null;
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

            List<Vector2> points;

            if (!routeObject.IsNone)
            {
                if (routeObject.Value.GetType() != typeof(RouteWrapper))
                {
                    Debug.Log("Route Object is not RouteWrapper instance");
                    return;
                }
                points = OnlineMapsDirectionStep.GetPoints((routeObject.Value as RouteWrapper).steps);
            }
            else if (!pointsObject.IsNone)
            {
                if (pointsObject.Value.GetType() != typeof(RoutePointsWrapper))
                {
                    Debug.Log("Points Object is not RoutePointsWrapper instance");
                    return;
                }
                points = (pointsObject.Value as RoutePointsWrapper).points;
            }
            else if (!stepObject.IsNone)
            {
                if (stepObject.Value.GetType() != typeof (RouteStepWrapper))
                {
                    Debug.Log("Step Object is not RouteStepWrapper instance");
                    return;
                }
                points = (stepObject.Value as RouteStepWrapper).step.points;
            }
            else
            {
                Debug.Log("Route Object, Points Object or Step Object must be not null.");
                return;
            }

            if (pointIndex.IsNone)
            {
                Debug.Log("Point index cannot be null.");
                return;
            }

            int i = pointIndex.Value;
            if (i < 0 || i >= points.Count)
            {
                Debug.Log("Point index out of range.");
                return;
            }

            storePoint.Value = points[i];
        }
    }
}