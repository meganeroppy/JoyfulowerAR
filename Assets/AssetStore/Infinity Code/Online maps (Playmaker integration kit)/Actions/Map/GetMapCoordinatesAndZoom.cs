using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GENERAL)]
    [Tooltip("Gets coordinates of the center point of the map and zoom.")]
    public class GetMapCoordinatesAndZoom : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [Tooltip("Coordinates of center point of map.")]
        public FsmVector2 storeCoordinates;

        [Tooltip("Map zoom.")]
        [UIHint(UIHint.Variable)]
        public FsmInt storeZoom;

        public override void Reset()
        {
            storeCoordinates = null;
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

            if (!storeCoordinates.IsNone) storeCoordinates.Value = OnlineMaps.instance.position;
            if (!storeZoom.IsNone) storeZoom.Value = OnlineMaps.instance.zoom;
        }
    }
}