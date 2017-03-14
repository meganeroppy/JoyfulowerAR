using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GyroAccelerator : MonoBehaviour {

	public float speed = 5.0f;

	public Text t;

	Vector3 origin;

	Rigidbody r;

	void Start()
	{
		origin = transform.position;

		Input.gyro.enabled = true;

	//	r = GetComponent<Rigidbody>();
	}

	void Update(){
		var dir = Vector3.zero;
		dir.x = Input.gyro.userAcceleration.x;
		dir.y = Input.gyro.userAcceleration.y;
		dir.z = Input.gyro.userAcceleration.z;

		//dir -= Physics.gravity;

		if(dir.sqrMagnitude > 1){
			dir.Normalize();
		}

		dir *= Time.deltaTime;

		transform.Translate(dir * speed);
	//	r.AddForce(dir * speed);

		string s = "";
		s += string.Format("pos = (\nx={0},\ny={1},\nz={2}\n)\n", transform.position.x, transform.position.y, transform.position.z);
		s += string.Format("dir = (\nx={0},\ny={1},\nz={2}\n)\n", dir.x, dir.y, dir.z );
		s += string.Format("x[{0}]y[{1}]z[{2}]", dir.x > 0 ? " + " : " - ", dir.y > 0 ? " + " : " - ", dir.z > 0 ? " + " : " - ");
		t.text = s;
	}

	public void Reset()
	{
		transform.position = origin;
	}
}