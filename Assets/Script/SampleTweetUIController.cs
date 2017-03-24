using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// Tweetテスト送信ボタン関連制御
/// </summary>
public class SampleTweetUIController : MonoBehaviour 
{
//	[SerializeField]
//	private RectTransform tweetButtonGroup = null;

	private bool dispTweetIcons = false;

	/// <summary>
	/// 送信処理中か？
	/// </summary>
	private bool processing = false;

	/// <summary>
	/// エモートアイコンが並んでいる時間
	/// </summary>
	private float dispDurarion = 2f;
	private float displTimer = 0;

//	float showLocalX = 400;
//	float hideLocalX = 540f;

	public void OnClickSendSampleTweetButton( int feeling )
	{ 
		// 処理中は何もしない
		if( processing )
		{
			return;
		}

		StartCoroutine( SendSampleTweet( feeling ) );
	}

	private IEnumerator SendSampleTweet( int feeling )
	{
		// 処理中フラグを立てる
		processing = true;

		// API送信
		yield return null;

		// TODO: 送信完了をアナウンス
		AnnounceCompleteSendSampleTweet( feeling );

		// TODO: マーカーを更新

		// 処理中フラグを下ろす
		processing = false;
	}

	/// <summary>
	/// 送信完了を通知
	/// </summary>
	private void AnnounceCompleteSendSampleTweet( int feeling )
	{
		// TODO: 演出
		Debug.Log( "[ " + feeling.ToString() + " ] の送信に成功" );
	}

	private void Update()
	{
	//	UpdateAppearStatus();
	}

	/// <summary>
	/// 一定時間表示していたら隠す
	/// </summary>
	private void UpdateAppearStatus()
	{
		if( displTimer > 0 )
		{
			displTimer -= Time.deltaTime;
		}
		else if( dispTweetIcons )
		{
			dispTweetIcons = false;
			ExpEmoteIcon( dispTweetIcons );
		}
	}

	/// <summary>
	/// ツィートボタン表示時・非表示切り替え時の演出
	/// </summary>
	private void ExpEmoteIcon(bool animIn)
	{
		/*
		if( animIn )
		{
			// 表示
			emoteButtonGroup.gameObject.SetActive( true );
			var goal = showLocalX;

			emoteButtonGroup.localPosition = new Vector2( hideLocalX, emoteButtonGroup.localPosition.y);
			emoteButtonGroup.DOLocalMoveX( goal, .5f );
		}
		else
		{
			// 非表示
			var goal = hideLocalX;
			emoteButtonGroup.localPosition = new Vector2( showLocalX, emoteButtonGroup.localPosition.y);
			emoteButtonGroup.DOLocalMoveX( goal, .5f ).OnComplete( () =>
				{
					emoteButtonGroup.gameObject.SetActive(false);
				});
		}
		 */
	}

	/// <summary>
	/// ドラッグ開始座標
	/// </summary>
	private Vector2 dragBeginPos;

	/// <summary>
	/// ドラッグ開始されたときのイベント
	/// </summary>
	public void OnBeginDrag( PointerEventData data )
	{
		Debug.Log("OnBeginaDrag");
		dragBeginPos = data.position;
	}

	/// <summary>
	/// ドラッグ開始されたときのイベント
	/// </summary>
	public void OnBeginDrag( )
	{
		Debug.Log("OnBeginaDrag");

		displTimer = dispDurarion;

		if( !dispTweetIcons )
		{
			dispTweetIcons = true;
			ExpEmoteIcon( dispTweetIcons );
		}
	}

	/// <summary>
	/// ボタンの範囲外に抜けた時呼ばれる
	/// </summary>
	public void OnPointerExit( PointerEventData data )
	{
		Debug.Log("OnPointerExit");

		if( !data.dragging )
		{
			// ドラッグ中以外はなにもしない
			return;
		}
			
		// 上方向に対するドラッグのみ有効
		if( data.position.y > dragBeginPos.y )
		{
			if( !dispTweetIcons )
			{
				dispTweetIcons = true;
				ExpEmoteIcon( dispTweetIcons );
			}
		}
	}

	/// <summary>
	/// タッチ範囲から抜けたときのイベント
	/// </summary>
	public void OnPointerExit( )
	{
		Debug.Log("OnPointerExit");
	}

	/// <summary>
	/// Jsonパース実験関数
	/// 押しやすいのでここに書いておく
	/// </summary>
	private void JsonParseTest()
	{
		var jsonData = new api_ygg.GetUfoInfoResponse();
		jsonData.ufoposlist = new List<api_ygg.UfoTableData>();

		for(int i=0 ;i<3 ; i++)
		{
			var data = new api_ygg.UfoTableData();
			data.areaName = "name" + i.ToString();
			data.ufo_id = i;
			jsonData.ufoposlist.Add( data );
		}

		var jsonStr = JsonUtility.ToJson( jsonData );

		Debug.Log( jsonStr );
	}
}
