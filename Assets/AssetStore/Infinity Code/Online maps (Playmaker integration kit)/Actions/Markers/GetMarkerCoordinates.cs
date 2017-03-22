using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Gets coordinates of marker")]
    public class GetMarkerCoordinates : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of marker.")]
        public FsmObject marker;

        [UIHint(UIHint.Variable)]
        [Tooltip("Coordinates of marker.")]
        public FsmVector2 storeCoordinates;

        public override void Reset()
        {
            marker = null;
            storeCoordinates = null;
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
            storeCoordinates.Value = m.position;
        }
    }
}