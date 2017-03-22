using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    public class MarkerWrapper : Object
    {
        public OnlineMapsMarkerBase marker;
        public static MarkerWrapper activeMarker;

        public MarkerWrapper(OnlineMapsMarkerBase marker)
        {
            this.marker = marker;
        }
    }
}