using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// ユグドラで使用したシーンマネージャ
/// </summary>
public class YggdraSceneManager : MonoBehaviour {

	public static YggdraSceneManager instance;
	
	/// <summary>
	/// デモフラグ
	/// 有効時はAPIから自分の位置やUFO情報を取得せず、きめうちの座標からスタートする。
	/// </summary>
	[System.NonSerialized]
	public bool demo = false;

	public class AccountInfo
	{
		public int id; // string?
	}
		
	private void Awake () 
	{
		instance = this;
	}

	public bool useGatheredData = false;

	private void Start()
	{
		// このシーンしかロードされていなかったらタイトルシーンをロード
		if( SceneManager.sceneCount <= 1 )
		{
			SceneManager.LoadScene("Title", LoadSceneMode.Additive);
		}
	}

	/// <summary>
	/// ログイン処理
	/// 将来を考えて一応用意したけどしばらく使わないかも
	/// </summary>
	public void StartLoginProcess()
	{
		StartCoroutine( ExecLoginProccess() );
	}
	public IEnumerator ExecLoginProccess()
	{
		// ローディングを表示
		// 削除はMainシーンからの通知を待つ
		SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);

		// アカウントが存在するかチェック
		AccountInfo accountInfo = null; 
		yield return StartCoroutine( CheckExistAccount( res =>
			{
				accountInfo = res;
			} ) );

		if ( accountInfo == null || accountInfo.id.Equals(0) )
		{
			Debug.Log("アカウントが存在しない  新規アカウントを作成します");

			yield return StartCoroutine( CreateNewAccount( res =>
				{
					accountInfo = res;
				} ) );

			if ( accountInfo == null || accountInfo.id.Equals(0) )
			{
				Debug.LogError("アカウント作成に失敗 ログイン処理を中断");
				yield break;
			}
		}

		// ログイン
		bool success = false;
		yield return StartCoroutine( Login( res =>
			{
				success = res;
			}, accountInfo ) );

		if( !success )
		{
			Debug.LogError("ログインに失敗 ログイン処理を中断");
			yield break;
		}

		// タイトルシーンを削除
		SceneManager.UnloadScene("Title");

		var sceneName = "Main";
		// メインシーンに遷移
		var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		while( !async.isDone )
			yield return null;

		var scene = SceneManager.GetSceneByName( sceneName );
		while ( !scene.isLoaded )
			yield return null;
		
		SceneManager.SetActiveScene(scene);

		// Mainシーンでの初回ロード完了時のイベントをセット
		ARObjectSetter.instance.initloadCompleteEvent = () =>
		{
			// ローディングシーンを削除
			if( LoadingSceneUIController.instance != null )
			{
				LoadingSceneUIController.instance.FadeOut( () =>
					{
						SceneManager.UnloadScene("LoadingScene");
					} );
			}
		};
	}
		
	/// <summary>
	/// アカウントが存在するかチェック
	/// </summary>
	private IEnumerator CheckExistAccount( System.Action<AccountInfo> callback )
	{
		var info = new AccountInfo();

		// TODO: アカウント情報を取得
		yield return null;
		info.id = 1;

		callback(info);
	}

	/// <summary>
	/// アカウントを新規作成
	/// </summary>
	private IEnumerator CreateNewAccount( System.Action<AccountInfo> callback )
	{
		var info = new AccountInfo();

		// TODO: アカウント作成
		yield return null;
		info.id = 1;

		callback(info);
	}

	/// <summary>
	/// ログイン
	/// </summary>
	private IEnumerator Login( System.Action<bool> callback, AccountInfo info )
	{
		// TODO: ログイン処理
		yield return new WaitForSeconds(3);
		var succeed = true;

		callback( succeed );
	}
}
