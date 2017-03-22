using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Gets 3D marker by index.")]
    public class Get3DMarkerByIndex : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Index of 3D marker.")]
        public FsmInt index;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of 3D marker.")]
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
            if (value < 0 || value >= OnlineMapsControlBase3D.instance.markers3D.Length)
            {
                Debug.LogError("Marker index out of range.");
                return;
            }

            storeInstance.Value = new MarkerWrapper(OnlineMapsControlBase3D.instance.markers3D[value]);
        }
    }
}