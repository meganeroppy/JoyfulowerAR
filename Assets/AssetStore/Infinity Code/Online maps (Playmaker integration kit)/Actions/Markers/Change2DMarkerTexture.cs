using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Change texture in 2D marker.")]
    public class Change2DMarkerTexture : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Instance of merker.")]
        public FsmObject markerInstance;

        [RequiredField]
        [Tooltip("New marker texture. In import settings must be enabled 'Read / Write enabled'. ")]
        public FsmTexture texture;

        public override void Reset()
        {
            markerInstance = null;
            texture = null;
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

            if (markerInstance.IsNone) return;

            if (markerInstance.Value.GetType() != typeof(MarkerWrapper))
            {
                Debug.Log("Its not MarkerWrapper instance");
                return;
            }

            MarkerWrapper wrapper = markerInstance.Value as MarkerWrapper;
            OnlineMapsMarker marker = wrapper.marker as OnlineMapsMarker;
            if (marker == null) return;
            marker.texture = texture.Value as Texture2D;
            marker.Init();
            OnlineMaps.instance.Redraw();
        }
    }
}