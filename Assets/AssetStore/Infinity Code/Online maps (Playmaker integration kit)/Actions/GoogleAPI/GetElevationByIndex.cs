using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("Gets elevation value from GetElevation response.")]
    public class GetElevationByIndex : FsmStateAction
    {
        [RequiredField]
        [Tooltip("GetElevation response object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject responseObject;

        [RequiredField]
        [Tooltip("Elevation index.")]
        public FsmInt elevationIndex = 0;

        [Tooltip("Elevation of the location in meters.")]
        [UIHint(UIHint.Variable)]
        public FsmFloat storeElevation;

        [Tooltip("Position for which elevation data is being computed. Note that for path requests, the set of location elements will contain the sampled points along the path.")]
        [UIHint(UIHint.Variable)]
        public FsmVector2 storeLocation;

        [Tooltip("Maximum distance between data points from which the elevation was interpolated, in meters.")]
        [UIHint(UIHint.Variable)]
        public FsmFloat storeResolution;

        public override void Reset()
        {
            responseObject = null;
            elevationIndex = 0;
            storeElevation = null;
            storeLocation = null;
            storeResolution = null;
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

            if (responseObject.Value.GetType() != typeof(ElevationWrapper))
            {
                Debug.Log("Its not ElevationWrapper instance");
                return;
            }

            ElevationWrapper wrapper = responseObject.Value as ElevationWrapper;

            if (elevationIndex.Value < 0 || elevationIndex.Value >= wrapper.count) return;

            OnlineMapsGoogleElevationResult result = wrapper.elevations[elevationIndex.Value];
            storeElevation.Value = result.elevation;
            storeLocation.Value = result.location;
            storeResolution.Value = result.resolution;
        }
    }
}