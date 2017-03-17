using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GPSLoader : MonoBehaviour 
{
	public static GPSLoader instance;

	[HideInInspector]
	public LocationServiceStatus Status;

	[SerializeField]
	private string defaultPosKey;

	[System.NonSerialized]
	public string myPosKey;

	private void Awake()
	{
		instance = this;
		myPosKey = defaultPosKey;
	}

	/// <summary>
	/// GPSデータを取得する
	/// </summary>
	/// <param name="callback">取得成功時コールバック（GPSデータ）</param>
	/// <param name="errorCallback">エラー時コールバック</param>
	public IEnumerator GetData( System.Action<GpsPosition> callback, System.Action<string> errorCallback) 
	{
		GpsPosition ret;

		if( JoyfulowerSceneManager.instance != null && JoyfulowerSceneManager.instance.demo )
		{
			// デモモードのときはダミーデータを作成してコールバック
			ret =  ARObjectSetter.sampleData[myPosKey];
			callback( ret );

			Debug.LogWarning( "デモモードなのでダミーデータ返却：( " + ret.longitude.ToString() + ", " + ret.latitude.ToString() + " )" + myPosKey );

			yield break;
		}

		if (!Input.location.isEnabledByUser) 
		{
			// 位置情報が無効な端末の時はダミーデータを作成してコールバック
			errorCallback(" Input.location.isEnabledByUser == false ");

			ret =  ARObjectSetter.sampleData[myPosKey];
			callback( ret );

			Debug.LogWarning("位置情報が無効な端末なのでダミーデータ返却：( " + ret.longitude.ToString() + ", " + ret.latitude.ToString() + " )"+ myPosKey);

			yield break;
		}

		Input.location.Start();
		int maxWait =  120;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds(1);
			maxWait--;
		}
		if (maxWait < 1) {
			errorCallback("Timed out");
			yield break;
		}
		if (Input.location.status == LocationServiceStatus.Failed) {
			errorCallback("Unable to determine device location");
			yield break;
		}

		var locLast = Input.location.lastData;

		ret = GpsPosition.Loc2Gps( locLast );

		callback( ret );

		Input.location.Stop();
	}

	/// <summary>
	/// ダミーのGPSデータを取得する
	/// </summary>
	/// <param name="callback">取得成功時コールバック（GPSデータ）</param>
	/// <param name="errorCallback">エラー時コールバック</param>
	public void GetDataDummy( System.Action<GpsPosition> callback, System.Action<string> errorCallback )
	{
		// Austin
		GpsPosition ret =  	new GpsPosition( (decimal)30.264601,  (decimal)-97.739619, 1 );
		callback( ret );
	}
}