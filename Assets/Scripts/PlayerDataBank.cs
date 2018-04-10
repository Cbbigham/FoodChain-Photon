using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDataBank : MonoBehaviour {

    public static PlayerDataBank instance;
	public enum Animal
	{
		Chameleon = 0, Crocodile = 1, Crow = 2, Deer = 3, Eagle = 4, EgyptianPlover = 5,
		Hyena = 6, Lion = 7, Mallard = 8, Mouse = 9, Otter = 10, Rabit = 11, Snake = 12 

	};
	public List<PlayerData> PlayerInfoList;
	void Awake () {
        instance = this;
	}
   
	public int RequestPlayerHabitat(int ID)
	{
		return PlayerInfoList [ID].HabitatID;
	}
	public int RequestPlayerLevel(int ID)
	{
		return PlayerInfoList [ID].level;
	}
	public string RequestPlayerAnimalName(int ID)
	{
		return PlayerInfoList [ID].Name;
	}

	public void RequestPlayerData(int ID, out string mName, out string mSpecial, out string mWinCon, out string mLoseCon, out string mEnvironment, out bool mSky, out int mLevel, out Sprite mImage)
	{
		mName = PlayerInfoList [ID].Name;
		mSpecial = PlayerInfoList [ID].Special;
		mWinCon = PlayerInfoList [ID].WinCondition;
		mLoseCon = PlayerInfoList [ID].LoseCondition;
		mEnvironment = PlayerInfoList [ID].Environment;
		mSky = PlayerInfoList [ID].sky;
		mLevel = PlayerInfoList [ID].level;
		mImage = PlayerInfoList [ID].Image;

	}

	public bool getSkyStatus(int ID)
	{
		return PlayerInfoList [ID].sky;
	}




}

[System.Serializable]
public class PlayerData
{
	public int Id;
	public string Name;
	public string Special;
	public string WinCondition;
	public string LoseCondition;
	public string Environment;
	public bool sky;
	public int level;
	public Sprite Image;
	public int HabitatID;
}
