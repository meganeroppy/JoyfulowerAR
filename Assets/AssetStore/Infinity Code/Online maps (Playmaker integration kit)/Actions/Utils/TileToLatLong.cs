using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.UTILS)]
    [Tooltip("Converts tile coordinates to geographic coordinates.")]
    public class TileToLatLong : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Zoom")]
        public FsmInt zoom;

        [Tooltip("Tile coordinates")]
        public FsmVector2 tile;

        [Tooltip("Tile X")]
        public FsmInt tileX;

        [Tooltip("Tile Y")]
        public FsmInt tileY;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Geographic coordinates (X - Lng, Y - Lat)")]
        public FsmVector2 storeCoordinates;

        public override void Reset()
        {
            zoom = null;
            tile = null;
            tileX = null;
            tileY = null;
            storeCoordinates = null;
        }

        public override void OnEnter()
        {
            double px, py;
            if (!tile.IsNone)
            {
                OnlineMaps.instance.projection.TileToCoordinates(tile.Value.x, tile.Value.y, zoom.Value, out px, out py);
                storeCoordinates.Value = new Vector2((float)px, (float)py);
            }
            else if (!tileX.IsNone && !tileY.IsNone)
            {
                OnlineMaps.instance.projection.TileToCoordinates(tileX.Value, tileY.Value, zoom.Value, out px, out py);
                storeCoordinates.Value = new Vector2((float)px, (float)py);
            }

            Finish();
        }
    }
}