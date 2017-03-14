using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// API関連
/// </summary>
namespace api_ygg
{
	/// <summary>
	/// テーブルデータ基底クラス
	/// </summary>
	[System.Serializable]
	public abstract class BaseTableData : ResponseParamInterface
	{
		/// <summary>
		/// レコード登録日時	
		/// timestamp	
		/// null NG				
		/// 初期値=CURRENT_TIMESTAMP
		/// </summary>
		public string created_at;

		/// <summary>
		/// レコード更新日時	
		/// timestamp	
		/// null NG
		/// 初期値=CURRENT_TIMESTAMP
		/// </summary>
		public string updated_at;

		/// <summary>
		/// 削除日時	
		/// timestamp
		/// null ok		
		/// 削除されていなければnull
		/// </summary>
		public string deleted_at;

		public string error;
	}

	/// <summary>
	/// マップ関連テーブルデータ基底クラス
	/// </summary>
	[System.Serializable]
	public abstract class BaseMapTableData : BaseTableData
	{
		/// <summary>
		/// マップID
		/// 桁数 12
		/// null NG
		/// </summary>
		public int m_id;

		/// <summary>
		/// 北緯南緯（ 北緯,南緯 ）
		/// 桁数 4000
		/// 北半球であれば北緯　
		/// </summary>
		public double ns_latitude;

		/// <summary>
		/// 東経西経（ 東経,西経 ）
		/// 桁数 4000
		/// 東京であれば東経　   
		/// </summary>
		public double ew_latitude;
	}

	/// <summary>
	/// ユーザーテーブル用データクラス
	/// 物理名 YG_USER_T
	/// 主キーはユーザID
	/// indexはチームID
	/// </summary>
	[System.Serializable]
	public class UserTableData : BaseTableData
	{ 
		/// <summary>
		/// ユーザID ゲームにログインするためのID
		/// 桁数 12
		/// null NG
		/// 初期値=auto
		/// </summary>
		public int u_id; 

		/// <summary>
		/// 所属チームＩＤ
		/// 桁数 12
		/// null OK			
		/// </summary>
		public int t_id;

		/// <summary>
		/// ゲームにログインするためのパスワード
		/// varchar
		/// 桁数 40	
		/// null NG			
		/// </summary>
		public string u_pw;

		/// <summary>
		/// アクセス権限	( 1 = 一般、2＝管理者、3＝特権 )
		/// varchar	
		/// 桁数 1
		/// null NG	
		/// </summary>
		public string auth_flg;
	}

	/// <summary>
	/// UFOテーブル用データクラス
	/// 物理名 YG_UFO_T
	/// 主キーはUFO_ID
	/// indexはチームID
	/// </summary>
	[System.Serializable]
	public class UfoTableData : BaseMapTableData
	{ 
		public UfoTableData(){}

		// GPS位置で生成するコンストラクタ
		public UfoTableData( GpsPosition pos )
		{
			ew_latitude = (double)pos.longitude;
			ns_latitude = (double)pos.latitude;

			u_point_x_10 = (double)pos.longitude;
			u_point_y_10 = (double)pos.latitude;
			height = (double)pos.altitude;
		}

		public string areaName;

		/// <summary>
		/// UFO_ID
		/// 桁数 12
		/// null NG
		/// 初期値 auto
		/// </summary>
		public int ufo_id;

		/// <summary>
		/// チームID 
		/// 桁数 12
		/// </summary>
		public int t_id;

		/// <summary>
		/// 座標(緯度・経度)
		/// 桁数 4000
		/// null NG
		/// </summary>
		public int coordinate;

		/// <summary>
		/// 高度（　m単位 ）
		/// 桁数 4000
		/// null NG
		/// </summary>
		public double height;

		/// <summary>
		/// 総体力
		/// 桁数 4000
		/// null NG
		/// 初期値 10000
		/// </summary>
		public int t_physical;

		/// <summary>
		/// 現在体力
		/// 桁数 4000
		/// null NG
		/// 初期値 t_physical値
		/// </summary>
		public int c_physical;

		/// <summary>
		/// 状態フラグ （1 = 待機、2＝行動中、3＝破壊）
		/// null NG
		/// 初期値 1
		/// </summary>
		public int dest_flg;

		public double u_point_x;

		/// <summary>
		/// 緯度っぽいけど経度が入っている？
		/// </summary>
		public double u_point_x_10;

		public double u_point_y;

		/// <summary>
		/// 経度っぽいけど緯度が入っている？
		/// </summary>
		public double u_point_y_10;
	}

	/// <summary>
	/// マップテーブルデータクラス
	/// YG_MAP_T
	/// 主キーはマップID
	/// </summary>
	[System.Serializable]
	public class MapTableData : BaseMapTableData
	{
		/// <summary>
		/// A座標(緯度・経度) 
		/// 桁数 4000
		/// </summary>
		public int a_coordinate;

		/// <summary>
		/// B座標(緯度・経度) 
		/// 桁数 4000
		/// </summary>
		public int b_coordinate;

		/// <summary>
		/// C座標(緯度・経度) 
		/// 桁数 4000
		/// </summary>
		public int c_coordinate;

		/// <summary>
		/// D座標(緯度・経度) 
		/// 桁数 4000
		/// </summary>
		public int d_coordinate;
	}

	/// <summary>
	/// チームテーブルデータクラス
	/// YG_TERM_T
	/// 主キーはチームID
	/// </summary>
	[System.Serializable]
	public class TeamTableData : BaseTableData
	{
		/// <summary>
		/// 所属チームID
		/// 桁数 12
		/// null NG
		/// 初期値 auto
		/// </summary>
		public int t_id;

		/// <summary>
		/// 管理者ID
		/// 桁数 12
		/// null NG
		/// </summary>
		public int u_id;

		/// <summary>
		/// チーム名称
		/// null NG
		/// 初期値 "dummy_name"
		/// </summary>
		public string t_name;
	}

	[System.Serializable]
	public class ChatData : ResponseParamInterface
	{ 
		/// <summary>
		/// 所属チームID
		/// 桁数 12
		/// null NG
		/// 初期値 auto
		/// </summary>
		public int t_id;

		/// <summary>
		/// ユーザーID
		/// 桁数 12
		/// null NG
		/// </summary>
		public int u_id;

		/// <summary>
		/// 感情タイプ
		/// 0=喜 1=怒 2=哀 3=楽
		/// </summary>
		public int emotion;

		/// <summary>
		/// 発信カウント
		/// </summary>
		public int c;

		/// <summary>
		/// 発信座標x
		/// </summary>
		public double u_point_x;

		/// <summary>
		/// 発信座標y
		/// </summary>
		public double u_point_y;

		/// <summary>
		/// 発信座標x
		/// </summary>
		public double u_point_x_10;

		/// <summary>
		/// 発信座標y
		/// </summary>
		public double u_point_y_10;
	}

}