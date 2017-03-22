using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GENERAL)]
    [Tooltip("Get coordinates by screen position.")]
    public class GetCoordinatesByScreenPosition : FsmStateAction
    {
        [UIHint(UIHint.Description)] 
        public string hint = "If the position is not specified, will be used Input.mousePosition.";

        [Tooltip("Position in screen space.")]
        public FsmVector2 position;

        [RequiredField]
        [Tooltip("Coordinates.")]
        [UIHint(UIHint.Variable)]
        public FsmVector2 storeCoordinates;

        public override void Reset()
        {
            storeCoordinates = null;
            position = new FsmVector2 { UseVariable = true };
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

            Vector2 screenPosition = (!position.IsNone) ? position.Value : (Vector2)Input.mousePosition;
            storeCoordinates.Value = OnlineMapsControlBase.instance.GetCoords(screenPosition);
        }
    }
}