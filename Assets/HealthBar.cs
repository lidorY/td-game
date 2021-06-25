using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{

    public Slider slider;
	private void Start()
	{
		SetMaxHealth(100);
	}
	public float GetHealth()
	{
		return slider.value;
	}
    public void SetHealth(int health)
	{
		slider.value = health;
	}
	public void SetHealthRelative(float health)
	{
		slider.value -= health;
	}

	public void SetMaxHealth(int health)
	{
		slider.maxValue = health;
		slider.value = health;
	}
}
