using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GENERAL)]
    [Tooltip("Lock map zoom range.")]
    public class SetZoomRange : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Max zoom.")]
        public FsmInt minZoom = 3;

        [RequiredField]
        [Tooltip("Max max.")]
        public FsmInt maxZoom = 20;

        public override void Reset()
        {
            minZoom = 3;
            maxZoom = 20;
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

            OnlineMaps.instance.zoomRange = new OnlineMapsRange(minZoom.Value, maxZoom.Value);
        }
    }
}