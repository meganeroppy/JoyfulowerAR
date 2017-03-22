using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Gets result info by index from FindAutocomplete response.")]
    public class GetAutocompleteResultByIndex : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("FindAutocomplete response object.")]
        public FsmObject responseObject;

        [RequiredField]
        [Tooltip("Result index.")]
        public FsmInt resultIndex = 0;

        [UIHint(UIHint.Variable)]
        [Tooltip("Contains the human-readable name for the returned result. For establishment results, this is usually the business name.")]
        public FsmString storeDescription;

        [UIHint(UIHint.Variable)]
        [Tooltip("Unique stable identifier denoting this place. This identifier may not be used to retrieve information about this place, but can be used to consolidate data about this place, and to verify the identity of a place across separate searches. Note: The id is deprecated in favor of place_id.")]
        public FsmString storeID;

        [UIHint(UIHint.Variable)]
        [Tooltip("Unique identifier for a place. To retrieve information about the place, pass this identifier in the placeId field of a Places API request.")]
        public FsmString storePlaceID;

        [UIHint(UIHint.Variable)]
        [Tooltip("Unique token that you can use to retrieve additional information about this place in a Place Details request. Although this token uniquely identifies the place, the converse is not true. A place may have many valid reference tokens. It's not guaranteed that the same token will be returned for any given place across different searches. Note: The reference is deprecated in favor of place_id.")]
        public FsmString storeReference;

        public override void Reset()
        {
            responseObject = null;
            resultIndex = 0;
            storeDescription = null;
            storeID = null;
            storePlaceID = null;
            storeReference = null;
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

            if (responseObject.IsNone) return;

            if (responseObject.Value.GetType() != typeof(FindAutocompleteResponseWrapper))
            {
                Debug.Log("Its not FindAutocompleteResponseWrapper instance");
                return;
            }

            FindAutocompleteResponseWrapper wrapper = responseObject.Value as FindAutocompleteResponseWrapper;
            if (resultIndex.Value < 0 || resultIndex.Value >= wrapper.count) return;

            OnlineMapsGooglePlacesAutocompleteResult result = wrapper.results[resultIndex.Value];
            storeDescription.Value = result.description;
            storeID.Value = result.id;
            storePlaceID.Value = result.place_id;
            storeReference.Value = result.reference;
        }
    }
}