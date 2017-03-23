using UnityEngine;
using System.Collections;

class RotateCamera : MonoBehaviour
{
	[SerializeField]
	Transform rotCamera = null;

	void Update () 
	{
	//	transform.rotation = Quaternion.Euler (0, rotCamera.transform.rotation.y, 0);
		transform.LookAt( rotCamera );
	}

	void Disable() 
	{
		this.gameObject.SetActive(false);
	}
}