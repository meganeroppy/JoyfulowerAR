using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Sets marker enabled.")]
    public class SetMarkerEnabled : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of 2D marker.")]
        public FsmObject marker;

        [RequiredField]
        [Tooltip("New enabled state.")]
        public FsmBool newState;

        public override void Reset()
        {
            marker = null;
            newState = null;
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

            if (marker.IsNone) return;

            if (marker.Value.GetType() != typeof(MarkerWrapper))
            {
                Debug.Log("Its not MarkerWrapper instance");
                return;
            }

            OnlineMapsMarker m = (marker.Value as MarkerWrapper).marker as OnlineMapsMarker;
            if (m == null)
            {
                Debug.Log("Wrong marker instance.");
                return;
            }
            m.enabled = newState.Value;
        }
    }
}