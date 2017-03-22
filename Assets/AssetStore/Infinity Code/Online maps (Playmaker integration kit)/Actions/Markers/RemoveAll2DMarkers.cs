using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.MARKERS)]
    [Tooltip("Remove all 2D markers from map.")]
    public class RemoveAll2DMarkers : FsmStateAction
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

            OnlineMaps.instance.RemoveAllMarkers();
        }
    }
}