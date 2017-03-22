using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GENERAL)]
    [Tooltip("Get screen area occupied by the map.")]
    public class GetScreenRect : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Screen area occupied by the map.")]
        [UIHint(UIHint.Variable)]
        public FsmRect storeScreenRect;

        public override void Reset()
        {
            storeScreenRect = null;
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

            storeScreenRect.Value = OnlineMapsControlBase.instance.screenRect;
        }
    }
}