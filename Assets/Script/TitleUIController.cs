using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleUIController : MonoBehaviour 
{
	private void Start()
	{
		// ベースシーンがロードされていなかったらロードする
		var sceneName = "BaseScene";

		var baseScene = SceneManager.GetSceneByName(sceneName);
		if( !baseScene.isLoaded )
		{
			SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		}
	}

	/// <summary>
	/// スクリーンボタンをクリック
	/// </summary>
	public void OnClickScreen()
	{
		if( JoyfulowerSceneManager.instance == null )
		{
			return;
		}

		Debug.Log( "通常モードで開始" );

		StartLoginProcess();
	}

	/// <summary>
	/// デモボタンをクリック
	/// </summary>
	public void OnClickDemoButton()
	{
		if( JoyfulowerSceneManager.instance == null )
		{
			return;
		}

		JoyfulowerSceneManager.instance.demo = true;

		StartLoginProcess();
	}

	private void StartLoginProcess()
	{
		JoyfulowerSceneManager.instance.StartLoginProcess();
	}
}
