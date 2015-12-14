﻿using UnityEngine;
using System.Collections;

public class a_do_trapBullfrog : a_do_trap_platform {

	/*
	 * Guess
	 */
	public static int targetX;//the position offsets to change.
	public static int targetY;
	public static int BaseX=48;
	public static int BaseY=48;

	public static int[,] heights =new int[8,8];

	public override void ExecuteTrap (int triggerX, int triggerY, int State)
	{
		switch (objInt.Owner)
		{
		case 0://Raise tile
			Debug.Log ("Raise tile (" + (BaseX+targetX) + "," + (BaseY +targetY) + ")");
			RaiseLowerBullfrog(+1);
			break;
		case 1://Lower tile
			Debug.Log ("Lower tile (" + (BaseX+targetX) + "," + (BaseY +targetY) + ")");
			RaiseLowerBullfrog(-1);
			break;
		case 2:

			targetX=targetX+1;
			if (targetX>=8)
			{
				targetX=0;
			}
			//Debug.Log ("Increment X =" + targetX);
			break;
		case 3:
			//Debug.Log ("Increment Y");
			targetY=targetY+1;

			if (targetY>=8)
			{
				targetY=0;
			}
			break;
		case 4://reset
			Debug.Log ("Reset all");
			ResetBullFrog();
			break;
		}
	}

	public void ResetBullFrog()
	{//TODO:Move player and all objects within area to a safe spot when resetting.
		//000~001~193~A voice utters the words "Reset Activated."
		ml.Add(playerUW.StringControl.GetString (1,193));
		for (int x=0; x<8; x++)
		{
			for (int y=0; y<8 ; y++)
			{
				GameObject platformTile=GameWorldController.FindTile ((BaseX+x),(BaseY+y),1);
				heights[x,y]=0;
				StartCoroutine(MoveTile (platformTile.transform, -platformTile.transform.position,0.1f));
				//platformTile.transform.position = Vector3.zero;
			}
		}
	}


	public void RaiseLowerBullfrog(int dir)
	{//TODO:Add a check for tiles at max/min height
		for (int x=-1; x<=1; x++)
		{
			for (int y=-1; y<=1; y++)
			{
				if ((x==0) && (y==0))
					{
						//raise or lower by 2
					if (((heights[targetX+x,targetY+y]<8) && (dir==+1)) || ((heights[targetX+x,targetY+y]>-4) && (dir==-1)))
						{
						GameObject platformTile=GameWorldController.FindTile ((BaseX+targetX+x),(BaseY+targetY+y),1);
						StartCoroutine(MoveTile (platformTile.transform, new Vector3(0f,(float)(2*dir) * (0.3f),0f) ,0.1f));
						heights[targetX+x,targetY+y]+=dir*2;	
						}
					}
					else
					{
						//raise by 1 if within bounds
						if (
							(targetX+x >= 0) && (targetX+x<+8)
							&& 
							(targetY+y >= 0) && (targetY+y<+8)
							)
						{
						if (((heights[targetX+x,targetY+y]<8) && (dir==+1)) || ((heights[targetX+x,targetY+y]>-4) && (dir==-1)))
							{
							//Raise or lower by 1
							GameObject platformTile=GameWorldController.FindTile ((BaseX+targetX+x),(BaseY+targetY+y),1);
							StartCoroutine(MoveTile (platformTile.transform, new Vector3(0f,(float)(1*dir) * (0.3f),0f) ,0.1f));
							heights[targetX+x,targetY+y]+=dir;	
							}
						}
					}
				}

		}
	}


	public override void PostActivate ()
	{//To stop destruction of trap

	}
}


