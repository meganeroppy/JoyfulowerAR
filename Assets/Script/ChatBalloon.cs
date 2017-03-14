using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatBalloon : MonoBehaviour {

	[SerializeField]
	private Text text;

	public void Set(string text, Transform targetTransform, RectTransform canvasRect)
	{
		this.text.text = text;

		var uiCamera = Camera.main;
		var worldCamera = Camera.main;

		var screenPos = RectTransformUtility.WorldToScreenPoint (worldCamera, targetTransform.position);

		var pos = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out pos);
		GetComponent<RectTransform> ().localPosition = pos;
	}
}
