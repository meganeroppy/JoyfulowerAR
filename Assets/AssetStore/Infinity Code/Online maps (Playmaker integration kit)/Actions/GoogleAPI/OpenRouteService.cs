using System.Linq;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Find directions between locations using Open Route Service.")]
    public class OpenRouteService : FsmStateAction
    {
        [Tooltip("Coordinates of the route begins.")]
        public FsmVector2 fromCoordinates;

        [Tooltip("Coordinates of the route ends.")]
        public FsmVector2 toCoordinates;

        [Tooltip("Language of instruction.")]
        public FsmString language = "en";

        [Tooltip("The preference of the routing.")]
        public OnlineMapsOpenRouteService.OnlineMapsOpenRouteServicePref pref = OnlineMapsOpenRouteService.OnlineMapsOpenRouteServicePref.Fastest;

        [Tooltip("No Motorways.")]
        public FsmBool noMotorways = false;

        [Tooltip("No Tollways.")]
        public FsmBool noTollways = false;

        [Tooltip("Coordinates of the via positions.")]
        public FsmVector2[] via;

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
            toCoordinates = new FsmVector2 { UseVariable = true };

            language = "en";
            pref = OnlineMapsOpenRouteService.OnlineMapsOpenRouteServicePref.Fastest;
            noMotorways = false;
            noTollways = false;
            via = null;

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

            if (fromCoordinates.IsNone || toCoordinates.IsNone) return;

            Vector2[] vias = via.Select(v => v.Value).ToArray();

            
            OnlineMapsTextWebService query = OnlineMapsOpenRouteService.Find(fromCoordinates.Value, toCoordinates.Value, language.Value, pref, noMotorways.Value, noTollways.Value, vias);
            if (query == null) return;

            query.OnComplete += delegate(string s)
            {
                Debug.Log(s);
                storeResponseString.Value = s;

                FindDirectionResponseWrapper wrapper = new FindDirectionResponseWrapper(new[] {OnlineMapsDirectionStep.TryParseORS(s)});
                storeResponseObject.Value = wrapper;
                storeResponseRoutesCount.Value = (wrapper.routes != null) ? wrapper.routes.Length : 0;
                if (OnComplete != null) Fsm.Event(OnComplete);
            };
        }
    }
}