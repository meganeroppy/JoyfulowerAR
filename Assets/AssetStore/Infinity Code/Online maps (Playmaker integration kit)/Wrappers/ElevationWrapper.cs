using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    public class ElevationWrapper : Object
    {
        public OnlineMapsGoogleElevationResult[] elevations;

        public int count
        {
            get { return (elevations != null) ? elevations.Length : 0; }
        }

        public ElevationWrapper(OnlineMapsGoogleElevationResult[] elevations)
        {
            this.elevations = elevations;
        }
    }
}