using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Gets 2D marker by index.")]
    public class Get2DMarkerByIndex : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Index of 2D marker.")]
        public FsmInt index;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of 2D marker.")]
        public FsmObject storeInstance;

        public override void Reset()
        {
            index = 0;
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

            int value = index.Value;
            if (value < 0 || value >= OnlineMaps.instance.markers.Length)
            {
                Debug.LogError("Marker index out of range.");
                return;
            }

            storeInstance.Value = new MarkerWrapper(OnlineMaps.instance.markers[value]);
        }
    }
}