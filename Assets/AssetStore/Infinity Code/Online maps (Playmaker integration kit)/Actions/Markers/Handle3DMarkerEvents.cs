using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Handle 3D Marker events.")]
    public class Handle3DMarkerEvents : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of marker.")]
        public FsmObject marker;

        [Tooltip("Marker OnClick event.")]
        public FsmEvent OnClick;

        [Tooltip("Marker OnDoubleClick event.")]
        public FsmEvent OnDoubleClick;

        [Tooltip("Marker OnLongPress event.")]
        public FsmEvent OnLongPress;

        [Tooltip("Marker OnPress event.")]
        public FsmEvent OnPress;

        [Tooltip("Marker OnRelease event.")]
        public FsmEvent OnRelease;

        [Tooltip("Marker OnRollOver event.")]
        public FsmEvent OnRollOver;

        [Tooltip("Marker OnRollOut event.")]
        public FsmEvent OnRollOut;

        [Tooltip("Marker OnPositionChanged event.")]
        public FsmEvent OnPositionChanged;

        public override void Reset()
        {
            marker = null;
            OnClick = null;
            OnDoubleClick = null;
            OnLongPress = null;
            OnPress = null;
            OnRelease = null;
            OnRollOver = null;
            OnRollOut = null;
            OnPositionChanged = null;
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

            if (marker.IsNone) return;

            if (marker.Value.GetType() != typeof(MarkerWrapper))
            {
                Debug.Log("Its not MarkerWrapper instance");
                return;
            }

            MarkerWrapper markerWrapper = marker.Value as MarkerWrapper;
            OnlineMapsMarker3D m = markerWrapper.marker as OnlineMapsMarker3D;

            if (m == null) return;

            if (OnClick != null)
            {
                m.OnClick += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnClick);
                };
            }
            if (OnDoubleClick != null)
            {
                m.OnDoubleClick += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnDoubleClick);
                };
            }
            if (OnPress != null)
            {
                m.OnPress += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnPress);
                };
            }
            if (OnRelease != null)
            {
                m.OnRelease += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnRelease);
                };
            }
            if (OnRollOver != null)
            {
                m.OnRollOver += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnRollOver);
                };
            }
            if (OnRollOut != null)
            {
                m.OnRollOut += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnRollOut);
                };
            }
            if (OnPositionChanged != null)
            {
                m.OnPositionChanged += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnPositionChanged);
                };
            }
            if (OnLongPress != null)
            {
                m.OnLongPress += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnLongPress);
                };
            }
        }
    }
}