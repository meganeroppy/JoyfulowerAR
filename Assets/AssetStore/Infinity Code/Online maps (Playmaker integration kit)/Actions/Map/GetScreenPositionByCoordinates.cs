using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GENERAL)]
    [Tooltip("Get position in screen space by coordinates.")]
    public class GetScreenPositionByCoordinates : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Coordinates.")]
        public FsmVector2 coordinates;

        [RequiredField]
        [Tooltip("Position in screen space.")]
        [UIHint(UIHint.Variable)]
        public FsmVector2 storePosition;

        public override void Reset()
        {
            coordinates = null;
            storePosition = null;
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

            storePosition.Value = OnlineMapsControlBase.instance.GetScreenPosition(coordinates.Value);
        }
    }
}