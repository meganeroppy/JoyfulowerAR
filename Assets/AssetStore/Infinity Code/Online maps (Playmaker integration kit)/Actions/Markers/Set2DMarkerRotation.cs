using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Sets 2D marker rotation.")]
    public class Set2DMarkerRotation : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of 2D marker.")]
        public FsmObject marker;

        [RequiredField]
        [Tooltip("Rotation of marker.")]
        public FsmFloat rotation;

        public override void Reset()
        {
            marker = null;
            rotation = null;
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
            m.rotation = rotation.Value;
        }
    }
}