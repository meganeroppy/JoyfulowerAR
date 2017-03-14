using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class LoadingSceneUIController : MonoBehaviour {

	public static LoadingSceneUIController instance;

	/// <summary>
	/// Now Loading...のテキスト
	/// </summary>
	[SerializeField]
	private Text loadingText;

	/// <summary>
	/// 背景画像
	/// </summary>
	[SerializeField]
	private Image bgImage;

	[SerializeField]
	private float fadeSpeed = 1f;

	public int MaxDotNum = 5;

	private void Awake()
	{
		instance = this;
	}

	private void Start () 
	{
		StartCoroutine( FadeIn() );
		StartCoroutine( AnimText() );
	}


	/// <summary>
	/// フェードイン
	/// </summary>
	private IEnumerator FadeIn()
	{
		if( bgImage == null )
		{
			yield break;
		}

		var alpha = 0f;
		Color bgColor;

		var originColor = bgImage.color;

		do
		{
			bgColor = new Color( originColor.r, originColor.g, originColor.b, alpha );
			bgImage.color = bgColor;
			alpha += fadeSpeed * Time.deltaTime;

			yield return null;
		} while( alpha < 1f );
	}

	/// <summary>
	/// フェードアウト
	/// </summary>
	public void FadeOut(System.Action callback=null)
	{
		StartCoroutine( ExecFadeOut( callback ) );
	}

	/// <summary>
	/// フェードアウト実行
	/// </summary>
	private IEnumerator ExecFadeOut( System.Action callback = null )
	{
		if( bgImage == null )
		{
			yield break;
		}

		var alpha = 1f;
		Color bgColor;
		var originColor = bgImage.color;

		do
		{
			bgColor = new Color( originColor.r, originColor.g, originColor.b, alpha );
			bgImage.color = bgColor;
			alpha -= fadeSpeed * Time.deltaTime;

			yield return null;
		} while( alpha > 0 );

		if( callback != null )
		{
			callback();
		}
	}

	/// <summary>
	/// ローディング中文字列のアニメーション
	/// </summary>
	private IEnumerator AnimText()
	{
		if( loadingText == null )
		{
			Debug.LogError( "ローディングテキストがnull" );
			yield break;
		}

		var baseText = "Now Loading";
		var dot = ".";
		int dotNum = 0;
		string str;

		while( true )
		{
			str = baseText;
			dotNum = dotNum >= MaxDotNum ? 0 : dotNum+1;

			for(int i = 0 ; i < dotNum ; i++)
			{
				str += dot;
			}

			loadingText.text = str;

			yield return new WaitForSeconds( 1 );
		}
	}
}
