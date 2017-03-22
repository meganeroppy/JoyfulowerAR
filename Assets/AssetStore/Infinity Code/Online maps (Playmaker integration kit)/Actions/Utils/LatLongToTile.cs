using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.UTILS)]
    [Tooltip("Converts geographic coordinates to tile coordinates.")]
    public class LatLongToTile : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Coordinate (X - Lng, Y - Lat)")]
        public FsmVector2 coordinate;

        [RequiredField]
        [Tooltip("Zoom")]
        public FsmInt zoom;

        [UIHint(UIHint.Variable)]
        [Tooltip("Tile coordinates")]
        public FsmVector2 storeTilePosition;

        [UIHint(UIHint.Variable)]
        [Tooltip("Tile X")]
        public FsmInt storeTileX;

        [UIHint(UIHint.Variable)]
        [Tooltip("Tile Y")]
        public FsmInt storeTileY;

        public override void Reset()
        {
            coordinate = null;
            zoom = null;
            storeTilePosition = null;
            storeTileX = null;
            storeTileY = null;
        }

        public override void OnEnter()
        {
            double px, py;
            OnlineMaps.instance.projection.CoordinatesToTile(coordinate.Value.x, coordinate.Value.y, zoom.Value, out px, out py);
            storeTilePosition.Value = new Vector2((float)px, (float)py);
            storeTileX.Value = (int) storeTilePosition.Value.x;
            storeTileY.Value = (int) storeTilePosition.Value.y;
            Finish();
        }
    }
}