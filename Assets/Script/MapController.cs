using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// マップの操作テスト
/// </summary>
public class MapController : MonoBehaviour 
{
	/// <summary>
	/// マップ
	/// </summary>
	[SerializeField]
	private OnlineMaps map = null;

	/// <summary>
	/// 花マーカーのテクスチャ
	/// </summary>
	[SerializeField]
	private List<Texture2D> flowerTex = null;

	/// <summary>
	/// ブルーム地点のテクスチャ
	/// </summary>
	[SerializeField]
	private Texture2D bloomPointTex = null;


	// Use this for initialization
	void Start () 
	{
		// マップの拡大率を指定
	//	map.zoom = 20;
	//	map.AddMarker()
	}
	
	void Update()
	{
		// TODO: GPSから自分の位置を判断して中心にしたいな あと回転も
		//		map.SetPosition( longitude, latitude );
	}

	/// <summary>
	/// マップ上のマーカーを更新
	/// </summary>
	public void UpdateMarker ( List<api.TweetInfo> tweetInfo, List<api.BloomPointInfo> bloomPointInfo )
	{
		if( !map )
		{
			Debug.LogError( "mapがnull" );
			return;
		}

		Debug.Log("マーカー更新");
		UpdateBloomPoints( tweetInfo, bloomPointInfo );
	}

	/// <summary>
	/// ブルーム地点マーカーを更新
	/// </summary>
	private void UpdateBloomPoints( List<api.TweetInfo> tweetInfo, List<api.BloomPointInfo> bloomPointInfo )
	{
		List<OnlineMapsMarker> markers = new List<OnlineMapsMarker>();

		foreach( api.TweetInfo ti in tweetInfo )
		{
			// ツィートの幸福度の最寄ブルーム地点を取得
			var nearest = GetNearestBloomPoint( ti.gps, bloomPointInfo );

			// 幸福度を加算
			var feelingIdx = GetFeelingToInt( ti.felling );
			var feeling = new api.HappinessEnergy();
			feeling.type = ( FlowerBase.FlowerType )feelingIdx;
			feeling.count = 1;

			nearest.energy.Add( feeling );
		}

		// ブルーム地点セット
		foreach( api.BloomPointInfo b in bloomPointInfo )
		{
			// 生成
			var m = new OnlineMapsMarker();

			// 位置をセット
			m.SetPosition( b.gps.longitude, b.gps.latitude );

			// 条件を満たしていたら開花
			if( b.energy.Count >= b.energyToBloom )
			{
				//	var texIdx = GetFeelingToInt( t.felling );
				var texIdx = 0;
				m.texture = flowerTex[ texIdx ];

				// 初期化
				m.Init();
			}

			markers.Add( m );
		}

		// マップに追加
		// TODO: 3Dマーカーできたらかっこいい
		map.AddMarkers( markers.ToArray() );
	}

	/// <summary>
	/// 最寄りのブルーム地点を返す
	/// </summary>
	private api.BloomPointInfo GetNearestBloomPoint( api.GPS originPosition, List<api.BloomPointInfo> bloomPointInfo )
	{
		if( bloomPointInfo.Count == 0 )
		{
			Debug.LogWarning( "範囲内にブルーム地点なし" );
			return null;
		}

		int nearestIdx = 0;
		float nearestDist = float.MaxValue;

		var vOriginPos = new Vector3( (float)originPosition.longitude, (float)originPosition.latitude );

		for( int i = 0 ; i < bloomPointInfo.Count ; i++ )
		{
			var bp = bloomPointInfo[ i ];
			var vTargetPos = new Vector3( ( float )bp.gps.longitude, ( float )bp.gps.latitude );

			float distance = Mathf.Abs( ( vTargetPos - vOriginPos ).magnitude );
			if( distance < nearestDist )
			{
				nearestDist = distance;
				nearestIdx = i;
			}
		}

		return bloomPointInfo[ nearestIdx ];
	}

	/// <summary>
	/// 花マーカーを更新
	/// </summary>
	private void UpdateFlower( List<api.TweetInfo> tweetInfo )
	{
		List<OnlineMapsMarker> markers = new List<OnlineMapsMarker>();

		foreach( api.TweetInfo t in tweetInfo )
		{
			// 生成
			var m = new OnlineMapsMarker();
			
			// 位置をセット
			m.SetPosition( t.gps.longitude, t.gps.latitude );

			var texIdx = GetFeelingToInt( t.felling );
			m.texture = flowerTex[ texIdx ];

			// 初期化
			m.Init();

			markers.Add( m );
		}

		// マップに追加
		// TODO: 3Dマーカーできたらかっこいい
		map.AddMarkers( markers.ToArray() );
	}

	/// <summary>
	/// 幸福度文字列を整数値に変換
	/// </summary>
	/// <param name="feeling"></param>
	/// <returns></returns>
	private int GetFeelingToInt( string feeling )
	{
		// 感情データが"A" ～ "F"でセットされているので整数値に変換する
		string alp = "ABCDEF";

		// テクスチャをセット
		return alp.IndexOf( feeling ) % flowerTex.Count;
	}
}
