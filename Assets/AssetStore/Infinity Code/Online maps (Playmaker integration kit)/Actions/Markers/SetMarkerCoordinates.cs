using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Sets coordinates of marker.")]
    public class SetMarkerCoordinates : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of marker.")]
        public FsmObject marker;

        [RequiredField]
        [Tooltip("Coordinates of marker.")]
        public FsmVector2 coordinates;

        public override void Reset()
        {
            marker = null;
            coordinates = null;
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
            m.position = coordinates.Value;
            OnlineMaps.instance.Redraw();
        }
    }
}