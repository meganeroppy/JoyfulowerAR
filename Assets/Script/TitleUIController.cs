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
		if( YggdraSceneManager.instance == null )
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
		if( YggdraSceneManager.instance == null )
		{
			return;
		}

		YggdraSceneManager.instance.demo = true;

		StartLoginProcess();
	}

	private void StartLoginProcess()
	{
		YggdraSceneManager.instance.StartLoginProcess();
	}
}
