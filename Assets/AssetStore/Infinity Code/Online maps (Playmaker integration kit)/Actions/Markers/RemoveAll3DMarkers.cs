using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [RequireComponent(typeof(OnlineMapsControlBase3D))]
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Remove all 3D markers from map.")]
    public class RemoveAll3DMarkers : FsmStateAction
    {
        public override void Reset()
        {
            
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

            OnlineMapsControlBase3D control = OnlineMapsControlBase3D.instance;
            if (control != null)
            {
                foreach (OnlineMapsMarker3D marker in control.markers3D) control.RemoveMarker3D(marker);
            }
        }
    }
}