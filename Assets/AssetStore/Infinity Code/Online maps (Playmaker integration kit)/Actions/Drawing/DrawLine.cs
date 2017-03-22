using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.DRAWING)]
    [Tooltip("Draw line on map.")]
    public class DrawLine : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Coordinates of points.")]
        public FsmVector2[] coordinates;

        [Tooltip("Line color.")]
        public FsmColor color = Color.black;

        [Tooltip("Line weight.")]
        public FsmInt weight = 1;

        [Tooltip("Drawing element instance.")]
        [UIHint(UIHint.Variable)]
        public FsmObject storeInstance;

        public override void Reset()
        {
            coordinates = null;
            color = Color.black;
            weight = 1;
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

            OnlineMapsDrawingLine line = new OnlineMapsDrawingLine(coords);
            if (!color.IsNone) line.color = color.Value;
            if (!weight.IsNone) line.weight = weight.Value;

            OnlineMaps.instance.AddDrawingElement(line);

            if (!storeInstance.IsNone) storeInstance.Value = new DrawingElementWrapper(line);
        }
    }
}