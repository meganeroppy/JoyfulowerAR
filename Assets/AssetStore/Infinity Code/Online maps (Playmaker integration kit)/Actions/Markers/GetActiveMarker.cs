using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Get the marker, the event which causes last.")]
    public class GetActiveMarker : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of marker.")]
        public FsmObject storeInstance;

        public override void Reset()
        {
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

            storeInstance.Value = MarkerWrapper.activeMarker;
        }
    }
}