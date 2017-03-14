using UnityEngine;
using System.Collections;

/// <summary>
/// 常に北を向く
/// </summary>
public class Compass : MonoBehaviour {

	Vector3 eRot = new Vector3(90,0,0);
	Quaternion q;

	void Start()
	{
		q = Quaternion.Euler( eRot );
	}

	// Update is called once per frame
	void Update () {
		transform.rotation = q;
	}
}
