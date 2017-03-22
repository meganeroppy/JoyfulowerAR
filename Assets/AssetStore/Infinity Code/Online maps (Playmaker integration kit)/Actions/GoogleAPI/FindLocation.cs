using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Find location by address or coordinates.")]
    public class FindLocation : FsmStateAction
    {
        [Tooltip("Location title.")]
        public FsmString address;

        [Tooltip("Location coordinates.")]
        public FsmVector2 coordinates;

        [Tooltip("Set map position to first result coordinates.")]
        public FsmBool moveToFirstResult = true;

        [Tooltip("On request complete event.")]
        public FsmEvent OnComplete;

        [Tooltip("Response as string.")]
        [UIHint(UIHint.Variable)]
        public FsmString storeResponseString;

        [Tooltip("First response coordinates.")]
        [UIHint(UIHint.Variable)]
        public FsmVector2 storeResponseCoordinates;

        public override void Reset()
        {
            address = new FsmString { UseVariable = true };
            coordinates = new FsmVector2{ UseVariable = true };
            moveToFirstResult = true;
            storeResponseString = null;
            storeResponseCoordinates = null;
            OnComplete = null;
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

            OnlineMapsTextWebService query = null;
            if (!address.IsNone) query = OnlineMapsGoogleGeocoding.Find(address.Value);
            else if (!coordinates.IsNone) query = OnlineMapsGoogleGeocoding.Find(coordinates.Value);

            if (query != null)
            {
                query.OnComplete += delegate (string result)
                {
                    storeResponseString.Value = result;
                    storeResponseCoordinates.Value = OnlineMapsGoogleGeocoding.GetCoordinatesFromResult(result);

                    if (OnComplete != null) Fsm.Event(OnComplete);
                    if (moveToFirstResult.Value) OnlineMapsGoogleGeocoding.MovePositionToResult(result);
                };
            }
        }
    }
}