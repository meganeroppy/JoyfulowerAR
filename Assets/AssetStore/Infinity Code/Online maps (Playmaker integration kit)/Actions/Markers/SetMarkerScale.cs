using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Sets marker scale.\nNote: It's works for 3D markers and Billboard markers in 3D controls, and Flat markers in tileset.")]
    public class SetMarkerScale : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of 2D marker.")]
        public FsmObject marker;

        [RequiredField]
        [Tooltip("Scale of marker.")]
        public FsmFloat scale;

        public override void Reset()
        {
            marker = null;
            scale = null;
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

            OnlineMapsMarkerBase m = (marker.Value as MarkerWrapper).marker;
            if (m == null)
            {
                Debug.Log("Wrong marker instance.");
                return;
            }
            m.scale = scale.Value;
        }
    }
}