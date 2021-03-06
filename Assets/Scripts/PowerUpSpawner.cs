﻿using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour {
	
	public GameObject shipLaserUp;
	public GameObject shipHealthUp;
	public GameObject shipShieldUp;
	public GameObject ship1Up;
	
	public GameObject starbaseWeaponUp;
	public GameObject starbaseHealthUp;
	public GameObject starbaseShieldUp;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShipPowerUp(Vector3 enemyPos)
	{
		int dropPowerUp = Random.Range(1,51);
		
		if(dropPowerUp == 3 || dropPowerUp == 25)
		{
			GameObject laserUp = Instantiate(shipLaserUp,enemyPos,Quaternion.identity)as GameObject;
		}
		else if(dropPowerUp == 10)
		{
			GameObject shipHealthIncrease = Instantiate(shipHealthUp,enemyPos,Quaternion.identity)as GameObject;
		}
		else if(dropPowerUp == 23)
		{
			GameObject shipShields = Instantiate(shipShieldUp,enemyPos,Quaternion.identity)as GameObject;
		}

	}
	
	public void StarbasePowerUp(Vector3 meteorPos)
	{
		int dropPowerUp = Random.Range(1, 21);
		
		if(dropPowerUp == 3)
		{
			GameObject laserUp = Instantiate(starbaseWeaponUp,meteorPos,Quaternion.identity)as GameObject;
		}
		else if(dropPowerUp == 9)
		{
			GameObject shipHealthIncrease = Instantiate(starbaseHealthUp,meteorPos,Quaternion.identity)as GameObject;
		}
		else if(dropPowerUp == 1)
		{
			GameObject shipShields = Instantiate(starbaseShieldUp,meteorPos,Quaternion.identity)as GameObject;
		}
	}
}
