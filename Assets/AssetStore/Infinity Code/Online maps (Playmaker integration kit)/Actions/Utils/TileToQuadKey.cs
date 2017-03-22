using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.UTILS)]
    [Tooltip("Converts tile index to quadkey.")]
    public class TileToQuadKey : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Zoom")]
        public FsmInt zoom;

        [RequiredField]
        [Tooltip("Tile X")]
        public FsmInt tileX;

        [RequiredField]
        [Tooltip("Tile Y")]
        public FsmInt tileY;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Quadkey")]
        public FsmString storeQuadkey;

        public override void Reset()
        {
            zoom = null;
            tileX = null;
            tileY = null;
            storeQuadkey = null;
        }

        public override void OnEnter()
        {
            storeQuadkey.Value = OnlineMapsUtils.TileToQuadKey(tileX.Value, tileY.Value, zoom.Value);
            Finish();
        }
    }
}