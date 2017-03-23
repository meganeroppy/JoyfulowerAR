using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// マップの操作テスト
/// </summary>
public class MapController : MonoBehaviour 
{

	[SerializeField]
	OnlineMaps map = null;

	// Use this for initialization
	void Start () {
	//	map.zoom = 20;

//		map.SetPosition( longitude, latitude );

	
		OnlineMapsMarker marker;
	//	marker.
	//	map.AddMarker()
	}
	
	/// <summary>
	/// マップ上のマーカーを更新
	/// </summary>
	public void UpdateMarker ( List<api.TweetInfo> tweetInfo, List<api.BloomPointInfo> bloomPointInfo )
	{
		Debug.Log("マーカー更新");
		UpdateFlower( tweetInfo );
	}

	/// <summary>
	/// 花マーカーを更新
	/// </summary>
	private void UpdateFlower( List<api.TweetInfo> tweetInfo )
	{
		
		OnlineMapsMarker marker = new OnlineMapsMarker();

	}
}
