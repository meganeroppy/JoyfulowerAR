using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Returns information about a set of places based on a string — for example 'pizza in New York' or 'shoe stores near Ottawa'. The service responds with a list of places matching the text string and any location bias that has been set.")]
    public class FindPlacesText : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The text string on which to search, for example: 'restaurant'. The Google Places service will return candidate matches based on this string and order the results based on their perceived relevance.")]
        public FsmString query;

        [RequiredField]
        [Tooltip("Your application's API key. This key identifies your application for purposes of quota management and so that places added from your application are made immediately available to your app. Visit the Google APIs Console to create an API Project and obtain your key.")]
        public FsmString googleApiKey;

        [Tooltip("The latitude/longitude around which to retrieve place information. ")]
        public FsmVector2 coordinates;

        [Tooltip("Defines the distance (in meters) within which to return place results. The maximum allowed radius is 50000 meters.")]
        public FsmInt radius;

        [Tooltip("The language code, indicating in which language the results should be returned, if possible.")]
        public FsmString language;

        [Tooltip("Restricts the results to places matching at least one of the specified types. Types should be separated with a pipe symbol (type1|type2|etc).")]
        public FsmString types;

        [Tooltip("Restricts results to only those places within the specified range. Valid values range between 0 (most affordable) to 4 (most expensive), inclusive. The exact amount indicated by a specific value will vary from region to region.")]
        public FsmInt minPrice = -1;

        [Tooltip("Restricts results to only those places within the specified range. Valid values range between 0 (most affordable) to 4 (most expensive), inclusive. The exact amount indicated by a specific value will vary from region to region.")]
        public FsmInt maxPrice = -1;

        [Tooltip("Returns only those places that are open for business at the time the query is sent. ")]
        public FsmBool openNow = false;

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
            query = null;
            googleApiKey = null;
            language = new FsmString { UseVariable = true };
            coordinates = new FsmVector2 { UseVariable = true };
            radius = new FsmInt { UseVariable = true, Value = 5000 };
            types = new FsmString { UseVariable = true };
            minPrice = new FsmInt { UseVariable = true, Value = -1 };
            maxPrice = new FsmInt { UseVariable = true, Value = -1};
            openNow = new FsmBool { UseVariable = true, Value = false };
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

            Vector2 coordinatesValue = coordinates.IsNone ? default(Vector2): coordinates.Value;
            int radiusValue = radius.IsNone ? -1 : radius.Value;
            string languageValue = language.IsNone ? null : language.Value;
            string typesValue = types.IsNone ? null : types.Value;
            int minPriceValue = minPrice.IsNone ? -1 : minPrice.Value;
            int maxPriceValue = maxPrice.IsNone ? -1 : maxPrice.Value;
            bool openNowValue = openNow.IsNone ? false : openNow.Value;

            OnlineMapsTextWebService q = OnlineMapsGooglePlaces.FindText(query.Value, googleApiKey.Value, coordinatesValue, radiusValue, languageValue, typesValue,
                minPriceValue, maxPriceValue, openNowValue);

            q.OnComplete += delegate(string s)
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