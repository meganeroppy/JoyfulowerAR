using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    public class FindAutocompleteResponseWrapper : Object
    {
        public OnlineMapsGooglePlacesAutocompleteResult[] results;

        public int count
        {
            get { return (results != null) ? results.Length : 0; }
        }

        public FindAutocompleteResponseWrapper(string response)
        {
            results = OnlineMapsGooglePlacesAutocomplete.GetResults(response);
        }
    }
}