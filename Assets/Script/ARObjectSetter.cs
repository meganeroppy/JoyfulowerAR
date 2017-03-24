using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ARオブジェクト配置用クラス
/// </summary>
public class ARObjectSetter : MonoBehaviour 
{
	public static ARObjectSetter instance;

	/// <summary>
	/// ARオブジェクトのプレハブ
	/// </summary>
	public GameObject arObject;

	public List<string> appearAreas;

	/// <summary>
	/// ARオブジェクトのリスト
	/// </summary>
	public static List<ArUfoObject> ufoList = new List<ArUfoObject>();

	/// <summary>
	/// 出現中UFO数テキスト
	/// </summary>
	[SerializeField]
	private Text ufoCount = null;

	/// <summary>
	/// UFO情報取得間隔秒
	/// </summary>
//	public float getInterval = 120;
	public float getInterval = 15;

	/// <summary>
	/// 強制アップデートフラグ
	/// </summary>
	public bool forceGetInfo = false;

	/// <summary>
	/// UFO体力送信間隔
	/// </summary>
	public float setInterval = 10f;

	/// <summary>
	/// 強制体力更新フラグ
	/// </summary>
	public bool forceSetInfo = false;

	/// <summary>
	/// APIマネージャ
	/// </summary>
	private api_ygg.APIManager apiManager;

	/// <summary>
	/// 初回ロードが終わったときに実行されるイベント
	/// </summary>
	[System.NonSerialized]
	public System.Action initloadCompleteEvent = null;

	/// <summary>
	/// 表示テスト用サンプルデータ
	/// ( 緯度(lat), 経度(lng), 高度(alt) )
	/// </summary>
	public static Dictionary<string, GpsPosition> sampleData = new Dictionary<string, GpsPosition>() {
		{"SoraCity", new GpsPosition( (decimal)35.698561,  (decimal)139.7667832, 180 )},
		{"DaiBiru", new GpsPosition( (decimal)35.699194,  (decimal)139.772122, 180 )},
		{"TokyoDoom", new GpsPosition( (decimal)35.70564,  (decimal)139.751891, 180 )},
		{"Tsutenkaku", new GpsPosition( (decimal)34.652499,  (decimal)135.506306, 180 )},
		{"SapporoDoom", new GpsPosition( (decimal)43.015775,  (decimal)141.409529, 180 )},
		{"BiwaLake", new GpsPosition( (decimal)35.345639,  (decimal)136.170901, 180 )},
		{"MtFuji", new GpsPosition( (decimal)35.360556,  (decimal)138.727778, 180 )},	
		{"NagoyaDoom", new GpsPosition( (decimal)35.185845,  (decimal)136.947484, 180 )},
		{"MtAso", new GpsPosition( (decimal)32.88692,  (decimal)131.084107, 180 )},
		{"ToriNoShima", new GpsPosition( (decimal)35.631103,  (decimal)139.769153, 180 )},
		{"OdaibaSea", new GpsPosition( (decimal)35.619254,  (decimal)139.807324, 180 )},

		{"Austin1", new GpsPosition( (decimal)30.265700,  (decimal)-97.736342, 200 )},// デモ用UFO位置A-1
		{"Austin2", new GpsPosition( (decimal)30.280738,  (decimal)-97.7378, 180 )},// デモ用UFO位置A-2
		{"Austin3", new GpsPosition( (decimal)30.250189,  (decimal)-97.739997, 160 )},// デモ用UFO位置A-3
		{"Austin_Player", new GpsPosition( (decimal)30.265089,  (decimal)-97.738997, 0 )}, // デモ用プレイヤー位置A

		{"KidsPlate1", new GpsPosition( (decimal)35.68541,  (decimal)139.7813, 180 )}, // デモ用プレイヤー位置K-1
		{"KidsPlate2", new GpsPosition( (decimal)35.68551,  (decimal)139.7819, 180 )}, // デモ用プレイヤー位置K-2
		{"KidsPlate3", new GpsPosition( (decimal)35.68539,  (decimal)139.7824, 180 )}, // デモ用プレイヤー位置K-3
		{"KidsPlate_Player", new GpsPosition( (decimal)35.68549,  (decimal)139.7821, 0 )}, // デモ用プレイヤー位置K
	};

	private void Awake()
	{
		instance = this;
		updateChatInfoOnce = false;
	}

	private void Start () 
	{
		apiManager = GetComponent<api_ygg.APIManager>();
		if( apiManager == null )
		{
			apiManager = gameObject.AddComponent<api_ygg.APIManager>();
		}
			
		// ベースシーンがロードされていなかったらロードする
		var sceneName = "BaseScene";
		var baseScene = SceneManager.GetSceneByName(sceneName);
		if( !baseScene.isLoaded )
		{
			SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		}

		StartCoroutine( StartLoad() );
	}

	/// <summary>
	/// 必要な情報をロードしてから初期設定を行う
	/// </summary>
	IEnumerator StartLoad()
	{
		// シーンマネージャが定義されるまで待機
		while( YggdraSceneManager.instance == null ) 
			yield return null;

		// プレイヤーの初期位置が設定されるまで待機
		while( PlayerMove.instance.currentGpsLocation == null )
			yield return null;

		// このシーンがアクティブになるまで待機
		var mySceneName = "Main";
		while( !SceneManager.GetActiveScene().name.Equals( mySceneName ) )
			yield return null;
		
		// UFO情報の取得を開始
		StartCoroutine( UpdateUfoInfo() );

		// 一定間隔でUFOの体力減少をサーバー通知
		StartCoroutine( SetUfoHealth() );

		// チャット情報の取得を開始
		StartCoroutine( UpdateChatInfo() );
	}

	/// <summary>
	/// 一定間隔でUFO情報取得
	/// </summary>
	private IEnumerator UpdateUfoInfo()
	{
		while(true)
		{
			var request = new api_ygg.GetUfoInfoRequest();
			var gpsLoc = PlayerMove.instance.currentGpsLocation;
			request.x = (double)gpsLoc.longitude;
			request.y = (double)gpsLoc.latitude;
			request.type = 1;
			request.u_rand = YggdraSceneManager.instance.useGatheredData;

			// UFO情報をサーバーから取得して表示する
			StartCoroutine( apiManager.GetUfoInfo( res =>
				{
					var param = res.ufoposlist;

					// ここから関数でよくない？
					RemoveDestroyedObjects( param );

					for( int i = 0 ; i < param.Count ; i++ )
					{
						var ufoData = param[ i ];
						SetUfoObject( ufoData );
					}
				},
				request
			) );

			// 初回ロード時イベントがあれば実行
			if( initloadCompleteEvent != null )
			{
				initloadCompleteEvent();
				initloadCompleteEvent = null;
			}

			// 次の更新まで待機
			var timer = 0f;
			while ( timer < getInterval && !forceGetInfo ){
				timer += Time.deltaTime;
				yield return null;
			}
			forceGetInfo = false;
		}
	}
		
	/// <summary>
	/// UFOの体力にローカルで影響を与えたらサーバーに通知を行う
	/// </summary>
	IEnumerator SetUfoHealth()
	{
		while(true)
		{
			List<api_ygg.AttackUfoRequest> changeList = null;

			// 全てのUFOを捜査
			for( int i = 0 ; i < ufoList.Count ; i++ )
			{
				var ufo = ufoList[ i ];

				// ローカルのダメージ値が１以上ならリストに追加
				if( ufo.damage > 0 )
				{
					if( changeList == null )
					{
						changeList = new List<api_ygg.AttackUfoRequest>();
					}
					var param = new api_ygg.AttackUfoRequest();

					param.ufo_id = ufo.GetId();
					param.damage = ufo.damage;

					changeList.Add( param );
				}

				if( !YggdraSceneManager.instance.demo )
				{
					// ローカルでのダメージを初期化
					ufo.ResetLocalDamge();
				}
			}

			if( changeList != null )
			{
				yield return StartCoroutine( apiManager.AttackUfo( changeList, res =>
					{
						var param = res.ufoposlist;


						// ここから関数でよくない？
						RemoveDestroyedObjects( param );

						for( int i = 0 ; i < param.Count ; i++ )
						{
							var ufoData = param[ i ];

							// 変更リストのIDのUFOだけ
							if( changeList.Find( o => o.ufo_id == ufoData.ufo_id ) != null )
							{
								SetUfoObject( ufoData );
							}
						}
					} ) );
			}
			
			// 次の更新まで待機
			var timer = 0f;
			while ( timer < getInterval && !forceSetInfo ){
				timer += Time.deltaTime;
				yield return null;
			}
			forceGetInfo = false;	
		}
	}

	public bool dispChatObjOnUI = false;

	/// <summary>
	/// 次の更新をスキップ
	/// </summary>
	public bool skipNextUpdateChatInfo = false;

	/// <summary>
	/// 一度チャット情報を取得したか？
	/// </summary>
	public bool updateChatInfoOnce{get; private set;}

	/// <summary>
	/// チャット情報を取得
	/// </summary>
	private IEnumerator UpdateChatInfo()
	{
		while(true)
		{
			if( !skipNextUpdateChatInfo ){
				var request = new api_ygg.UpdateChatRequest();

				// 正しいIDをセット
				request.u_id = 1;

				// 自分の位置をセット
				request.x = (double)PlayerMove.instance.currentGpsLocation.longitude;
				request.y = (double)PlayerMove.instance.currentGpsLocation.latitude;

				// オプションを指定
				request.type = updateChatInfoOnce ? 4 : 3; // 初回は全て取得　２回目以降は初回取得以降に送信された情報のみ取得
				request.u_rand = YggdraSceneManager.instance.useGatheredData;

				yield return apiManager.UpdateChatInfo( res => 
					{
					//	SetChatObjects(res);
						updateChatInfoOnce = true;
					}, request );
			}	
			else
			{
				skipNextUpdateChatInfo = false;
			}

			var timer = 0f;
			while ( timer < getInterval || skipNextUpdateChatInfo ){
				timer += Time.deltaTime;
				yield return null;
			}
		}
	}


	/// <summary>
	/// スケールを 1 / 1000
	/// にするフラグ
	/// </summary>
	[SerializeField]
	bool microVision = false;

	/// <summary>
	/// サーバー上に存在していないオブジェクトを削除する
	/// </summary>
	/// <param name="ufoData"></param>
	private void RemoveDestroyedObjects( List<api_ygg.UfoTableData> ufoData )
	{
		for ( int i = ufoList.Count-1 ; i >= 0 ; i-- )
		{
			var obj = ufoList[ i ];
			// ID検索でヒットしなければ破壊して配列から除外
			bool del = ufoData.Find( v => v.ufo_id.Equals( obj.GetId() ) ) == null;
			if( del )
			{
				obj.data.c_physical = 0;
				ufoList.RemoveAt( i );
			}
		}
	}

	/// <summary>
	/// UFOのARオブジェクトをセットする
	/// 座標変換とオブジェクト生成を分離
	/// </summary>
	private void SetUfoObject( api_ygg.UfoTableData ufoData )
	{
		// UFOの座標を指定
		var ufoPos = new GpsPosition( ufoData.u_point_x_10, ufoData.u_point_y_10, ufoData.height );
		Debug.Log( "目標のGPS座標は ( long" + ufoPos.longitude.ToString() + ", lat" + ufoPos.latitude.ToString() + ")" );

		var tagStr = "ID=" + ufoData.ufo_id;
		if( !string.IsNullOrEmpty( ufoData.areaName ) )
		{
			tagStr += "[ " + ufoData.areaName + " ]";
		}

		// 自分の位置からの相対座標を取得 いきなりしたのベクターにしても良いかも？
		GpsPosition targetRelativePos = GetRelativePosition( ufoPos );
		Debug.Log( tagStr + "までの相対座標は ( long" + targetRelativePos.longitude.ToString() + ", lat" + targetRelativePos.latitude.ToString() + ")" );


		// GPSの座標をベクターに変換
		Vector3 vTargetRelativePos = new Vector3( ( float )targetRelativePos.longitude, ( float )targetRelativePos.altitude, ( float )targetRelativePos.latitude );
		Debug.Log( tagStr + "までの相対座標(ベクター)は (x" + vTargetRelativePos.x.ToString() + ", z" + vTargetRelativePos.z.ToString() + ")" );

		// マイクロ表示のときは 1/1000
		if( microVision )
		{
			//	vTargetRelativePos *= 0.001f;
			vTargetRelativePos *= 0.1f;
		}

		// 距離（メートル）を求める
		float fDistance = Mathf.Abs( ( new Vector2( vTargetRelativePos.x, vTargetRelativePos.z ) ).magnitude );
		Debug.Log( vTargetRelativePos.x.ToString() + ", " + vTargetRelativePos.z.ToString() + "の距離=" + fDistance.ToString() );

		// 同一のIDのUFOが存在するか調べる
		var ufo = ufoList.Find( v => v.GetId().Equals( ufoData.ufo_id ) );

		// 5km以上離れているオブジェクトは表示しない 生成済みだったら削除
		if( fDistance > 5000 && !microVision )
		{
			Debug.Log( tagStr + "への距離 " + fDistance.ToString() + "は範囲外なので生成しない" );
			
			if( ufo != null )
			{
				Debug.Log( "生成済みで範囲外になったUFOを削除" );

				Destroy( ufo.gameObject );
				ufoList.Remove( ufo );
			}

			return;
		}


		// 仮: 壊れているUFOは生成しない
		bool destroyed = ufoData.dest_flg == 3;
		if( destroyed )
		{ 
			Debug.Log( tagStr + "は破壊済みなので生成しない" );
			return;
		}

		if( ufo == null )
		{
			// 見つからなければ新しく生成してリストに追加
			ufo = Instantiate( arObject ).GetComponent<ArUfoObject>();
			ufo.gameObject.name = "UFO[ " + tagStr + " ]";
			ufoList.Add( ufo );
		}

		// パラメータ更新
		ufo.SetParameter( ufoData, tagStr );

		// 位置をセット
		// TODO: 将来的にはUFOのベースのポジションはセットし、モデルを個要素として表現する
		ufo.transform.position = vTargetRelativePos;
		ufo.transform.localScale = Vector3.one;

		// 
		
	}

	/// <summary>
	/// GPS座標を元に自身から見た相対座標を返す
	/// </summary>
	private GpsPosition GetRelativePosition( GpsPosition targetGpsPos )
	{
		// 自分のGPS座標
		GpsPosition myPos = PlayerMove.instance.currentGpsLocation;
		Debug.Log( "自分のGPS座標は ( long" + myPos.longitude.ToString() + ", lati" + myPos.latitude.ToString() + ")");

		// 自分の位置からの相対座標を取得
		GpsPosition pos_rel = GpsPosition.GetDistance( targetGpsPos, myPos );

		return pos_rel;
	}

	private void Update()
	{
		ufoCount.text = "UFO x [ " + ufoList.Count.ToString() + " ]";
	}

}
