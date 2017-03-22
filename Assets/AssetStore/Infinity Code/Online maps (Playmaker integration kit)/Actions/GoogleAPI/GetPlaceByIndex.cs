using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Gets place info by index from FindPlace response.")]
    public class GetPlaceByIndex : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("FindPlace response object")]
        public FsmObject responseObject;

        [RequiredField]
        [Tooltip("Place index")]
        public FsmInt placeIndex = 0;

        [UIHint(UIHint.Variable)]
        [Tooltip("URL of a recommended icon which may be displayed to the user when indicating this result.")]
        public FsmString storeIcon;

        [UIHint(UIHint.Variable)]
        [Tooltip("Unique stable identifier denoting this place.")]
        public FsmString storeID;

        [UIHint(UIHint.Variable)]
        [Tooltip("Place coordinates.")]
        public FsmVector2 storeLocation;

        [UIHint(UIHint.Variable)]
        [Tooltip("Human-readable name for the returned result.")]
        public FsmString storeName;

        [UIHint(UIHint.Variable)]
        [Tooltip("Human-readable address of this place.")]
        public FsmString storeFormattedAddress;

        [UIHint(UIHint.Variable)]
        [Tooltip("Value indicating if the place is open at the current time.")]
        public FsmBool storeOpenNow;

        [UIHint(UIHint.Variable)]
        [Tooltip("Unique identifier for a place.")]
        public FsmString storePlaceID;

        [UIHint(UIHint.Variable)]
        [Tooltip("The price level of the place, on a scale of 0 to 4.")]
        public FsmInt storePriceLevel;

        [UIHint(UIHint.Variable)]
        [Tooltip("Place's rating, from 1.0 to 5.0, based on aggregated user reviews.")]
        public FsmFloat storeRating;

        [UIHint(UIHint.Variable)]
        [Tooltip("Unique token that you can use to retrieve additional information about this place in a Place Details request.")]
        public FsmString storeReference;

        [UIHint(UIHint.Variable)]
        [Tooltip("Indicates the scope of the place_id.")]
        public FsmString storeScope;

        [UIHint(UIHint.Variable)]
        [Tooltip("Feature name of a nearby location.")]
        public FsmString storeVicinity;

        public override void Reset()
        {
            responseObject = null;
            placeIndex = 0;
            storeIcon = null;
            storeID = null;
            storeLocation = null;
            storeName = null;
            storeFormattedAddress = null;
            storeOpenNow = null;
            storePlaceID = null;
            storePriceLevel = null;
            storeRating = null;
            storeReference = null;
            storeScope = null;
            storeVicinity = null;
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

            if (responseObject.Value.GetType() != typeof(FindPlaceResponseWrapper))
            {
                Debug.Log("Its not FindPlaceResponseWrapper instance");
                return;
            }

            FindPlaceResponseWrapper wrapper = responseObject.Value as FindPlaceResponseWrapper;
            if (placeIndex.Value < 0 || placeIndex.Value >= wrapper.count) return;

            OnlineMapsGooglePlacesResult result = wrapper.results[placeIndex.Value];
            storeIcon.Value = result.icon;
            storeID.Value = result.id;
            storeLocation.Value = result.location;
            storeName.Value = result.name;
            storeFormattedAddress.Value = result.formatted_address;
            storeOpenNow.Value = result.open_now;
            storePlaceID.Value = result.place_id;
            storePriceLevel.Value = result.price_level;
            storeRating.Value = result.rating;
            storeReference.Value = result.reference;
            storeScope.Value = result.scope;
            storeVicinity.Value = result.vicinity;
        }
    }
}