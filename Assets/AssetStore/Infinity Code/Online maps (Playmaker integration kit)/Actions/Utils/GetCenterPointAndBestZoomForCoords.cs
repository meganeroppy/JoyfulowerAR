using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.UTILS)]
    [Tooltip("Get Center Point And Best Zoom between Coords.")]
    public class GetCenterPointAndBestZoomForCoords : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Coordinates")]
        public FsmVector2[] coordinates;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Center point coordinate (X - Lng, Y - Lat)")]
        public FsmVector2 storeCenterPoint;

        [UIHint(UIHint.Variable)]
        [Tooltip("Best zoom")]
        public FsmInt storeZoom;

        public override void Reset()
        {
            coordinates = null;
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
            if (coordinates == null || coordinates.Length < 2) return;

            List<Vector2> coords = new List<Vector2>();
            foreach (FsmVector2 v in coordinates) if (!v.IsNone) coords.Add(v.Value);
            if (coords.Count < 2) return;

            Vector2 center;
            int _zoom;
            OnlineMapsUtils.GetCenterPointAndZoom(coords.ToArray(), out center, out _zoom);

            storeCenterPoint.Value = center;
            storeZoom.Value = _zoom;
        }
    }
}