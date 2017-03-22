using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("The Place Autocomplete returns place predictions. The request specifies a textual search string and optional geographic bounds. The service can be used to provide autocomplete functionality for text-based geographic searches, by returning places such as businesses, addresses and points of interest as a user types.")]
    public class FindAutocomplete : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The text string on which to search. The Place Autocomplete service will return candidate matches based on this string and order results based on their perceived relevance.")]
        public FsmString input;
        
        [RequiredField]
        [Tooltip("Your application's API key. This key identifies your application for purposes of quota management. Visit the Google APIs Console to select an API Project and obtain your key.")]
        public FsmString googleApiKey;

        [Tooltip("The types of place results to return.")]
        public FsmString types;

        [Tooltip("The position, in the input term, of the last character that the service uses to match predictions. For example, if the input is 'Google' and the offset is 3, the service will match on 'Goo'. The string determined by the offset is matched against the first word in the input term only. For example, if the input term is 'Google abc' and the offset is 3, the service will attempt to match against 'Goo abc'. If no offset is supplied, the service will use the whole term. The offset should generally be set to the position of the text caret.")]
        public FsmInt offset = -1;

        [Tooltip("The point around which you wish to retrieve place information.")]
        public FsmVector2 coordinates = default (Vector2);

        [Tooltip("The distance (in meters) within which to return place results. Note that setting a radius biases results to the indicated area, but may not fully restrict results to the specified area.")]
        public FsmInt radius = -1;

        [Tooltip("The language in which to return results.")]
        public FsmString language;

        [Tooltip("A grouping of places to which you would like to restrict your results. Currently, you can use components to filter by country. The country must be passed as a two character, ISO 3166-1 Alpha-2 compatible country code. For example: components=country:fr would restrict your results to places within France.")]
        public FsmString components;

        [Tooltip("On request complete event.")]
        public FsmEvent OnComplete;

        [Tooltip("Response as string.")]
        [UIHint(UIHint.Variable)]
        public FsmString storeResponseString;

        [Tooltip("Response as object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject storeResponseObject;

        [Tooltip("Results count.")]
        [UIHint(UIHint.Variable)]
        public FsmInt storeResponseResultCount;

        public override void Reset()
        {
            input = null;
            googleApiKey = null;
            types = new FsmString { UseVariable = true };
            offset = new FsmInt { UseVariable = true, Value = -1};
            coordinates = new FsmVector2{UseVariable = true};
            radius = new FsmInt{UseVariable = true, Value = -1};
            language = new FsmString { UseVariable = true };
            components = new FsmString { UseVariable = true };
            OnComplete = null;
            storeResponseString = null;
            storeResponseObject = null;
            storeResponseResultCount = null;
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

            string typesValue = types.IsNone ? null : types.Value;
            int offsetValue = offset.IsNone ? -1 : offset.Value;
            Vector2 coordinatesValue = coordinates.IsNone ? default(Vector2) : coordinates.Value;
            int radiusValue = radius.IsNone ? -1 : radius.Value;
            string languageValue = language.IsNone ? null : language.Value;
            string componentsValue = components.IsNone ? null : components.Value;

            OnlineMapsTextWebService query = OnlineMapsGooglePlacesAutocomplete.Find(input.Value, googleApiKey.Value, 
                typesValue, offsetValue, coordinatesValue, radiusValue, languageValue, componentsValue);

            query.OnComplete += delegate(string s)
            {
                storeResponseString.Value = s;
                FindAutocompleteResponseWrapper wrapper = new FindAutocompleteResponseWrapper(s);
                storeResponseObject.Value = wrapper;
                storeResponseResultCount.Value = wrapper.count;
                if (OnComplete != null) Fsm.Event(OnComplete);
            };
        }
    }
}