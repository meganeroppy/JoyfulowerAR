using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ARオブジェクト制御クラス
/// </summary>
public class ArUfoObject : MonoBehaviour 
{	
	public TextMesh mesh;

	/// <summary>
	/// ID
	/// </summary>
	public int GetId()
	{
		return myData.ufo_id;
	}

	/// <summary>
	/// ローカルで与えたダメージ
	/// </summary>
	public int damage{ private set; get; }

	/// <summary>
	/// 耐久値ゲージ
	/// </summary>
	[SerializeField]
	private Image HealthGauge = null;

	private string areaName;

	private api_ygg.UfoTableData myData;
	public api_ygg.UfoTableData data { get { return myData; } }

	/// <summary>
	/// パラメータをセット
	/// </summary>
	public void SetParameter( api_ygg.UfoTableData param, string areaName)
	{
		myData = param;
		this.areaName = areaName;

		// UFOの体力をセット
	//	ufo.HP = myData.c_physical;

		//　ゲージ更新
		UpdateGauge();
	}

	private bool displayedAppearLog = false;

	/// <summary>
	/// ラベル更新
	/// </summary>
	private void UpdateLabel()
	{
		string str = "";
		str += areaName + "ID= " +  myData.ufo_id.ToString() + " ( lon = " + myData.ew_latitude.ToString() + ", lat= " + myData.ns_latitude.ToString() + " ) { " + myData.c_physical.ToString() + " / " + myData.t_physical.ToString() + " } ";

		mesh.text = str;

		if( !displayedAppearLog )
		{
			Debug.Log( str );
			displayedAppearLog = true;
		}
	}

	/// <summary>
	/// 耐久値ゲージを更新
	/// </summary>
	private void UpdateGauge()
	{
		UpdateLabel();

		if( HealthGauge == null )
		{
			Debug.LogWarning( "耐久値ゲージが未定義" );
			return;
		}

		var percent = (float)myData.c_physical / myData.t_physical;

		HealthGauge.fillAmount = percent;
	}

	/// <summary>
	/// ダメージを適用
	/// </summary>
	public void ApplyDamage(int val)
	{
		damage += val;

		myData.c_physical -= val;

		UpdateGauge();
	}

	/// <summary>
	/// ローカルでダメージを0に
	/// </summary>
	public void ResetLocalDamge()
	{
		damage = 0;
	}

	private void Update()
	{
		if( HealthGauge != null )
		{
			HealthGauge.transform.LookAt( PlayerMove.instance.transform.position );
		}
	}
}
