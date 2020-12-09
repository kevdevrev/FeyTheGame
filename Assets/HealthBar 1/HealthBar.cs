using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

	public Slider slider;
	public Gradient gradient;
	public Image fill;

	public Fey parent;


	private void Start()
	{ 
		parent = gameObject.GetComponentInParent<Fey>();
     
	}

	public void updateHP()
	{
		slider.value = parent.Health;
	}

	public void setMaxHP(int maxHP)
	{
		slider.maxValue = maxHP;
		slider.value = slider.maxValue;

	}

}
