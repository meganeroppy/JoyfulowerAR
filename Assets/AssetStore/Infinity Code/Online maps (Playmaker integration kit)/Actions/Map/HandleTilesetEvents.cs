using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GENERAL)]
    [Tooltip("Handle tileset events.")]
    public class HandleTilesetEvents : FsmStateAction
    {
        [Tooltip("Tileset OnCameraControl event.")]
        public FsmEvent OnCameraControl;

        [Tooltip("Tileset OnSmoothZoomBegin event.")]
        public FsmEvent OnSmoothZoomBegin;

        [Tooltip("Tileset OnSmoothZoomProcess event.")]
        public FsmEvent OnSmoothZoomProcess;

        [Tooltip("Tileset OnSmoothZoomFinish event.")]
        public FsmEvent OnSmoothZoomFinish;

        public override void Reset()
        {
            OnCameraControl = null;
            OnSmoothZoomBegin = null;
            OnSmoothZoomProcess = null;
            OnSmoothZoomFinish = null;
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

            OnlineMapsTileSetControl control = OnlineMapsTileSetControl.instance;
            if (control == null)
            {
                Debug.Log("Need tileset control.");
                return;
            }

            if (OnCameraControl != null) control.OnCameraControl += delegate { Fsm.Event(OnCameraControl); };
            if (OnSmoothZoomBegin != null) control.OnSmoothZoomBegin += delegate { Fsm.Event(OnSmoothZoomBegin); };
            if (OnSmoothZoomProcess != null) control.OnSmoothZoomProcess += delegate { Fsm.Event(OnSmoothZoomProcess); };
            if (OnSmoothZoomFinish != null) control.OnSmoothZoomFinish += delegate { Fsm.Event(OnSmoothZoomFinish); };
        }
    }
}