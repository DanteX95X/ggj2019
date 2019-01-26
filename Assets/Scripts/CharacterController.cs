using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	[SerializeField] private float speed = 10.0f;
	[SerializeField] private Sprite sleeping = null;
	
	private new Rigidbody2D rigidbody = null;
	private SpriteRenderer sprite = null;
	private AudioSource[] audios = null;

	private Vector2 targetPosition;
	private Vector2 direction;

	public float Speed => speed;
	
	private void Awake()
	{
		rigidbody = GetComponentInChildren<Rigidbody2D>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		audios = GetComponents<AudioSource>();
	}

	private void Update()
	{
		if(Input.GetMouseButton(0))
		{
			targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (targetPosition != (Vector2)transform.position)
			{
				direction = (targetPosition - (Vector2) transform.position).normalized;
				rigidbody.velocity = direction * speed;
			}
		}
		else
		{
			rigidbody.velocity = Vector2.zero;
		}

		if ((targetPosition - (Vector2) transform.position).magnitude < 0.1f)
		{
			rigidbody.velocity = Vector2.zero;
		}
		else
		{
			float angle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
			sprite.transform.eulerAngles = new Vector3(0, 0, angle);
		}
	}

	public void SwapSprites()
	{
		sprite.sprite = sleeping;
		rigidbody.velocity = Vector2.zero;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		audios[Random.Range(0, audios.Length)].Play();
	}
}
