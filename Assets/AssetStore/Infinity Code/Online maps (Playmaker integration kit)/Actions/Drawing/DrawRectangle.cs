using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.DRAWING)]
    [Tooltip("Draw rectangle on map.")]
    public class DrawRectangle : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Longitude of top-left angle.")]
        public FsmFloat x = 0;

        [RequiredField]
        [Tooltip("Latitude of top-left angle.")]
        public FsmFloat y = 0;

        [RequiredField]
        [Tooltip("Width in arc angles.")]
        public FsmFloat width = 1;

        [RequiredField]
        [Tooltip("Height in arc angles.")]
        public FsmFloat height = 1;

        [Tooltip("Border color.")]
        public FsmColor borderColor = Color.black;

        [Tooltip("Border weight.")]
        public FsmInt borderWeight = 1;

        [Tooltip("Background color.")]
        public FsmColor backgroundColor = new Color(1, 1, 1, 0);

        [Tooltip("Drawing element instance.")]
        [UIHint(UIHint.Variable)]
        public FsmObject storeInstance;

        public override void Reset()
        {
            x = 0;
            y = 0;
            width = 1;
            height = 1;
            borderColor = Color.black;
            borderWeight = 1;
            backgroundColor = new Color(1, 1, 1, 0);
            storeInstance = null;
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

            OnlineMapsDrawingRect rect = new OnlineMapsDrawingRect(x.Value, y.Value, width.Value, height.Value);
            if (!borderColor.IsNone) rect.backgroundColor = borderColor.Value;
            if (!borderWeight.IsNone) rect.borderWeight = borderWeight.Value;
            if (!backgroundColor.IsNone) rect.backgroundColor = backgroundColor.Value;

            OnlineMaps.instance.AddDrawingElement(rect);

            if (!storeInstance.IsNone) storeInstance.Value = new DrawingElementWrapper(rect);
        }
    }
}