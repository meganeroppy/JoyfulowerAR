using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// API のパラメータ群
/// </summary>
namespace api_jf 
{
	/// <summary>
	/// 定数定義
	/// </summary>
	public static class Const
	{
		/// <summary>
		/// リトライ間隔
		/// </summary>
		public const int RETRY_INTERVAL = 3;

		/// <summary>
		/// 最大リトライ回数
		/// </summary>
		public const int RETRY_NUM = 3;

		/// <summary>
		/// メソッド種類
		/// </summary>
		public const string method = "POST";

		/// <summary>
		/// プロトコル種類
		/// </summary>
		public const string protocol = "http";

		/// <summary>
		/// FQDN (Fully Qualitied Domain Name)
		/// </summary>
	//	public const string fqdn = "ec2-35-166-84-86.us-west-2.compute.amazonaws.com";
		public const string fqdn = "www.triaws.com";
	}

	/// <summary>
	/// APIとの通信を行う
	/// </summary>
	public class APIManager : MonoBehaviour 
	{
		public static APIManager instance;

		void Awake(){
			instance = this;
		}

		/// <summary>
		/// サーバURL
		/// </summary>
		public static string serverUrl
		{
			get
			{
				return Const.protocol + "://" + Const.fqdn + "/";
			}
		}

		/// <summary>
		/// 呼び出し
		/// </summary>
		private IEnumerator Call<ReqType, ResType>( ReqType req, System.Action<ResType> callback = null )
			where ReqType : class, RequestParamInterface, new()
			where ResType : class, ResponseParamInterface
		{
			// リクエストがなければ作成
			if( req == null ) { req = new ReqType(); }

			// url作成
			string url = GetURL( req.ApiName );

			// 一旦Jsonに変換
			var json = JsonUtility.ToJson( req );

			// Json変換でパラメータがあればGetメソッド用パラメータに変換して最後に付ける
			if( !string.IsNullOrEmpty( json ) )
			{
				var getParam = MakeGetMethodParamFromJson( json );
				url += getParam;
			}

			Debug.Log( "通信開始 : " + url );

			WWW www = null;
			int retryCnt = Const.RETRY_NUM; //  リトライする回数
			float retryInterval = Const.RETRY_INTERVAL; // 一回リトライするまでの秒数

			bool succeed = false;
			for( int i = 0 ; i < retryCnt ; i++ )
			{
				www = new WWW( url );

				Debug.Log( "待機中 " + ( i + 1 ).ToString() + "回目 " + url );
				yield return www;

				if( !string.IsNullOrEmpty( www.error ) )
				{
					Debug.LogWarning( "通信エラー\n" + retryInterval.ToString() + "秒後にリトライします : " + url );
					yield return new WaitForSeconds( retryInterval );
				}
				else
				{
					succeed = true;
					break;
				}
			}

			if( !succeed )
			{
				Debug.LogWarning( "リトライを" + retryCnt.ToString() + "回行いましたが、正しい値を得られませんでした。" );
			}

			string str = succeed ? "成功" : "失敗";
			Debug.Log( str + " : " + url );

			ResType res = null;
			try
			{
				res = JsonUtility.FromJson<ResType>( www.text );
			}
			catch( System.Exception e )
			{
				Debug.LogError( e.ToString() );
			}

			if( res != null )
			{
				Debug.Log( "response = " + www.text );
				// 適切な値を返す
				callback( res );
			}
		}

		/// <summary>
		/// API名から接続先URLを取得する
		/// </summary>
		/// <param name="apiName"></param>
		public static string GetURL( string apiName )
		{
			return serverUrl + "?" + apiName;
		}

		/// <summary>
		/// JsonでGetメソッドのパラメータを作る
		/// </summary>
		private string MakeGetMethodParamFromJson( string json )
		{
			string ret = json;

			// 余分な文字を除く
			ret = ret.Trim( new char[] { '{', '}' } );

			// なぜかTrimで " が残るのでReplaceを使う
			ret = ret.Replace( "\"", "" );

			// ":"を"="で置き換え
			ret = ret.Replace( ':', '=' );
			// ","を"&"で置き換え
			ret = ret.Replace( ',', '&' );

			// 先頭に"&"をつける
			ret = "&" + ret;

			return ret;
		}

		/// <summary>
		/// CSVを構造体にパース
		/// </summary>
		/// <returns>The csv.</returns>
		/// <param name="csv">Csv.</param>
		/// <typeparam name="ResType">The 1st type parameter.</typeparam>
		private GetTweetInfoResponseParameter ParsefromCsv<ResType>(string csv)
			where ResType : GetTweetInfoResponseParameter
		{
			var param = new GetTweetInfoResponseParameter ();
			param.tweetInfoList = new List<GetTweetInfoResponseParameter.TweetInfo> ();

			// レコード単位で分割
			var records = csv.Split ('\n');
			for (int i = 0; i < records.Length; i++) 
			{
				var tInfo = new GetTweetInfoResponseParameter.TweetInfo ();

				var data = records [i].Split (',');

				if (data.Length < 7) 
				{
					continue;
				}
					
				tInfo.gps = new GPS ();
				double.TryParse (data [0], out tInfo.gps.latitude);
				double.TryParse (data [1], out tInfo.gps.longitude);
				double.TryParse (data [2], out tInfo.gps.altitude);
				tInfo.felling = data [3].ToString ();
				tInfo.comment = data [4].ToString ();
				DateTime.TryParse (data [5], out tInfo.date);
				tInfo.status = data [6].ToString ();

				param.tweetInfoList.Add ( tInfo );
			}

			return param;
		}

		/// <summary>
		/// ツイート情報を取得する
		/// </summary>
		public IEnumerator GetTweetInfo( System.Action< GetTweetInfoResponseParameter > callback )
		{
			if( JfSceneManager.instance.simulateGetFromDatabase )
			{
				var res = new GetTweetInfoResponseParameter();
			
				// データベースからの取得をシミュレート
				Debug.Log("APIの返却値での花生成をシミュレートします");
				var twtCnt = UnityEngine.Random.Range(6, 15);

				var alp = "ABCDEF";

				for( int i = 0 ; i < twtCnt ; i++ )
				{
					var info = new GetTweetInfoResponseParameter.TweetInfo();
					var idx = UnityEngine.Random.Range( 0, alp.Length );
					info.felling = alp[ idx ].ToString();

					res.tweetInfoList.Add( info );
				}

				callback( res );

				yield break;
			}


			var request = new GetTweetInfoRequestParameter();

			yield return StartCoroutine( Call<GetTweetInfoRequestParameter, GetTweetInfoResponseParameter>( request, res =>
			{
				callback( res );
			}) );
		}
	}

	/// <summary>
	/// リクエストパラメータにはこのインタフェースの継承が必要
	/// </summary>
	public interface RequestParamInterface
	{
		/// <summary>
		/// API名
		/// </summary>
		string ApiName
		{
			get;
		}
	}

	/// <summary>
	/// レスポンスパラメータにはこのインタフェースの継承が必要
	/// </summary>
	public interface ResponseParamInterface
	{
		/*
		/// <summary>
		/// レスポンスステータス
		/// </summary>
		ResponseParameterCode.Status Status
		{
			get;
#if UNITY_EDITOR
			set;
#endif
		}
		*/
	}

	/// <summary>
	/// GPS座標データ
	/// </summary>
	public struct GPS
	{
		/// <summary>
		/// 経度
		/// </summary>
		public double longitude;

		/// <summary>
		/// 緯度
		/// </summary>
		public double latitude;

		/// <summary>
		/// 高度
		/// </summary>
		public double altitude;
	}

	/// <summary>
	/// ツイート情報取得レスポンスパラメータ
	/// </summary>
	public class GetTweetInfoResponseParameter : ResponseParamInterface
	{
		/// <summary>
		/// つぶやき情報
		/// </summary>
		public struct TweetInfo
		{
			public int id;
			/// <summary>
			/// 位置情報
			/// </summary>
			public GPS gps;

			/// <summary>
			/// 幸福指数
			/// </summary>
			public string felling;

			/// <summary>
			/// コメント
			/// </summary>
			public string comment;

			/// <summary>
			/// つぶやかれた時間
			/// </summary>
			public DateTime date;

			public string status;
		}

		/// <summary>
		/// ブルームポイント情報
		/// </summary>
		public struct BloomPointInfo
		{
			/// <summary>
			/// 位置情報
			/// </summary>
			public GPS gps;

			/// <summary>
			/// 幸福指数
			/// </summary>
			public int happiness;
		}
			
		/// <summary>
		/// つぶやきリスト
		/// </summary>
		public List<TweetInfo> tweetInfoList;

		/// <summary>
		/// ブルームポイントリスト
		/// </summary>
		public List<BloomPointInfo> bloomPointInfoList;
	}

	/// <summary>
	/// ツイート情報取得リクエストパラメータ
	/// </summary>
	public  class GetTweetInfoRequestParameter : RequestParamInterface
	{
		/// <summary>
		/// 自分の座標
		/// </summary>
		//public GPS my_position;

		/// <summary>
		/// 取得する距離（メートル）
		/// </summary>
		//public float range;

		public virtual string ApiName
		{
			get
			{
				return "~joyfulower/keiji/prot/API001.php";
			}
		}

		/// <summary>
		/// URL
		/// </summary>
		//public string url = "http://www.triaws.com/~joyfulower/keiji/prot/API001.php";
	}
}
