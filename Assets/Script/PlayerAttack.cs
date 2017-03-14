using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public GameObject AttackPrefab;

    public void Attack()
    {
	//	Instantiate(AttackPrefab, Vector3.zero, Quaternion.identity);
		Instantiate(AttackPrefab, transform.position, Quaternion.identity);
    }
}
