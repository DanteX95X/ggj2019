using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public static event System.Action OnGameOver = null;
	
	public float Counter { get; set; }

	private float timeLimit;
	public float TimeLimit
	{
		get { return timeLimit; }
		set
		{
			timeLimit = value;
			Counter = timeLimit;
		}
	}

	private Image image = null;
	
	private void Awake()
	{
		image = GetComponent<Image>();
	}
	
	private void Update()
	{
		Counter -= Time.deltaTime;
		image.fillAmount = 1 - Counter / TimeLimit;
		if (Counter <= 0)
		{
			OnGameOver?.Invoke();
		}
	}
	
}
