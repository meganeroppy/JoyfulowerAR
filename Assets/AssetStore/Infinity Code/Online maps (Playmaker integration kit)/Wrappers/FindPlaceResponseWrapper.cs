using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    public class FindPlaceResponseWrapper : Object
    {
        public OnlineMapsGooglePlacesResult[] results;

        public int count
        {
            get { return (results != null) ? results.Length : 0; }
        }

        public FindPlaceResponseWrapper(string s)
        {
            results = OnlineMapsGooglePlaces.GetResults(s);
        }
    }
}