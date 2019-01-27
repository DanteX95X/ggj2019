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
	private AudioSource stepSounds = null;

	private Vector2 targetPosition;
	private Vector2 direction;

	public float Speed => speed;

	private float timer = 0.0f;
	
	private void Awake()
	{
		rigidbody = GetComponentInChildren<Rigidbody2D>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		audios = GetComponents<AudioSource>();
		stepSounds = sprite.GetComponent<AudioSource>();
	}

	private void Update()
	{
		if(Input.GetMouseButton(0))
		{
			if (timer > 0)
			{
				timer -= Time.deltaTime;
			}
			else
			{
				stepSounds.Play();
				StepSoundsPlay();
			}
			
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
			timer = 0;
		}

		if ((targetPosition - (Vector2) transform.position).magnitude < 0.1f)
		{
			rigidbody.velocity = Vector2.zero;
			timer = 0;
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

	private void StepSoundsPlay()
	{
		timer = 0.3f + Random.Range(0.0f, 0.1f);
	}
}
