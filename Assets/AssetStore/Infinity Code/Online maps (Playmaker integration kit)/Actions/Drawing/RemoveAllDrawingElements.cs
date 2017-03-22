using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.DRAWING)]
    [Tooltip("Remove all drawing elements from map.")]
    public class RemoveAllDrawingElements : FsmStateAction
    {
        public override void Reset()
        {
            
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

            while (OnlineMaps.instance.drawingElements.Count > 0)
            {
                OnlineMaps.instance.RemoveDrawingElement(OnlineMaps.instance.drawingElements[0]);
            }
        }
    }
}