using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Create a new 2D marker on the map.")]
    public class Create2DMarker : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Marker coordinates. X - Longituge. Y - Latitude.")]
        public FsmVector2 coordinates = Vector2.zero;

        [Tooltip("Marker texture.")]
        public FsmTexture texture;

        [Tooltip("Marker tooltip.")]
        public FsmString tooltip;

        [Tooltip("Marker align.")]
        public OnlineMapsAlign align = OnlineMapsAlign.Bottom;

        [UIHint(UIHint.Variable)]
        [Tooltip("Instance of marker.")]
        public FsmObject storeInstance;

        [Tooltip("Marker OnClick event.")]
        public FsmEvent OnClick;

        [Tooltip("Marker OnDoubleClick event.")]
        public FsmEvent OnDoubleClick;

        [Tooltip("Marker OnPress event.")]
        public FsmEvent OnPress;

        [Tooltip("Marker OnRelease event.")]
        public FsmEvent OnRelease;

        [Tooltip("Marker OnRollOver event.")]
        public FsmEvent OnRollOver;

        [Tooltip("Marker OnRollOut event.")]
        public FsmEvent OnRollOut;

        public override void Reset()
        {
            coordinates = Vector2.zero;
            texture = new FsmTexture { UseVariable = true };
            tooltip = new FsmString { UseVariable = true };
            align = OnlineMapsAlign.Bottom;

            storeInstance = null;
            OnClick = null;
            OnDoubleClick = null;
            OnPress = null;
            OnRelease = null;
            OnRollOver = null;
            OnRollOut = null;
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

            Texture2D markerTexture = texture.IsNone ? null : texture.Value as Texture2D;
            string markerTooltip = tooltip.IsNone ? "" : tooltip.Value;
            OnlineMapsMarker marker = OnlineMaps.instance.AddMarker(coordinates.Value, markerTexture, markerTooltip);
            marker.align = align;
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
        }
    }
}