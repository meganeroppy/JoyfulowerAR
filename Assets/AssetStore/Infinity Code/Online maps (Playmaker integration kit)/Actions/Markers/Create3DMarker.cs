using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [RequireComponent(typeof(OnlineMapsControlBase3D))]
    [Tooltip("Create a new 3D marker on the map.")]
    public class Create3DMarker : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Marker coordinates. X - Longituge. Y - Latitude.")]
        public FsmVector2 coordinates = Vector2.zero;

        [RequiredField]
        [Tooltip("Marker prefab.")]
        public FsmGameObject prefab;

        [Tooltip("Marker tooltip.")]
        public FsmString tooltip;

        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of marker.")]
        public FsmObject storeInstance;

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
            coordinates = null;
            prefab = null;
            tooltip = null;
            storeInstance = null;
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

            OnlineMapsControlBase3D control = OnlineMapsControlBase3D.instance as OnlineMapsControlBase3D;
            OnlineMapsMarker3D marker = control.AddMarker3D(coordinates.Value, prefab.Value);
            if (!tooltip.IsNone) marker.label = tooltip.Value;
            MarkerWrapper markerWrapper = new MarkerWrapper(marker);
            storeInstance.Value = markerWrapper;

            if (OnClick != null)
            {
                marker.OnClick += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnClick);
                };
            }
            if (OnDoubleClick != null)
            {
                marker.OnDoubleClick += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnDoubleClick);
                };
            }
            if (OnPress != null)
            {
                marker.OnPress += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnPress);
                };
            }
            if (OnRelease != null)
            {
                marker.OnRelease += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnRelease);
                };
            }
            if (OnRollOver != null)
            {
                marker.OnRollOver += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnRollOver);
                };
            }
            if (OnRollOut != null)
            {
                marker.OnRollOut += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnRollOut);
                };
            }
            if (OnPositionChanged != null)
            {
                marker.OnPositionChanged += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnPositionChanged);
                };
            }
            if (OnLongPress != null)
            {
                marker.OnLongPress += delegate
                {
                    MarkerWrapper.activeMarker = markerWrapper;
                    Fsm.Event(OnLongPress);
                };
            }
        }
    }
}