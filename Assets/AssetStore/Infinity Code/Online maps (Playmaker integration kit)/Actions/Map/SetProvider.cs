using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GENERAL)]
    [Tooltip("Sets provider of tiles and map type.")]
    public class SetProvider : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Provider of tiles and maptype.")]
        public FsmString mapType = "arcgis";

        [Tooltip("URL pattern for custom provider.")]
        public FsmString customPattern;

        public override void Reset()
        {
            mapType = "arcgis";
            customPattern = null;
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

            OnlineMaps.instance.activeType = OnlineMapsProvider.FindMapType(mapType.Value);
            
            if (OnlineMaps.instance.activeType.isCustom)
            {
                OnlineMaps.instance.customProviderURL = !customPattern.IsNone ? customPattern.Value : "";
            }
            else OnlineMaps.instance.customProviderURL = "";
        }
    }
}