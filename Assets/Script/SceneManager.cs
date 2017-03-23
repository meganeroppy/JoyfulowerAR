using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JfSceneManager : MonoBehaviour 
{
	/// <summary>
	/// 実体
	/// </summary>
	public static JfSceneManager instance;

	/// <summary>
	/// シーンタイプ定義
	/// </summary>
	public enum SceneType
	{
		Explore,	// 散策
		Bouquet,	// 花束作成
	}

	/// <summary>
	/// 現在のシーンタイプ
	/// </summary>
	[HideInInspector]
	public SceneType sceneType;

	/// <summary>
	/// ツイート情報取得間隔
	/// </summary>
	private const float update_interval = 8f;

	/// <summary>
	/// APIマネージャ
	/// </summary>
	[SerializeField]
	private api.APIManager api = null;

	/// <summary>
	/// 取得した花の構造体
	/// </summary>
	public class FlowerItem
	{
		/// <summary>
		/// ゲームオブジェクト名
		/// </summary>
		public string name;

		/// <summary>
		/// 所持数
		/// </summary>
		public int count;

		/// <summary>
		/// 花の種類
		/// </summary>
		public FlowerBase.FlowerType flowerType;
	}

	public List<FlowerItem> fList;

	/// <summary>
	/// データベースからの値の取得をシュミュレートするか？
	/// ( = 実際にデータの取得は行わなくする)
	/// </summary>
	[SerializeField]
	private bool _simulateGetFromDatabase = false;
	public bool simulateGetFromDatabase { get { return _simulateGetFromDatabase; } }

	// Use this for initialization
	void Start () 
	{
		instance = this;

		// デフォルトは散策モード
		sceneType = SceneType.Explore;

		// 花アイテムリストを初期化
		fList = new List<FlowerItem>();

		if( api == null )
		{
			Debug.LogError("APIマネージャが未定義");
			return;
		}

		// 一定周期でツィートを情報を取得
		StartCoroutine( WaitAndGetTweetInfo() );
	}

	/// <summary>
	/// マップ制御系
	/// </summary>
	[SerializeField]
	MapController map = null;

	/// <summary>
	/// 一定間隔でツイート情報の更新を行う
	/// </summary>
	IEnumerator WaitAndGetTweetInfo()
	{
		//Debug.Log("ツイート情報取得中");
		float timer = 0;

		while( true )
		{
			// データベースからデータを取得する
			yield return StartCoroutine( api.GetTweetInfo( res =>
			{
				Debug.Log( "APIの返却値で花の生成を行います" );

				// マップを更新
				map.UpdateMarker( res.tweetInfoList, res.bloomPointInfoList );
			} ) );

			// 一定時間待機
			while( timer < update_interval )
			{
				yield return null;
				timer += Time.deltaTime;
			}
		}
	}
}
