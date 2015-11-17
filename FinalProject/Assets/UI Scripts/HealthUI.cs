using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour 
{
	public RocketHealth rh;

	public Image healthBar;
	public Image shieldBar;
	
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(rh == null)
		{
			GameObject ship = GameObject.FindGameObjectWithTag("myShip");
			if(ship != null)
				rh = ship.GetComponent<RocketHealth>();
		}
		else
		{
			healthBar.fillAmount = (float)rh.currentHealth/rh.MaxHealth;
			shieldBar.fillAmount = (float)rh.curShield/rh.MaxShield;
		}

	}
}
