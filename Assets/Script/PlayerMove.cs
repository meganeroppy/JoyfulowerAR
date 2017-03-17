using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerMove : MonoBehaviour 
{
	public static PlayerMove instance;

	public Text logObj;
	public Text cntObj;
	public Text statObj;

	private GPSLoader gpsl;

	private float intervalSec = 15f;

	public GpsPosition currentGpsLocation = null;

	/// <summary>
	/// 現在ターゲットに指定されているUFO
	/// </summary>
	[NonSerialized]
	public ArUfoObject currentTargetUfo = null;

	void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		gpsl = GetComponent<GPSLoader>();

		StartCoroutine( StartLoad() );
	}

	/// <summary>
	///  開始時ロード
	/// </summary>
	private IEnumerator StartLoad()
	{
		// シーンマネージャが定義されるまで待機
		while( JoyfulowerSceneManager.instance == null )
			yield return null;

		StartCoroutine( GetGpsPosRegularly() );
	}

	private void Update()
	{
		statObj.text = "GPS State = " + gpsl.Status.ToString();
	}

	/// <summary>
	/// 一定間隔でGPS位置お取得する
	/// </summary>
	private IEnumerator GetGpsPosRegularly()
	{
		while(true){

			cntObj.text = "getting...";

			yield return StartCoroutine( gpsl.GetData( res =>
				{
					// 自分の位置更新
					UpdateOriginGpsPosition( res );

					// ログ出力
					SetLog( res );

				},
				err =>
				{
					SetLogError( err );
				}
			) );


			// 次の更新時間まで待機
			var timer = 0f;
			while ( timer < intervalSec ){
				cntObj.text = "GPS Update In " + ((int)(intervalSec - timer)).ToString() + " Sec"; 

				timer += Time.deltaTime;
				yield return null;
			}
		}
	}

	/// <summary>
	/// ログ表示
	/// </summary>
	private void SetLog(GpsPosition info )
	{
		string s = "";
		var lat = info.latitude;
		var lon = info.longitude;
		var alt = info.altitude;
	//	float hAcu = info.horizontalAccuracy;
	//	float vAcu = info.verticalAccuracy;
	//	double time = info.timestamp;

		double timestamp = Input.location.lastData.timestamp;
		DateTime datetime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp).ToLocalTime();

		s +=  string.Format("(latitude = {0}, longitude = {1}, altitude = {2})\n", lat,lon,alt);
	//	s +=  string.Format("(hAccuracy = {0}, vAccuracy = {1})\n", hAcu,vAcu);
		s +=  string.Format("(timestamp = {0})\n", datetime.ToString());

		logObj.text = s;
	}

	/// <summary>
	/// エラー表示
	/// </summary>
	private void SetLogError(string err)
	{
		logObj.text = err;
	}

	/// <summary>
	/// 位置情報に基づいて自分の位置を変更
	/// 初回は位置情報の登録のみ
	/// </summary>
	/// <param name="loc">Location.</param>
	private void UpdatePosition( LocationInfo newLocation )
	{
		var gpsPos = GpsPosition.Loc2Gps( newLocation );
		UpdateOriginGpsPosition(gpsPos);
	}

	/// <summary>
	/// 位置情報に基づいて自分の位置を変更
	/// </summary>
	private void UpdateOriginGpsPosition( GpsPosition newLocation )
	{
		// 現在GPS位置を更新
		currentGpsLocation = newLocation;

		// TODO ↓ 自分の座標は常に0,0,0だしここから下の処理いらなくない？

		// 移動先座標を計算
//		GpsPosition distance = GpsPosition.GetDistance(currentGpsLocation, newLocation );
//		Vector3 newPos = new Vector3((float)distance.longitude, (float)distance.altitude, (float)distance.latitude);
//		transform.position = newPos;
	}

	public GameObject AttackPrefab;

	/// <summary>
	/// プレイヤーからの攻撃
	/// </summary>
	public void Attack()
	{
		Instantiate(AttackPrefab, transform.position, transform.rotation);
	}
}
