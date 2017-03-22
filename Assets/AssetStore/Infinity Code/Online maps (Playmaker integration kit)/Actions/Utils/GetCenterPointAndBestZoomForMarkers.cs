using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.UTILS)]
    [Tooltip("Get Center Point And Best Zoom between Markers.")]
    public class GetCenterPointAndBestZoomForMarkers : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Markers")]
        public FsmObject[] markers;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Center point coordinate (X - Lng, Y - Lat)")]
        public FsmVector2 storeCenterPoint;

        [UIHint(UIHint.Variable)]
        [Tooltip("Best zoom")]
        public FsmInt storeZoom;

        public override void Reset()
        {
            markers = null;
            storeCenterPoint = null;
            storeZoom = null;
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

            if (markers == null) return;

            List<OnlineMapsMarkerBase> ms = new List<OnlineMapsMarkerBase>();

            foreach (FsmObject marker in markers)
            {
                MarkerWrapper m = marker.Value as MarkerWrapper;
                if (m == null) continue;
                ms.Add(m.marker);
            }

            if (ms.Count == 0) return;

            Vector2 center;
            int _zoom;
            OnlineMapsUtils.GetCenterPointAndZoom(ms.ToArray(), out center, out _zoom);

            storeCenterPoint.Value = center;
            storeZoom.Value = _zoom;
        }
    }
}