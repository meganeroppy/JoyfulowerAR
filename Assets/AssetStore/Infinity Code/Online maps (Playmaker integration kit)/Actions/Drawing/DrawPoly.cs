using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.DRAWING)]
    [Tooltip("Draw polygon on map.")]
    public class DrawPoly : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Coordinates of points.")]
        public FsmVector2[] coordinates;

        [Tooltip("Border color.")]
        public FsmColor borderColor = Color.black;

        [Tooltip("Border weight.")]
        public FsmInt borderWeight = 1;

        [Tooltip("Background color.")]
        public FsmColor backgroundColor = new Color(1, 1, 1, 0);

        [Tooltip("Drawing element instance.")]
        [UIHint(UIHint.Variable)]
        public FsmObject storeInstance;

        public override void Reset()
        {
            coordinates = null;
            borderColor = Color.black;
            borderWeight = 1;
            backgroundColor = new Color(1, 1, 1, 0);
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

            if (coordinates == null)
            {
                Debug.LogError("Coordinates is null.");
                return;
            }

            List<Vector2> coords = new List<Vector2>();
            foreach (FsmVector2 c in coordinates)
            {
                if (c.IsNone) continue;
                coords.Add(c.Value);
            }

            if (coords.Count < 2)
            {
                Debug.LogWarning("Coordinates have at least two values.");
                return;
            }

            OnlineMapsDrawingPoly poly = new OnlineMapsDrawingPoly(coords);
            if (!borderColor.IsNone) poly.backgroundColor = borderColor.Value;
            if (!borderWeight.IsNone) poly.borderWeight = borderWeight.Value;
            if (!backgroundColor.IsNone) poly.backgroundColor = backgroundColor.Value;

            OnlineMaps.instance.AddDrawingElement(poly);

            if (!storeInstance.IsNone) storeInstance.Value = new DrawingElementWrapper(poly);
        }
    }
}