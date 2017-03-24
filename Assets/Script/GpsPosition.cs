using UnityEngine;
using System.Collections;

/// <summary>
/// 位置情報管理クラス
/// </summary>
public class GpsPosition
{
	public decimal latitude;
	public decimal longitude;
	public decimal altitude;
	public GpsPosition( decimal latitude, decimal longitude, decimal altitude )
	{
		this.latitude = latitude;
		this.longitude = longitude;
		this.altitude = altitude;		
	}
	public GpsPosition( double latitude, double longitude, double altitude )
	{
		this.latitude = (decimal)latitude;
		this.longitude = (decimal)longitude;
		this.altitude = (decimal)altitude;		
	}

	/// <summary>
	/// LocationInfo から GpsPositionに変換
	/// </summary>
	public static GpsPosition Loc2Gps( LocationInfo loc )
	{
		return new GpsPosition( ( decimal )loc.latitude, ( decimal )loc.longitude, ( decimal )loc.altitude );
	}

	/// <summary>
	/// ２点間の座標の差を計算する
	/// 参考 http://offsidenow.phpapps.jp/archives/496
	/// </summary>
	/// <param name="worldCoordinationMode"> true=世界測地系で計算を行う false=日本測地系で計算を行う</param>
	public static GpsPosition GetDistance(GpsPosition pos1,GpsPosition pos2, bool worldCoordinationMode=true)
	{
		var radlat1 = pos1.latitude * ( decimal )System.Math.PI / 180m; //緯度1
		var radlon1 = pos1.longitude * ( decimal )System.Math.PI / 180m; //経度1
		var radlat2 = pos2.latitude * ( decimal )System.Math.PI / 180m; //緯度2
		var radlon2 = pos2.longitude * ( decimal )System.Math.PI / 180m; //経度2
		//平均緯度
		var radLatAve = (radlat1 + radlat2) / 2;
		//緯度差
		var radLatDiff = radlat1 - radlat2;
		//経度差算
		var radLonDiff = radlon1 - radlon2;

		var sinLat = System.Math.Sin((double)radLatAve);
		double meridianRad;
		double dvrad;
		if(worldCoordinationMode){
			// 世界測地系で計算（デフォルト）
			double tmp =  1.0 - 0.00669438 * (sinLat*sinLat);
			meridianRad = 6335439.0 / System.Math.Sqrt(tmp*tmp*tmp); // 子午線曲率半径
			dvrad = 6378137.0 / System.Math.Sqrt(tmp); // 卯酉線曲率半径
		}else{
			// 日本測地系で計算
			double tmp = 1.0 - 0.00667478 * (sinLat*sinLat);
			meridianRad = 6334834.0 / System.Math.Sqrt(tmp*tmp*tmp); // 子午線曲率半径
			dvrad = 6377397.155 / System.Math.Sqrt(tmp); // 卯酉線曲率半径
		}
		var tLat = meridianRad * (double)radLatDiff;
		var tLon = dvrad * System.Math.Cos((double)radLatAve) * (double)radLonDiff;

		// 高度 はそのままメートル換算
	//	var alti = (pos1.altitude - pos2.altitude) * (decimal)111f;
		var alti = (pos1.altitude - pos2.altitude);

		return new GpsPosition( ( decimal )tLon, ( decimal )tLat, alti );
		/*
		var dist = (decimal)System.Math.Sqrt((t1*t1) + (t2*t2));

		dist = (decimal)System.Math.Floor(dist); //小数点以下切り捨て
		return dist; //２点間の直線距離を返す (単位はm)
		*/
	}
}
