using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
	public static event System.Action OnLevelFinished = null;
	
	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponentInParent<CharacterController>() != null)
		{
			OnLevelFinished?.Invoke();
		}
	}
}
