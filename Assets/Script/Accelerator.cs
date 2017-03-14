using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Accelerator : MonoBehaviour {

	public float speed = 5.0f;

	public Text t;

	Vector3 origin;

	void Start()
	{
		origin = transform.position;
	}

	void Update(){
		var dir = Vector3.zero;
		dir.x = Input.acceleration.x;
		dir.y = Input.acceleration.y;
		dir.z = Input.acceleration.z;

		//dir -= Physics.gravity;

		if(dir.sqrMagnitude > 1){
			dir.Normalize();
		}

		dir *= Time.deltaTime;

		transform.Translate(dir * speed);

		string s = "";
		s += string.Format("pos = ({0},{1},{2})\n", transform.position.x, transform.position.y, transform.position.z);
		s += string.Format("dir = ({0},{1},{2})\n", dir.x, dir.y, dir.z );
		s += string.Format("x[{0}]y[{1}]z[{2}]", dir.x > 0 ? " + " : " - ", dir.y > 0 ? " + " : " - ", dir.z > 0 ? " + " : " - ");
		t.text = s;
	}

	public void Reset()
	{
		transform.position = origin;
	}
}