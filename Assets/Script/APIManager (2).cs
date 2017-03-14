using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// API関連
/// </summary>
namespace api_ygg
{
	/// <summary>
	/// 定数定義
	/// </summary>
	public static class Const
	{
		/// <summary>
		/// リトライ間隔
		/// </summary>
		public const int  RETRY_INTERVAL = 3;

		/// <summary>
		/// 最大リトライ回数
		/// </summary>
		public const int  RETRY_NUM = 3;

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
		public const string fqdn = "ec2-35-166-84-86.us-west-2.compute.amazonaws.com";
	}

	/// <summary>
	/// APIとの通信を行う
	/// </summary>
	public class APIManager : MonoBehaviour {
		public static APIManager instance;

		void Awake(){
			instance = this;
		}

		/// <summary>
		/// UFO状態を取得
		/// </summary>
		public IEnumerator GetUfoInfo( System.Action<GetUfoInfoResponse> callback, GetUfoInfoRequest request )
		{
			if( YggdraSceneManager.instance != null && YggdraSceneManager.instance.demo )
			{
				callback( GetDummyUfoInfo() );
				yield break;
			}

			// API 呼び出し
			yield return StartCoroutine( Call<GetUfoInfoRequest, GetUfoInfoResponse >( request, res =>  
				{					
					callback(res);			
				}) );
		}

		/// <summary>
		/// UFO状態ダミーを作成して返却
		/// デモ用
		/// </summary>
		public GetUfoInfoResponse GetDummyUfoInfo()
		{
			var ret = new GetUfoInfoResponse();
			ret.ufoposlist = new List<UfoTableData>();

			var baseIdx = 0;

			// ダミーパラメータを設定
			var areaPrefix = "Austin";
			for( int idx = 1 ; ; idx++, baseIdx++ )
			{
				var areaName = areaPrefix + idx.ToString();
				if( !ARObjectSetter.sampleData.ContainsKey( areaName ) )
					break;

				var posData = ARObjectSetter.sampleData[areaName];

				var data = new UfoTableData( posData );
				data.t_physical = 10000;
				data.c_physical = 8000;
				data.ufo_id = baseIdx + idx;
				data.areaName = areaName;

				ret.ufoposlist.Add( data );
			}

			areaPrefix = "KidsPlate";
			for( int idx = 1 ; ; idx++, baseIdx++ )
			{
				var areaName = areaPrefix + idx.ToString();
				if( !ARObjectSetter.sampleData.ContainsKey( areaName ) )
					break;

				var posData = ARObjectSetter.sampleData[areaName];

				var data = new UfoTableData( posData );
				data.t_physical = 10000;
				data.c_physical = 8000;
				data.ufo_id = baseIdx + idx;
				data.areaName = areaName;

				ret.ufoposlist.Add( data );
			}

			return ret;
		}

		/// <summary>
		/// UFO体力を減少させる
		/// </summary>
		public IEnumerator AttackUfo( List<AttackUfoRequest> list, System.Action<GetUfoInfoResponse> callback )
		{
			if( YggdraSceneManager.instance != null && YggdraSceneManager.instance.demo )
			{
				// デモの時はローカルで完結
				yield break;
			}

			var request = new AttackUfoRequest();
			var gpsLoc = PlayerMove.instance.currentGpsLocation;
			request.x = (double)gpsLoc.longitude;
			request.y = (double)gpsLoc.latitude;
			request.type = 1;
			request.u_rand = YggdraSceneManager.instance.useGatheredData;


			// TODO: 本来は全て渡すが、現状１つしか渡せないので１つめを渡す
			request.damage = list[0].damage;
			request.ufo_id = list[0].ufo_id;

			// API 呼び出し
			yield return StartCoroutine( Call<AttackUfoRequest, GetUfoInfoResponse>( request, res =>  
				{
					callback(res);		
				}) );
		}
				
		/// <summary>
		/// 最新のチャット情報を取得
		/// </summary>
		public IEnumerator UpdateChatInfo( System.Action< List<ChatData> > callback, UpdateChatRequest request )
		{
			if( YggdraSceneManager.instance != null && YggdraSceneManager.instance.demo )
			{
				callback( CreateDummyChatInfo() );
				yield break;
			}

			yield return Call<UpdateChatRequest, UpdateChatResponse>( request, res =>
				{
					callback( res.stamps );
				});
		}

		/// <summary>
		/// ダミーチャット情報を生成
		/// </summary>
		private List<ChatData> CreateDummyChatInfo()
		{
			var ret = new List<ChatData>();

			// ダミーチャット情報生成
			do
			{
				var chat = new ChatData();
				chat.u_point_x = Random.Range(-20f, 20f);
				chat.u_point_y = Random.Range(-10f, 10f);

				chat.emotion= Random.Range(0, 4);

				ret.Add( chat );

			}while(!Random.Range(0,10).Equals(0));

			return ret;
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
			ret = ret.Trim( new char[]{'{', '}'} );

			// なぜかTrimで " が残るのでReplaceを使う
			ret = ret.Replace("\"", "");

			// ":"を"="で置き換え
			ret = ret.Replace(':', '=');
			// ","を"&"で置き換え
			ret = ret.Replace(',', '&');

			// 先頭に"&"をつける
			ret = "&" + ret;

			return ret;
		}

		private IEnumerator Call<ReqType, ResType>( ReqType req, System.Action<ResType> callback = null )
			where ReqType : class, RequestParamInterface, new()
			where ResType : class, ResponseParamInterface
		{
			// リクエストがなければ作成
			if (req == null) { req = new ReqType(); }			

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
			try{
				res = JsonUtility.FromJson<ResType>( www.text );
			}
			catch( System.Exception e) 
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
	/// UFO情報取得リクエスト
	/// </summary>
	public class GetUfoInfoRequest : RequestParamInterface
	{
		public virtual string ApiName
		{
			get{
				return "action_getpos=true&output=json";
			}
		}

		/// <summary>
		/// 自分の周辺いにデータを集めた状態で返すフラグ
		/// </summary>
		public bool u_rand;

		/// <summary>
		/// 自分のx座標
		/// = 経度
		/// </summary>
		public double x;

		/// <summary>
		/// 自分のy座標
		/// = 高度
		/// </summary>
		public double y;

		/// <summary>
		/// 自分のz座標
		/// </summary>
		public double z;

		/// <summary>
		/// APIタイプ 返却値の構造が変わる
		/// 1=ufoposlist以下にUFO情報配列
		/// 1以外=直接UFO情報配列
		/// </summary>
		public int type;
	}

	/// <summary>
	/// UFO情報取得レスポンス
	/// </summary>
	[System.Serializable]
	public class  GetUfoInfoResponse : ResponseParamInterface
	{
		public List<UfoTableData> ufoposlist;
	}

	/// <summary>
	/// UFOHP減少リクエスト
	/// </summary>
	public class AttackUfoRequest : GetUfoInfoRequest
	{
		public override string ApiName{
			get{
				return "action_attack=true&output=json";
			}
		}

		/// <summary>
		/// 攻撃したUFOのID
		/// </summary>
		public int ufo_id;

		/// <summary>
		/// 与えたダメージ
		/// </summary>
		public int damage;
	}

	/// <summary>
	/// UFOHP減少レスポンス
	/// </summary>
	public class  AttackUfoResponse : ResponseParamInterface
	{

	}

	/// <summary>
	/// ログインリクエスト
	/// </summary>
	public class LoginRequest : RequestParamInterface
	{
		public string ApiName
		{
			get{
				return "/yggdrasill/?action_logout=true";
			}
		}
		/*
		public override List<string> param
		{
			get{
				return new List<string>(
					"uid", 
					"pw"
				);
			}
		}
		*/
	}

	/// <summary>
	/// ログインレスポンス
	/// </summary>
	public class LoginResponse
	{
		/// <summary>
		/// 英数ランダム値
		/// </summary>
		public string cookie = "SessionID";

		public string host = "xxxx.xxxx.xxxx.net";
	}

	/// <summary>
	/// ログアウトリクエスト
	/// </summary>
	public class LogoutRequest : RequestParamInterface
	{
		public string ApiName
		{
			get{
				return "/yggdrasill/?action_logout=true";
			}
		}
		/*
		public override List<string> param
		{
			get{
				return new List<string>(
					"logout=1" 
				);
			}
		}
		*/
	}

	/// <summary>
	/// チャット情報取得リクエスト
	/// emotionに値を入れると取得時に送信も可能
	/// </summary>
	public class UpdateChatRequest : RequestParamInterface
	{
		/// <summary>
		/// チームID
		/// </summary>
		public int t_id;

		/// <summary>
		/// ユーザーID
		/// 設定しないときは1扱い
		/// </summary>
		public int u_id;

		/// <summary>
		/// 周りに集めるフラグ
		/// </summary>
		public bool u_rand;

		/// <summary>
		/// 感情タイプ
		/// 0=喜 1=怒 2=哀 3=楽
		/// ■ この値をセットしない場合は受信のみ ■
		/// </summary>
		public int emotion;

		/// <summary>
		/// 自分のx座標
		/// </summary>
		public double x;

		/// <summary>
		/// 自分のy座標
		/// </summary>
		public double y;

		/// <summary>
		/// オプション値
		/// type=1 --- 登録されているchat情報を全件取得します
		/// type=2 --- 最後にchatを取得した時間以降のchatだけ取得します
		/// type=3 --- 登録されているchat情報を全件取得しますが、emotion毎にカウントされたデータが返ります（cパラメータ追加）
		/// type=4 --- 最後にchatを取得した時間以降のchatだけ取得しますが、emotion毎にカウントされたデータが返ります（cパラメータ追加）
		/// </summary>
		public int type;

		public string ApiName {
			get {
				return "action_chat=true&output=json";
			}
		}

		// &emotion=2&t_id=1&u_id=2&u_rand=true&type=3
	}

	/// <summary>
	/// チャット情報更新レスポンス
	/// </summary>
	public class UpdateChatResponse : ResponseParamInterface 
	{
		/// <summary>
		/// チャット情報リスト
		/// </summary>
		public List<ChatData> stamps;
	}
}