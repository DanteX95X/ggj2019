using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedPointer : MonoBehaviour
{
	private Bed target = null;

	private float lifetime = 3;
	
	private void Awake()
	{
		target = FindObjectOfType<Bed>();
	}

	void Start()
	{
		Vector2 direction = ((Vector2) target.transform.position - (Vector2) transform.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
		transform.eulerAngles = new Vector3(0, 0, angle);
	}

	private void Update()
	{
		lifetime -= Time.deltaTime;

		if (lifetime <= 0)
		{
			Destroy(gameObject);
		}
	}
}
