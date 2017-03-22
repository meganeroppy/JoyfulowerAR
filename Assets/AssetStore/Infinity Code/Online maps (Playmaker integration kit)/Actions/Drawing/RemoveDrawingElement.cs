using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.DRAWING)]
    [Tooltip("Remove drawing element from map.")]
    public class RemoveDrawingElement : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Drawing element instance.")]
        [UIHint(UIHint.Variable)]
        public FsmObject drawingElementVariable;

        public override void Reset()
        {
            drawingElementVariable = null;
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

            if (drawingElementVariable.Value.GetType() != typeof(DrawingElementWrapper))
            {
                Debug.LogError("Its not DrawingElement instance");
                return;
            }
            DrawingElementWrapper wrapper = drawingElementVariable.Value as DrawingElementWrapper;
            OnlineMaps.instance.RemoveDrawingElement(wrapper.drawingElement);
        }
    }
}