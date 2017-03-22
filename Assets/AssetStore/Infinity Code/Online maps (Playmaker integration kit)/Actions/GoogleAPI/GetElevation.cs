using System.Linq;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(OnlineMapsCategories.GOOGLE)]
    [Tooltip("The Elevation API provides elevation data for all locations on the surface of the earth, including depth locations on the ocean floor (which return negative values).\nNote: Use Locations or Path + Samples.")]
    public class GetElevation : FsmStateAction
    {
        [Tooltip("Locations on the earth from which to return elevation data.")]
        public FsmVector2[] locations;

        [Tooltip("Path on the earth for which to return elevation data.")]
        public FsmVector2[] path;

        [Tooltip("Specifies the number of sample points along a path for which to return elevation data. The samples parameter divides the given path into an ordered set of equidistant points along the path.")]
        public FsmInt samples;

        [RequiredField]
        [Tooltip("Your application's API key. This key identifies your application for purposes of quota management and so that places added from your application are made immediately available to your app. Visit the Google APIs Console to create an API Project and obtain your key.")]
        public FsmString googleApiKey;

        [Tooltip("Client ID identifies you as a Maps API for Work customer and enables support and purchased quota for your application. Requests made without a client ID are not eligible for Maps API for Work benefits.")]
        public FsmString client;

        [Tooltip("Your welcome letter includes a cryptographic signing key, which you must use to generate unique signatures for your web service requests.")]
        public FsmString signature;

        [Tooltip("On request complete event.")]
        public FsmEvent OnComplete;

        [Tooltip("Response as string.")]
        [UIHint(UIHint.Variable)]
        public FsmString storeResponseString;

        [Tooltip("Response as object.")]
        [UIHint(UIHint.Variable)]
        public FsmObject storeResponseObject;

        [Tooltip("Elevation count.")]
        [UIHint(UIHint.Variable)]
        public FsmInt storeResponseCount;

        public override void Reset()
        {
            locations = null;
            path = null;
            samples = null;
            googleApiKey = null;
            client = null;
            signature = null;
            storeResponseString = null;
            storeResponseObject = null;
            storeResponseCount = null;
            OnComplete = null;
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

            OnlineMapsTextWebService query = null;

            if (locations != null) query = OnlineMapsGoogleElevation.Find(locations.Select(l => l.Value).ToArray(), googleApiKey.Value, client.Value, signature.Value);
            else if (path != null) query = OnlineMapsGoogleElevation.Find(path.Select(p => p.Value).ToArray(), samples.Value, googleApiKey.Value, client.Value, signature.Value);

            if (query != null)
            {
                query.OnComplete += delegate (string result)
                {
                    storeResponseString.Value = result;
                    OnlineMapsGoogleElevationResult[] results = OnlineMapsGoogleElevation.GetResults(result);
                    storeResponseObject.Value = new ElevationWrapper(results);
                    storeResponseCount.Value = (results != null) ? result.Length : 0;
                    if (OnComplete != null) Fsm.Event(OnComplete);
                };
            }
        }
    }
}