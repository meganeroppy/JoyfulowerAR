using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// ワールド座標のチャットオブジェクトをスクリーン座標に変換してUIとして表示を行う
/// </summary>
public class UIFollowTarget : MonoBehaviour 
{
	RectTransform rectTransform = null;
	[SerializeField] Transform target = null;

	[SerializeField]
	Canvas canvas;

	void Awake()
	{
		rectTransform = GetComponent<RectTransform> ();
		canvas = GetComponent<Graphic> ().canvas;
	}

	void Update ()
	{
		var pos = Vector2.zero;
		var uiCamera = Camera.main;
		var worldCamera = Camera.main;
		var canvasRect = canvas.GetComponent<RectTransform> ();

		var screenPos = RectTransformUtility.WorldToScreenPoint (worldCamera, target.position);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out pos);
		rectTransform.localPosition = pos;
	}
}