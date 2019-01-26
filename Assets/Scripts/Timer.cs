using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public static event System.Action OnGameOver = null;
	
	public float Counter { get; set; }
	public float TimeMultiplier { get; set; } = 1;

	private Text text = null;
	
	private void Awake()
	{
		text = GetComponent<Text>();
	}
	
	private void Update()
	{
		Counter -= Time.deltaTime * TimeMultiplier;
		text.text = "" + (int) Counter;
		if (Counter <= 0)
		{
			OnGameOver?.Invoke();
		}
	}
	
}
