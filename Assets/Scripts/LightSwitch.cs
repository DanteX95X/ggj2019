using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
	private Timer timer = null;
	private Light pointLight = null;
	
	void Awake()
	{
		timer = transform.parent.GetComponentInChildren<Timer>();
		pointLight = FindObjectOfType<Light>();
	}

	public void Switch()
	{
		timer.TimeMultiplier = 4 - timer.TimeMultiplier;
		pointLight.range = 14 - pointLight.range;
	}
	
}
