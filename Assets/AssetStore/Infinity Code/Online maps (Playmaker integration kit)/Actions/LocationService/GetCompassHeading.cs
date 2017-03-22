using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.LOCATION)]
    [Tooltip("Gets compass true heading.")]
    public class GetCompassHeading : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("Compass true heading.")]
        public FsmFloat storeCompassHeading;

        public override void Reset()
        {
            storeCompassHeading = null;
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

            storeCompassHeading.Value = OnlineMapsLocationService.instance.trueHeading;
        }
    }
}