using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GENERAL)]
    [Tooltip("Set new map texture.\nWorkds only if 'Source=Texture'.\nIn texture import settings must be enabled 'Read / Write enabled'. ")]
    public class SetMapTexture : FsmStateAction
    {
        [RequiredField]
        [Tooltip("New texture. In texture import settings must be enabled 'Read / Write enabled'.")]
        public FsmTexture texture;

        public override void Reset()
        {
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

            OnlineMaps.instance.SetTexture(texture.Value as Texture2D);
        }
    }
}