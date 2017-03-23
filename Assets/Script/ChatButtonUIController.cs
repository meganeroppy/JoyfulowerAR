using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// チャットボタン関連制御
/// </summary>
public class ChatButtonUIController : EventTrigger 
{
	[SerializeField]
	private RectTransform emoteButtonGroup = null;

	private bool dispEmoteIcons = false;

	/// <summary>
	/// エモートアイコンが並んでいる時間
	/// </summary>
	private float dispDurarion = 2f;
	private float displTimer = 0;

	float showLocalX = 400;
	float hideLocalX = 540f;


	private void Update()
	{
		UpdateAppearStatus();
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
		else if( dispEmoteIcons )
		{
			dispEmoteIcons = false;
			ExpEmoteIcon( dispEmoteIcons );
		}
	}

	/// <summary>
	/// エモートボタンの演出
	/// </summary>
	private void ExpEmoteIcon(bool animIn)
	{
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
	}

	private Vector2 dragBeginPos;

	public override void OnBeginDrag( PointerEventData data )
	{
		Debug.Log("OnBeginaDrag");
		dragBeginPos = data.position;
	}
	public void OnBeginDrag( )
	{
		Debug.Log("OnBeginaDrag");

		displTimer = dispDurarion;

		if( !dispEmoteIcons )
		{
			dispEmoteIcons = true;
			ExpEmoteIcon( dispEmoteIcons );
		}
	}

	/// <summary>
	/// ボタンの範囲外に抜けた時呼ばれる
	/// </summary>
	public override void OnPointerExit( PointerEventData data )
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
			if( !dispEmoteIcons )
			{
				dispEmoteIcons = true;
				ExpEmoteIcon( dispEmoteIcons );
			}
		}
	}
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
