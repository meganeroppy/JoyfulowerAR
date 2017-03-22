using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Get the marker from Online Maps Location Service.")]
    public class GetLocationServiceMarker : FsmStateAction
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

            if (OnlineMapsLocationService.instance == null)
            {
                Debug.LogError("Online Maps Location Service.");
                return;
            }

            storeInstance.Value = new MarkerWrapper(OnlineMapsLocationService.marker);
        }
    }
}