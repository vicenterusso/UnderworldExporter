﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuHud : GuiBase {
		public Texture2D CursorIcon;
		public Rect CursorPosition;


		//References to hud elements
		public GameObject CharGen;//panel
		public Text CharName;
		public Text CharGender;
		public Text CharClass;
		public Text CharStr;
		public Text CharDex;
		public Text CharInt;
		public Text CharVit;
		public Text[] CharSkillName;
		public Text[] CharSkillVal;
		public InputField EnterCharName;
		public int MenuMode =0; //0=main, 1 = chargen
		public int chargenStage=0; 
		//0 = Gender
		//1 = Handeness
		//2 = Class
		//3,4,5,6,7 = Skills. Some are skipped over.
		//8 is portrait/race
		//9 is difficulty.
		//10 is name
		//11 is confirm.

		public Text CharGenQuestion;
		private string CharNameAns;
		private int CharClassAns;
		private int SkillSeed;
		public RawImage CharGenBody;

		protected  int cursorSizeX =64;
		protected  int cursorSizeY =64;

		void Start()
		{
			if (GameWorldController.instance.AtMainMenu)
			{
					CursorPosition = new Rect(
							0.0f,
							0.0f,
							cursorSizeX,
							cursorSizeY);
					//Play the splash screens.
					UWHUD.instance.CutScenesFull.SetAnimation="FadeToBlackSleep";
					Cutscene_Splash ci = UWHUD.instance.gameObject.AddComponent<Cutscene_Splash>();
					UWHUD.instance.CutScenesFull.cs=ci;
					UWHUD.instance.CutScenesFull.Begin();
			}
		}

		void OnGUI()
		{
				CursorPosition.center = Event.current.mousePosition;
				GUI.DrawTexture (CursorPosition,CursorIcon);
		}


		public void ButtonClickMainMenu(int option)
		{//Button clicks on front menu.

				if (MenuMode==0)
				{
					switch (option)	
					{
					case 0: //PLay introduction
							//UWHUD.instance.CutScenesFull.SetAnimation="cs013_n01";
							Cutscene_Intro ci = UWHUD.instance.gameObject.AddComponent<Cutscene_Intro>();
							UWHUD.instance.CutScenesFull.cs=ci;
							UWHUD.instance.CutScenesFull.Begin();
					
							break;

					case 1: // Create Character
							MenuMode=1;
							CharGen.SetActive(true);
							CharGenQuestion.text=getQuestion(0);
							PlaceButtons(Chargen.GetChoices(Chargen.STAGE_GENDER,-1),false);
							break;

					case 2:// Acknowledgements
							Cutscene_Credits cc = UWHUD.instance.gameObject.AddComponent<Cutscene_Credits>();
							UWHUD.instance.CutScenesFull.cs=cc;
							UWHUD.instance.CutScenesFull.Begin();
							break;
					case 3:// Journey onwards. In the future will be a link to load menu
							JourneyOnwards();
							break;
					}		
				}
				else
				{//Chargen
						ChargenClick(option);	
				}

		}


		public void ChargenClick(int option)
		{

				//0 = Gender
				//1 = Handeness
				//2 = Class
				//3,4,5,6,7 = Skills. Some are skipped over.
				//8 is portrait/race
				//9 is difficulty.
				//10 is name
				//11 is confirm.


				switch (chargenStage)
				{
				case Chargen.STAGE_GENDER:
					//Gender
						GameWorldController.instance.playerUW.PlayerSkills.InitSkills();//Set all skills to zero
						if (option == 0)
						{
							GameWorldController.instance.playerUW.isFemale=false;
						}
						else
						{
							GameWorldController.instance.playerUW.isFemale=true;
						}
						CharGender.text=StringController.instance.GetString(2,Chargen.GetChoices(chargenStage,-1)[option]);
						chargenStage++;
						PlaceButtons(Chargen.GetChoices(chargenStage,-1),false);

					break;
				case Chargen.STAGE_HANDENESS:
					//Handedness
						if (option == 0)
						{
								GameWorldController.instance.playerUW.isLefty=false;
						}
						else
						{
								GameWorldController.instance.playerUW.isLefty=true;
						}
						chargenStage++;
						PlaceButtons(Chargen.GetChoices(chargenStage,-1),false);
					break;
				case Chargen.STAGE_CLASS:
						GameWorldController.instance.playerUW.CharClass=getClass(option);

						CharClassAns=option;
						SkillSeed= Chargen.getSeed(option);
						//Set str, int and dex here.
						GameWorldController.instance.playerUW.PlayerSkills.STR=Chargen.getBaseSTR(option) + Random.Range(1,SkillSeed);
						GameWorldController.instance.playerUW.PlayerSkills.INT=Chargen.getBaseINT(option) + Random.Range(1,SkillSeed);
						GameWorldController.instance.playerUW.PlayerSkills.DEX=Chargen.getBaseDEX(option) + Random.Range(1,SkillSeed);
						CharStr.text = GameWorldController.instance.playerUW.PlayerSkills.STR.ToString();
						CharInt.text = GameWorldController.instance.playerUW.PlayerSkills.INT.ToString();
						CharDex.text = GameWorldController.instance.playerUW.PlayerSkills.DEX.ToString();
						CharClass.text= GameWorldController.instance.playerUW.CharClass;
						//todo
						chargenStage++;
						if (Chargen.GetChoices(chargenStage,CharClassAns).GetUpperBound(0)==0)
						{			//Only one choice. Accept it by default.					
							//	chargenStage++;
								CharGenQuestion.text=getQuestion(chargenStage);
								ChargenClick(0);
								return;
						}
						else
						{
								PlaceButtons(Chargen.GetChoices(chargenStage,CharClassAns),false);		
						}

						break;

				case Chargen.STAGE_SKILLS_1:
				case Chargen.STAGE_SKILLS_2:
				case Chargen.STAGE_SKILLS_3:
				case Chargen.STAGE_SKILLS_4:
						//Set skills here if possible.
						AdvanceSkill(option,chargenStage);
						chargenStage++;
						if (Chargen.GetChoices(chargenStage,CharClassAns).GetUpperBound(0)==0)
						{		//Only one choice. Accept it by default.					
								//chargenStage++;
								CharGenQuestion.text=getQuestion(chargenStage);
								ChargenClick(0);
								return;
						}
						else
						{
								PlaceButtons(Chargen.GetChoices(chargenStage,CharClassAns),false);		
						}
						break;
				case Chargen.STAGE_SKILLS_5:
						//Assume I will always have a choice here.
						//Set skills here if possible.
						AdvanceSkill(option,chargenStage);
						chargenStage++;
						PlaceButtons(Chargen.GetChoices(chargenStage,CharClassAns),true);//Moving to protrait.
						break;
				case Chargen.STAGE_PORTRAIT:
						chargenStage++;
						PlaceButtons(Chargen.GetChoices(chargenStage,-1),false);
						GameWorldController.instance.playerUW.Body=option;
						//Show the matching body.
						//Update the paperdoll.

						break;
				case Chargen.STAGE_DIFFICULTY:
						//Not implemented.
						//Show the name input box.
						//Remove buttons.
						chargenStage++;
						RemoveButtons();
						EnterCharName.gameObject.SetActive(true);
						EnterCharName.Select();
						break;
				case Chargen.STAGE_NAME:
						//Set the player name.
						GameWorldController.instance.playerUW.CharName=CharNameAns;
						chargenStage++;
						EnterCharName.gameObject.SetActive(false);
						PlaceButtons(Chargen.GetChoices(chargenStage,CharClassAns),false);
						break;
				case Chargen.STAGE_CONFIRM:
						if (option==0)
						{
							//Start a new game
								JourneyOnwards();
						}
						else
						{
							//restart chargen
							chargenStage=Chargen.STAGE_GENDER;
							PlaceButtons(Chargen.GetChoices(chargenStage,-1),false);
						}
						break;
				}

				//Set next question.
				CharGenQuestion.text=getQuestion(chargenStage);

		}

		public void EnterCharNameEvent()
		{
				chargen
				CharNameAns=EnterCharName.text;
				ChargenClick(0);
		}

		public void AdvanceSkill(int option, int Stage)
		{
				int actualSkillNo = Chargen.GetChoices(Stage,CharClassAns)[option]-31;
				Debug.Log("advancing " + (actualSkillNo) + " by " + SkillSeed);
				GameWorldController.instance.playerUW.PlayerSkills.AdvanceSkill(actualSkillNo,Random.Range(1,SkillSeed));
		}

		public string getClass(int option)
		{
				return StringController.instance.GetString(2,23+option);
		}

		public string getQuestion(int option)
		{
				//0 = Gender
				//1 = Handeness
				//2 = Class
				//3,4,5,6,7 = Skills. Some are skipped over.
				//8 is portrait/race
				//9 is difficulty.
				//10 is name
				//11 is confirm.

				switch (option)
				{
				case 0:
				case 1:
				case 2:
						return StringController.instance.GetString(2,1+option);
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
						return StringController.instance.GetString(2,4);
				case 8:
						return "";//no question about portait.
				case 9:
				case 10:
				case 11:
						return StringController.instance.GetString(2,option-3);
				default:
						return "UNKNOWN Option!";
				}
		}

		public void RemoveButtons()
		{
			//Destroy existing buttons.
			foreach(Transform child in CharGen.transform)
			{
					//Debug.Log(child.name.Substring(0,4) );
					if (child.name.Substring(0,5) == "_Char")
					{
							Destroy(child.transform.gameObject);
					}
			}	
		}

		public void PlaceButtons(int[] buttons, bool isImageButton)
		{
				RemoveButtons();
	

			if (isImageButton)
			{//JUST A VERTICAL LIST OF PORTRAITS
				int GenderPortraitIndex=7;
				if (GameWorldController.instance.playerUW.isFemale)
				{
					GenderPortraitIndex=12;
				}
				for (int i=0; i<=buttons.GetUpperBound(0);i++)	
				{
					GameObject myObj = (GameObject)Instantiate(Resources.Load("Prefabs/_CharGenImageButton"));
					myObj.transform.SetParent(CharGen.transform);
					myObj.GetComponent<ChargenButton>().SubmitTarget=this;
					myObj.GetComponent<ChargenButton>().Value=i;
					myObj.GetComponent<RectTransform>().anchoredPosition=new Vector3(70f, 60 - i *30f);
					myObj.GetComponent<RectTransform>().localScale=new Vector2(1.0f,1.0f);
					//Load the protraits					
					myObj.GetComponent<ChargenButton>().ButtonImage.texture = (Texture2D)Resources.Load(_RES +"/Hud/Chargen/chrbtns_" + (GenderPortraitIndex+i).ToString("0000"));
				}
			}	
			else
				{
				//Pick a configuration to use.
						if (buttons.GetUpperBound(0)<=8)
						{//One Column
						for (int i=0; i<=buttons.GetUpperBound(0);i++)
							{
								GameObject myObj = (GameObject)Instantiate(Resources.Load("Prefabs/_CharGenTextButton"));
								myObj.transform.SetParent(CharGen.transform);
								myObj.GetComponent<ChargenButton>().SubmitTarget=this;
								myObj.GetComponent<ChargenButton>().Value=i;
								myObj.GetComponent<RectTransform>().anchoredPosition=new Vector3(70f, 60 - i *20f);
								myObj.GetComponent<RectTransform>().localScale=new Vector2(1.0f,1.0f);
								myObj.GetComponent<ChargenButton>().ButtonText.text=StringController.instance.GetString(2,buttons[i]);
							}

						}
						else
						{
						//Two Columns
							for (int i=0; i<=buttons.GetUpperBound(0);i++)
							{//First 5 buttons
								GameObject myObj = (GameObject)Instantiate(Resources.Load("Prefabs/_CharGenTextButton"));
								myObj.transform.SetParent(CharGen.transform);
								myObj.GetComponent<ChargenButton>().SubmitTarget=this;
								myObj.GetComponent<ChargenButton>().Value=i;
								if (i<5)
								{//First 4 buttons
									myObj.GetComponent<RectTransform>().anchoredPosition=new Vector3(50f, 90- i *10f);			
								}
								else
								{
									myObj.GetComponent<RectTransform>().anchoredPosition=new Vector3(80f, 90- (i-5) *10f);			
								}	
								myObj.GetComponent<ChargenButton>().ButtonText.text=StringController.instance.GetString(2,buttons[i]);
								myObj.GetComponent<RectTransform>().localScale=new Vector2(1.0f,1.0f);
							}
						}
				}		
		}


		public void JourneyOnwards()
		{
			GameWorldController.instance.SwitchLevel(0);
			GameWorldController.instance.playerUW.transform.position= new Vector3(38f, 4f, 2.7f);
			UWHUD.instance.gameObject.SetActive(true);
			GameWorldController.instance.playerUW.playerController.enabled=true;
			GameWorldController.instance.playerUW.playerMotor.enabled=false;
			GameWorldController.instance.AtMainMenu=false;
			GameWorldController.instance.playerUW.playerInventory.Refresh();
			Destroy (this.gameObject);
		}
}
