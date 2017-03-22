using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.LOCATION)]
    [Tooltip("Handle Location Service events.")]
    public class HandleLocationEvent : FsmStateAction
    {
        [Tooltip("On compass true heading changed event.")]
        public FsmEvent OnCompassChanged;

        [Tooltip("On GPS location changed event.")]
        public FsmEvent OnLocationChanged;

        [Tooltip("On GPS location inited event.")]
        public FsmEvent OnLocationInited;

        public override void Reset()
        {
            OnCompassChanged = null;
            OnLocationChanged = null;
            OnLocationInited = null;
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

            if (OnCompassChanged != null)
            {
                OnlineMapsLocationService.instance.OnCompassChanged += delegate {
                    Fsm.Event(OnCompassChanged);
                };
            }
            if (OnLocationChanged != null)
            {
                OnlineMapsLocationService.instance.OnLocationChanged += delegate {
                    Fsm.Event(OnLocationChanged);
                };
            }
            if (OnLocationInited != null)
            {
                OnlineMapsLocationService.instance.OnLocationInited += delegate {
                    Fsm.Event(OnLocationInited);
                };
            }
        }
    }
}