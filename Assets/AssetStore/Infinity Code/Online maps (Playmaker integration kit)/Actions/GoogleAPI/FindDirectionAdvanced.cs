using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Find directions between locations with full props.")]
    public class FindDirectionAdvanced : FsmStateAction
    {
        [Tooltip("Coordinates of the route begins.")]
        public FsmVector2 fromCoordinates;

        [Tooltip("Title of the route begins.")]
        public FsmString fromLocation;

        [Tooltip("Coordinates of the route ends.")]
        public FsmVector2 toCoordinates;

        [Tooltip("Title of the route ends.")]
        public FsmString toLocation;

        [Tooltip("Specifies the mode of transport to use when calculating directions.")]
        public OnlineMapsGoogleDirections.Mode mode = OnlineMapsGoogleDirections.Mode.driving;

        [Tooltip("Specifies an array of waypoints.\nWaypoints alter a route by routing it through the specified location(s).\nA waypoint is specified as either a latitude/longitude coordinate or as an address which will be geocoded. \nWaypoints are only supported for driving, walking and bicycling directions.")]
        public string[] waypoints;

        [Tooltip("Search alternative routes.")]
        public FsmBool alternativeRoutes = false;

        [Tooltip("Indicates that the calculated route(s) should avoid the indicated features.")]
        public OnlineMapsGoogleDirections.Avoid avoid = OnlineMapsGoogleDirections.Avoid.none;

        [Tooltip("Specifies the unit system to use when displaying results.")]
        public OnlineMapsGoogleDirections.Units units = OnlineMapsGoogleDirections.Units.metric;

        [Tooltip("Specifies the region code, specified as a ccTLD (\"top-level domain\") two-character value.")]
        public FsmString region;

        [Tooltip("Specifies the language in which to return results.")]
        public FsmString language;

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
            fromLocation = new FsmString { UseVariable = true };
            toCoordinates = new FsmVector2 { UseVariable = true };
            toLocation = new FsmString { UseVariable = true };
            mode = OnlineMapsGoogleDirections.Mode.driving;
            waypoints = null;
            alternativeRoutes = false;
            avoid = OnlineMapsGoogleDirections.Avoid.none;
            units = OnlineMapsGoogleDirections.Units.metric;
            region = null;
            language = null;
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

            object from;
            object to;

            if (!fromCoordinates.IsNone) from = fromCoordinates.Value;
            else if (!fromLocation.IsNone) from = fromLocation.Value;
            else return;

            if (!toCoordinates.IsNone) to = toCoordinates.Value;
            else if (!toLocation.IsNone) to = toLocation.Value;
            else return;

            OnlineMapsGoogleDirections.Params p = new OnlineMapsGoogleDirections.Params(from, to);
            p.mode = mode;
            p.waypoints = waypoints;
            p.alternatives = alternativeRoutes.Value;
            p.avoid = avoid;
            p.units = units;
            p.region = region.Value;
            p.language = language.Value;

            query = OnlineMapsGoogleDirections.Find(p);
            if (query == null) return;

            query.OnComplete += delegate(string s)
            {
                Debug.Log(s);
                storeResponseString.Value = s;
                FindDirectionResponseWrapper wrapper = new FindDirectionResponseWrapper(OnlineMapsDirectionStep.TryParseWithAlternatives(s));
                storeResponseObject.Value = wrapper;
                storeResponseRoutesCount.Value = wrapper.routes != null ? wrapper.routes.Length : 0;
                if (OnComplete != null) Fsm.Event(OnComplete);
            };
        }
    }
}