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
		UpdateFlower( tweetInfo );
	}

	/// <summary>
	/// 花マーカーを更新
	/// </summary>
	private void UpdateFlower( List<api.TweetInfo> tweetInfo )
	{
		// 感情データが"A" ～ "F"でセットされているので整数値に変換する
		string alp = "ABCDEF";
		List<OnlineMapsMarker> markers = new List<OnlineMapsMarker>();

		foreach( api.TweetInfo t in tweetInfo )
		{
			// 生成
			var m = new OnlineMapsMarker();
			
			// 位置をセット
			m.SetPosition( t.gps.longitude, t.gps.latitude );
			
			// テクスチャをセット
			var texIdx = alp.IndexOf( t.felling ) % flowerTex.Count;
			m.texture = flowerTex[ texIdx ];

			// 初期化
			m.Init();

			markers.Add( m );
		}

		// マップに追加
		// TODO: 3Dマーカーできたらかっこいい
		map.AddMarkers( markers.ToArray() );

	}
}
