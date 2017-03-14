using UnityEngine;
using System.Collections;

public class GridObject : MonoBehaviour {

	public GridCube gridCube;

	/// <summary>
	/// 設置間隔(メートル)
	/// </summary>
	public float interval = 1f;

	/// <summary>
	/// 自分を中心とした設置範囲（メートル）
	/// </summary>
	public float range = 3f;

	// Use this for initialization
	void Start ()
	{
		int line = (int)(((range * 2) + 1) / interval);

		for(int x=0 ; x < line ; ++x)
		{
			for(int y=0 ; y < line ; ++y)
			{
				for(int z=0 ; z < line ; ++z)
				{
					var c = Instantiate<GridCube>(gridCube);
					c.transform.SetParent(transform);
					Vector3 pos = new Vector3(x * interval, y * interval, z * interval);
					c.transform.localPosition = pos;
					c.t.text = string.Format("({0}, {1}, {2})", (int)pos.x, (int)pos.y, (int)pos.z);
				}
			}
		}	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
