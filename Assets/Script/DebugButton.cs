using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugButton : MonoBehaviour {

	/// <summary>
	/// デバッグ用UI
	/// </summary>
	[SerializeField]
	private GameObject debugUiGroup;

	/// <summary>
	/// デバッグ用グリッドキューブ
	/// </summary>
	[SerializeField]
	private GameObject gridCube;

	[SerializeField]
	private bool activeInDefault = false;
	private bool active;

	void Awake()
	{
		active = activeInDefault;

		SetEnable();
	}

	[SerializeField]
	private Text gatherModeButtonLabel;

	private void Update()
	{
		UpdateGatherModeButtonLabel();
	}

	private void UpdateGatherModeButtonLabel()
	{
		if( JoyfulowerSceneManager.instance == null )
		{
			return;
		}

		gatherModeButtonLabel.text = JoyfulowerSceneManager.instance.useGatheredData ? "ON" : "OFF";
	}

	/// <summary>
	/// ボタンが押された
	/// </summary>
	public  void Press()
	{
		active = !active;
		SetEnable();
	}

	/// <summary>
	/// 表示切り替え
	/// </summary>
	private void SetEnable()
	{
		// デバッグ用UI表示切り替え
		debugUiGroup.SetActive( active );

		// グリッドキューブ表示切り替え
		gridCube.SetActive( active );
	}

	/// <summary>
	/// データ集結フラグ変更ボタン
	/// </summary>
	public void OnClickSwitchGatheringButton()
	{
		JoyfulowerSceneManager.instance.useGatheredData = !JoyfulowerSceneManager.instance.useGatheredData;
	}

	/// <summary>
	/// 自分の基準位置をセット
	/// </summary>
	public void OnClickSetMyPlaceButton(string placeName)
	{
		if( !ARObjectSetter.sampleData.ContainsKey(placeName) )
		{
			Debug.LogError( placeName + " のキーは存在しない");
			return;
		}

		GPSLoader.instance.myPosKey = placeName;
	}
}
