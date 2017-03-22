using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GENERAL)]
    [Tooltip("Lock map coordinates range.")]
    public class SetCoordinatesRange : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Min latitude.")]
        [HasFloatSlider(90, -90)]
        public FsmFloat minLatitude = -90;

        [RequiredField]
        [Tooltip("Max latitude.")]
        [HasFloatSlider(90, -90)]
        public FsmFloat maxLatitude = 90;

        [RequiredField]
        [Tooltip("Min longitude.")]
        [HasFloatSlider(-180, 180)]
        public FsmFloat minLongitude = -180;

        [RequiredField]
        [Tooltip("Max longitude.")]
        [HasFloatSlider(-180, 180)]
        public FsmFloat maxLongitude = 180;

        [Tooltip("Type of range locking.")]
        public OnlineMapsPositionRangeType lockType = OnlineMapsPositionRangeType.center;

        public override void Reset()
        {
            minLatitude = -90;
            maxLatitude = 90;
            minLongitude = -180;
            maxLongitude = 180;
            lockType = OnlineMapsPositionRangeType.center;
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

            OnlineMaps.instance.positionRange = new OnlineMapsPositionRange(minLatitude.Value, minLongitude.Value, maxLatitude.Value, maxLongitude.Value, lockType);
        }
    }
}