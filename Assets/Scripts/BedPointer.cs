using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedPointer : MonoBehaviour
{
	private Bed target = null;
	
	private void Awake()
	{
		target = FindObjectOfType<Bed>();
	}
	
	void Update()
	{
		Vector2 direction = ((Vector2) target.transform.position - (Vector2) transform.parent.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
		transform.eulerAngles = new Vector3(0, 0, angle);
	}
}
