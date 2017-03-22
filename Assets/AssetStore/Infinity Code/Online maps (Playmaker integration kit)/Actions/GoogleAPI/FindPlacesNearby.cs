using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("A Nearby Search lets you search for places within a specified area. You can refine your search request by supplying keywords or specifying the type of place you are searching for.")]
    public class FindPlacesNearby : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The latitude/longitude around which to retrieve place information. ")]
        public FsmVector2 coordinates;

        [RequiredField]
        [Tooltip("Defines the distance (in meters) within which to return place results. The maximum allowed radius is 50000 meters.")]
        public FsmInt radius = 5000;

        [RequiredField]
        [Tooltip("Your application's API key. This key identifies your application for purposes of quota management and so that places added from your application are made immediately available to your app. Visit the Google APIs Console to create an API Project and obtain your key.")]
        public FsmString googleApiKey;

        [Tooltip("A term to be matched against all content that Google has indexed for this place, including but not limited to name, type, and address, as well as customer reviews and other third-party content.")]
        public FsmString keyword;

        [Tooltip("One or more terms to be matched against the names of places, separated with a space character. Results will be restricted to those containing the passed name values. Note that a place may have additional names associated with it, beyond its listed name. The API will try to match the passed name value against all of these names. As a result, places may be returned in the results whose listed names do not match the search term, but whose associated names do.")]
        public FsmString _name;

        [Tooltip("Restricts the results to places matching at least one of the specified types. Types should be separated with a pipe symbol (type1|type2|etc).")]
        public FsmString types;

        [Tooltip("Restricts results to only those places within the specified range. Valid values range between 0 (most affordable) to 4 (most expensive), inclusive. The exact amount indicated by a specific value will vary from region to region.")]
        public FsmInt minPrice = -1;

        [Tooltip("Restricts results to only those places within the specified range. Valid values range between 0 (most affordable) to 4 (most expensive), inclusive. The exact amount indicated by a specific value will vary from region to region.")]
        public FsmInt maxPrice = -1;

        [Tooltip("Returns only those places that are open for business at the time the query is sent. ")]
        public FsmBool openNow = false;

        [Tooltip("Specifies the order in which results are listed. ")]
        public OnlineMapsFindPlaces.OnlineMapsFindPlacesRankBy rankBy = OnlineMapsFindPlaces.OnlineMapsFindPlacesRankBy.prominence;

        [Tooltip("On request complete event.")]
        public FsmEvent OnComplete;

        [UIHint(UIHint.Variable)]
        [Tooltip("Response as string.")]
        public FsmString storeResponseString;

        [UIHint(UIHint.Variable)]
        [Tooltip("Response as object.")]
        public FsmObject storeResponseObject;

        [UIHint(UIHint.Variable)]
        [Tooltip("Count of response results.")]
        public FsmInt storeResponseObjectCount;

        public override void Reset()
        {
            coordinates = null;
            radius = 5000;
            googleApiKey = null;
            keyword = new FsmString { UseVariable = true };
            _name = new FsmString { UseVariable = true };
            types = new FsmString { UseVariable = true };
            minPrice = new FsmInt { UseVariable = true, Value = -1 };
            maxPrice = new FsmInt { UseVariable = true, Value = -1 };
            openNow = new FsmBool { UseVariable = true, Value = false };
            rankBy = OnlineMapsFindPlaces.OnlineMapsFindPlacesRankBy.prominence;
            OnComplete = null;
            storeResponseString = null;
            storeResponseObject = null;
            storeResponseObjectCount = null;
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

            string keywordValue = keyword.IsNone ? null : keyword.Value;
            string nameValue = _name.IsNone ? null : _name.Value;
            string typesValue = types.IsNone ? null : types.Value;
            int minPriceValue = minPrice.IsNone ? -1 : minPrice.Value;
            int maxPriceValue = maxPrice.IsNone ? -1 : maxPrice.Value;
            bool openNowValue = openNow.IsNone ? false : openNow.Value;

            OnlineMapsTextWebService query = OnlineMapsGooglePlaces.FindNearby(coordinates.Value, radius.Value, googleApiKey.Value, keywordValue, 
                nameValue, typesValue, minPriceValue, maxPriceValue, openNowValue, rankBy);

            query.OnComplete += delegate(string s)
            {
                storeResponseString.Value = s;
                FindPlaceResponseWrapper wrapper = new FindPlaceResponseWrapper(s);
                storeResponseObject.Value = wrapper;
                storeResponseObjectCount.Value = wrapper.count;

                if (OnComplete != null) Fsm.Event(OnComplete);
            };
        }
    }
}