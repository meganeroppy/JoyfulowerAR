using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIGuageController : MonoBehaviour {

	/// <summary>
	/// ゲージのフィル部分
	/// </summary>
	[SerializeField]
	private Image fillSprite = null;

	/// <summary>
	/// ターゲット情報
	/// </summary>
	[SerializeField]
	private Text targetInfo = null;

	private void Update()
	{
		UpdateFillSprite();
	}

	private void UpdateFillSprite()
	{
		var target = PlayerMove.instance.currentTargetUfo;
		if( !target )
		{
			fillSprite.fillAmount = 0;
			targetInfo.text = "No Target";
			targetInfo.color = Color.gray;
			return;
		}

		var data = target.data;
		var parcent = (float)data.c_physical / data.t_physical;
		fillSprite.fillAmount = parcent;

		var str = string.Format("MotherShip ID[{0}] HP({1} / {2})", data.ufo_id, data.c_physical, data.t_physical);
		targetInfo.text = str;

		targetInfo.color = Color.black;
	}

}
