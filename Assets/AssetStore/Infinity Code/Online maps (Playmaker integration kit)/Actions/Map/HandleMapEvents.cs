using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GENERAL)]
    [Tooltip("Handle map events.")]
    public class HandleMapEvents : FsmStateAction
    {
        [Tooltip("Map OnChangePosition event.")]
        public FsmEvent OnChangePosition;

        [Tooltip("Map OnChangeZoom event.")]
        public FsmEvent OnChangeZoom;

        [Tooltip("Map OnUpdateBefore event.")]
        public FsmEvent OnUpdateBefore;

        [Tooltip("Map OnUpdateLate event.")]
        public FsmEvent OnUpdateLate;

        [Tooltip("Map OnClick event.")]
        public FsmEvent OnClick;

        [Tooltip("Map OnDoubleClick event.")]
        public FsmEvent OnDoubleClick;

        [Tooltip("Map OnDrag event.")]
        public FsmEvent OnDrag;

        [Tooltip("Map OnLongPress event.")]
        public FsmEvent OnLongPress;

        [Tooltip("Map OnPress event.")]
        public FsmEvent OnPress;

        [Tooltip("Map OnRelease event.")]
        public FsmEvent OnRelease;

        public override void Reset()
        {
            OnChangePosition = null;
            OnChangeZoom = null;
            OnUpdateBefore = null;
            OnUpdateLate = null;
            OnClick = null;
            OnDoubleClick = null;
            OnDrag = null;
#if !UNITY_4_3
            OnLongPress = null;
#endif
            OnPress = null;
            OnRelease = null;
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

            OnlineMaps api = OnlineMaps.instance;
            OnlineMapsControlBase control = OnlineMapsControlBase.instance;
            if (OnChangePosition != null) api.OnChangePosition += delegate { Fsm.Event(OnChangePosition); };
            if (OnChangeZoom != null) api.OnChangeZoom += delegate { Fsm.Event(OnChangeZoom); };
            if (OnUpdateBefore != null) api.OnUpdateBefore += delegate { Fsm.Event(OnUpdateBefore); };
            if (OnUpdateLate != null) api.OnUpdateLate += delegate { Fsm.Event(OnUpdateLate); };

            if (OnClick != null) control.OnMapClick += delegate { Fsm.Event(OnClick); };
            if (OnDoubleClick != null) control.OnMapDoubleClick += delegate { Fsm.Event(OnDoubleClick); };
            if (OnDrag != null) control.OnMapDrag += delegate { Fsm.Event(OnDrag); };
            if (OnLongPress != null) control.OnMapLongPress += delegate { Fsm.Event(OnLongPress); };
            if (OnPress != null) control.OnMapPress += delegate { Fsm.Event(OnPress); };
            if (OnRelease != null) control.OnMapRelease += delegate { Fsm.Event(OnRelease); };
        }
    }
}