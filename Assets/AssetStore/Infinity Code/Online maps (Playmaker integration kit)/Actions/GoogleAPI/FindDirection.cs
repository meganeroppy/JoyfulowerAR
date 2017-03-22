using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Find directions between locations.")]
    public class FindDirection : FsmStateAction
    {
        [Tooltip("Coordinates of the route begins.")]
        public FsmVector2 fromCoordinates;

        [Tooltip("Title of the route begins.")]
        public FsmString fromLocation;

        [Tooltip("Coordinates of the route ends.")]
        public FsmVector2 toCoordinates;

        [Tooltip("Title of the route ends.")]
        public FsmString toLocation;

        [Tooltip("Search alternative routes.")]
        public FsmBool alternativeRoutes = false;

        [Tooltip("On request complete event.")]
        public FsmEvent OnComplete;

        [Tooltip("Response as string.")]
        [UIHint(UIHint.Variable)]
        public FsmString storeResponseString;

        [Tooltip("Response as object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject storeResponseObject;

        [Tooltip("Route count.")]
        [UIHint(UIHint.Variable)]
        public FsmInt storeResponseRoutesCount;

        public override void Reset()
        {
            fromCoordinates = new FsmVector2 { UseVariable = true };
            fromLocation = new FsmString{ UseVariable = true };
            toCoordinates = new FsmVector2 { UseVariable = true };
            toLocation = new FsmString { UseVariable = true };
            alternativeRoutes = false;
            OnComplete = null;
            storeResponseString = null;
            storeResponseObject = null;
            storeResponseRoutesCount = null;
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
            if (!fromCoordinates.IsNone && !toCoordinates.IsNone) query = OnlineMapsGoogleDirections.Find(fromCoordinates.Value, toCoordinates.Value, alternativeRoutes.Value);
            else if (!fromCoordinates.IsNone && !toLocation.IsNone) query = OnlineMapsGoogleDirections.Find(fromCoordinates.Value, toLocation.Value, alternativeRoutes.Value);
            else if (!fromLocation.IsNone && !toCoordinates.IsNone) query = OnlineMapsGoogleDirections.Find(fromLocation.Value, toCoordinates.Value, alternativeRoutes.Value);
            else if (!fromLocation.IsNone && !toLocation.IsNone) query = OnlineMapsGoogleDirections.Find(fromLocation.Value, toLocation.Value, alternativeRoutes.Value);

            if (query == null) return;

            query.OnComplete += delegate(string s)
            {
                Debug.Log(s);
                storeResponseString.Value = s;
                FindDirectionResponseWrapper wrapper = new FindDirectionResponseWrapper(OnlineMapsDirectionStep.TryParseWithAlternatives(s));
                storeResponseObject.Value = wrapper;
                storeResponseRoutesCount.Value = (wrapper.routes != null) ? wrapper.routes.Length : 0;
                if (OnComplete != null) Fsm.Event(OnComplete);
            };
        }
    }
}