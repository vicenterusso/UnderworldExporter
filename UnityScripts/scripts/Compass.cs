﻿using UnityEngine;
using System.Collections;

public class Compass : GuiBase {

	//Updates the compass display based on the characters heading.

	private int PreviousHeading=-1;
	public const int NORTH = 0;
	public const int NORTHNORTHEAST = 1;
	public const int NORTHEAST = 2;
	public const int EASTNORTHEAST = 3;
	public const int EAST = 4 ;
	public const int EASTSOUTHEAST = 5 ;
	public const int SOUTHEAST = 6 ;
	public const int SOUTHSOUTHEAST = 7 ;
	public const int SOUTH = 8;
	public const int SOUTHSOUTHWEST = 9 ;
	public const int SOUTHWEST = 10 ;
	public const int WESTSOUTHWEST = 11 ;
	public const int WEST = 12;
	public const int WESTNORTHWEST = 13 ;
	public const int NORTHWEST = 14 ;
	public const int NORTHNORTHWEST = 15 ;

	private UITexture comp;
	public UITexture[] NorthIndicators=new UITexture[16];

	private Texture2D[] CompassPoles=new Texture2D[4];

	// Use this for initialization
	void Start () {
		comp=this.GetComponent<UITexture>();
		for (int i=0;i<4;i++)
		{
			CompassPoles[i]=Resources.Load <Texture2D> ("HUD/Compass/Compass_000"+i.ToString());
		}
	}


	
	// Update is called once per frame
	void Update () {
		if (PreviousHeading!=playerUW.currentHeading)
		{
			UpdateNorthIndicator();
			PreviousHeading=playerUW.currentHeading;
			switch (playerUW.currentHeading)
			{
				case NORTH:
				case SOUTH:
				case WEST:
				case EAST:
					{
					comp.mainTexture=CompassPoles[0];//Resources.Load <Texture2D> ("HUD/Compass/Compass_0000");
					break;
					}
				case NORTHWEST:
				case NORTHEAST:
				case SOUTHEAST:
				case SOUTHWEST:
					{
						comp.mainTexture=CompassPoles[2];//Resources.Load <Texture2D> ("HUD/Compass/Compass_0002");
						break;
					}
				case NORTHNORTHWEST:
				case EASTNORTHEAST:
				case SOUTHSOUTHEAST:
				case WESTSOUTHWEST:
					{
						comp.mainTexture=CompassPoles[1];//Resources.Load <Texture2D> ("HUD/Compass/Compass_0001");
						break;
					}
				default:
					{
					comp.mainTexture=CompassPoles[3];//Resources.Load <Texture2D> ("HUD/Compass/Compass_0003");
					break;
					}
		
			}
		}
	}

	void UpdateNorthIndicator()
	{
		for (int i =0; i<16;i++)
		{
			NorthIndicators[i].enabled=(i==playerUW.currentHeading);
		}
	}



	void OnClick()
	{
		playerUW.playerHud.MessageScroll.Clear ();
		playerUW.playerHud.MessageScroll.Add (playerUW.StringControl.GetString (1,64) 
		                                      + playerUW.GetFedStatus() 
		                                      + " and " 
		                                      + playerUW.GetFatiqueStatus());

		playerUW.playerHud.MessageScroll.Add (playerUW.StringControl.GetString (1,65) 
		                                      + playerUW.StringControl.GetString (1,411+GameWorldController.instance.LevelNo) 
		                                      + playerUW.StringControl.GetString (1,66));
		if (GameClock.day<10)
		{
			playerUW.playerHud.MessageScroll.Add (playerUW.StringControl.GetString (1,67) 
			                                      + playerUW.StringControl.GetString (1,411+GameClock.day)
			                                      + playerUW.StringControl.GetString (1,68));
		}
		else
		{//incountable
			playerUW.playerHud.MessageScroll.Add (playerUW.StringControl.GetString (1,69));
		}

		playerUW.playerHud.MessageScroll.Add (playerUW.StringControl.GetString (1,70) 
		                                      + playerUW.StringControl.GetString (1,71+((GameClock.hour)/2)));


		/*
000~001~064~You are currently 
000~001~065~You are on the 
000~001~066~ level of the Abyss.
000~001~067~It is the 
000~001~068~ day of your imprisonment.
000~001~069~It has been an uncountable number of days since you entered the Abyss.
000~001~070~You guess that it is currently 
000~001~071~night
000~001~072~night
000~001~073~dawn
000~001~074~early morning
000~001~075~morning
000~001~076~late morning
000~001~077~mid-day
000~001~078~afternoon
000~001~079~late afternoon
000~001~080~early evening
000~001~081~evening
000~001~082~night



000~001~113~fatigued
000~001~114~very tired
000~001~115~drowsy
000~001~116~awake
000~001~117~rested
000~001~118~wide awake


000~001~411~first
000~001~412~second
000~001~413~third
000~001~414~fourth
000~001~415~fifth
000~001~416~sixth
000~001~417~seventh
000~001~418~eighth
000~001~419~ninth
000~001~420~tenth
		 */
	}
}
