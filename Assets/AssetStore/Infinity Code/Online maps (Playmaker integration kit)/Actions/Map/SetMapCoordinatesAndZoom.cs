using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GENERAL)]
    [Tooltip("Sets coordinates of the center point of the map and zoom.")]
    public class SetMapCoordinatesAndZoom : FsmStateAction
    {
        [Tooltip("Coordinates of center point of map.")]
        public FsmVector2 coordinates;

        [Tooltip("Map zoom.")]
        public FsmInt zoom;

        public override void Reset()
        {
            coordinates = null;
            zoom = null;
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

            if (!coordinates.IsNone) OnlineMaps.instance.position = coordinates.Value;
            if (!zoom.IsNone) OnlineMaps.instance.zoom = zoom.Value;
        }
    }
}