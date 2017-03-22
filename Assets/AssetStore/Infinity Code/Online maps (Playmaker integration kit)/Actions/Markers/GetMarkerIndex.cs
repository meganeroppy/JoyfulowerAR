using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Get the index of marker")]
    public class GetMarkerIndex : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of marker.")]
        public FsmObject marker;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Index of marker.")]
        public FsmInt storeIndex;

        public override void Reset()
        {
            marker = null;
            storeIndex = null;
        }

        public override void OnEnter()
        {
            DoEnter();
            Finish();
        }

        private void DoEnter()
        {
            OnlineMaps map = OnlineMaps.instance;
            if (map == null)
            {
                Debug.LogError("Online Maps not found.");
                return;
            }

            if (marker.IsNone || storeIndex.IsNone) return;

            if (marker.Value.GetType() != typeof(MarkerWrapper))
            {
                Debug.Log("Its not MarkerWrapper instance");
                return;
            }

            OnlineMapsMarkerBase m = (marker.Value as MarkerWrapper).marker;
            if (m is OnlineMapsMarker)
            {
                for (int i = 0; i < map.markers.Length; i++)
                {
                    if (map.markers[i] == m)
                    {
                        storeIndex.Value = i;
                        break;
                    }
                }
            }
            else if (m is OnlineMapsMarker3D)
            {
                for (int i = 0; i < OnlineMapsControlBase3D.instance.markers3D.Length; i++)
                {
                    if (OnlineMapsControlBase3D.instance.markers3D[i] == m)
                    {
                        storeIndex.Value = i;
                        break;
                    }
                }
            }
        }
    }
}