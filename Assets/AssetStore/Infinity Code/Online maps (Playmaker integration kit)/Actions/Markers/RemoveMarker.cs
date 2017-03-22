using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Remove 2D or 3D marker from map.")]
    public class RemoveMarker : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of 2D or 3D marker.")]
        public FsmObject marker;

        public override void Reset()
        {
            marker = null;
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

            if (m is OnlineMapsMarker) OnlineMaps.instance.RemoveMarker(m as OnlineMapsMarker);
            else if (m is OnlineMapsMarker3D)
            {
                OnlineMapsControlBase control = OnlineMapsControlBase3D.instance;
                if (control is OnlineMapsControlBase3D) (control as OnlineMapsControlBase3D).RemoveMarker3D(m as OnlineMapsMarker3D);
            }
        }
    }
}