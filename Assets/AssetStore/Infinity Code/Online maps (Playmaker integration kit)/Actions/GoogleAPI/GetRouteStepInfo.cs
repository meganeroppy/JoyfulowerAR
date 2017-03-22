using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Gets route step info.")]
    public class GetRouteStepInfo : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Step object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject stepObject;

        [Tooltip("The total distance covered by this step.")]
        [UIHint(UIHint.Variable)]
        public FsmInt storeDistance;

        [Tooltip("The total duration of the passage of this step.")]
        [UIHint(UIHint.Variable)]
        public FsmInt storeDuration;

        [Tooltip("Location of the endpoint of this step (lng, lat).")]
        [UIHint(UIHint.Variable)]
        public FsmVector2 storeEnd;

        [Tooltip("Instructions to the current step.")]
        [UIHint(UIHint.Variable)]
        public FsmString storeInstructions;

        [Tooltip("Maneuver the current step.")]
        [UIHint(UIHint.Variable)]
        public FsmString storeManeuver;

        [Tooltip("Location of the startpoint of this step (lng, lat).")]
        [UIHint(UIHint.Variable)]
        public FsmVector2 storeStart;

        [Tooltip("Count points in step.")]
        [UIHint(UIHint.Variable)]
        public FsmInt storePointsCount;

        public override void Reset()
        {
            stepObject = null;
            storeDistance = null;
            storeDuration = null;
            storeEnd = null;
            storeInstructions = null;
            storeManeuver = null;
            storeStart = null;
            storePointsCount = null;
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

            storeDistance.Value = wrapper.step.distance;
            storeDuration.Value = wrapper.step.duration;
            storeEnd.Value = wrapper.step.end;
            storeInstructions.Value = wrapper.step.instructions;
            storeManeuver.Value = wrapper.step.maneuver;
            storeStart.Value = wrapper.step.start;
            storePointsCount.Value = wrapper.step.points.Count;
        }
    }
}