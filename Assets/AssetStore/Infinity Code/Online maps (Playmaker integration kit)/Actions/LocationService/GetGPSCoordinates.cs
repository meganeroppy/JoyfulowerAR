using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.LOCATION)]
    [Tooltip("Gets GPS coordinates.")]
    public class GetGPSCoordinates : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("GPS coordinates.")]
        public FsmVector2 storeCoordinates;

        public override void Reset()
        {
            storeCoordinates = null;
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

            if (OnlineMapsLocationService.instance == null)
            {
                Debug.LogError("Online Maps Location Service not found.");
                return;
            }

            storeCoordinates.Value = OnlineMapsLocationService.instance.position;
        }
    }
}