using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Gets 3D marker instance GameObject.")]
    public class Get3DMarkerInstance : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of 3D marker.")]
        public FsmObject marker;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Marker 3D instance.")]
        public FsmGameObject storeInstance;

        public override void Reset()
        {
            marker = null;
            storeInstance = null;
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

            OnlineMapsMarker3D m = (marker.Value as MarkerWrapper).marker as OnlineMapsMarker3D;
            storeInstance.Value = m.instance;
        }
    }
}