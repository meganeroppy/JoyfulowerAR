using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.UTILS)]
    [Tooltip("Converts geographic coordinates to Mercator coordinates.")]
    public class LatLongToMercator : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Coordinate (X - Lng, Y - Lat)")]
        public FsmVector2 coordinate;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Mercator coordinates")]
        public FsmVector2 mercatorCoordinates;

        public override void Reset()
        {
            coordinate = null;
            mercatorCoordinates = null;
        }

        public override void OnEnter()
        {
            mercatorCoordinates.Value = OnlineMapsUtils.LatLongToMercat(coordinate.Value.x, coordinate.Value.y);
            Finish();
        }
    }
}