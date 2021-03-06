using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{

    public Slider slider;
	public GameObject parent;
	public float max_health;

	private void Start()
	{
		SetMaxHealth(max_health);
	}
	public float GetHealth()
	{
		return slider.value;
	}
    public void SetHealth(float health)
	{
		slider.value = health;
		if (health <= 0)
		{
			if (parent != null) { Destroy(parent); }
		}
	}
	public void SetHealthRelative(float health)
	{
		SetHealth(slider.value - health);
	}

	public void SetMaxHealth(float health)
	{
		slider.maxValue = health;
		slider.value = health;
	}
}
