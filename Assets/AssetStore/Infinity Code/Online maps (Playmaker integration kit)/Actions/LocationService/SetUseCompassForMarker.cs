using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.LOCATION)]
    [Tooltip("Set use compass for marker value.")]
    public class SetUseCompassForMarker : FsmStateAction
    {
        [Tooltip("Use Compass?")]
        public FsmBool useCompass;

        public override void Reset()
        {
            useCompass = null;
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

            if (useCompass.IsNone)
            {
                Debug.LogError("Use compass is none.");
                return;
            }

            OnlineMapsLocationService.instance.useCompassForMarker = useCompass.Value;
        }
    }
}