using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Gets marker rotation.")]
    public class Get2DMarkerRotation : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of 2D marker.")]
        public FsmObject marker;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Rotation of marker.")]
        public FsmFloat storeRotation;

        public override void Reset()
        {
            marker = null;
            storeRotation = null;
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
            storeRotation.Value = m.rotation;
        }
    }
}