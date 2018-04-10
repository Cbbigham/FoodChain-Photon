using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LookAtTimer : MonoBehaviour {

	float lastCheck;
	float checkFrequecy;

	public Text TimerNumberText;
	public Text RoundTitle;

	public TextNotification timerWarning;
	public LocalServerInfo serverInfo;

	public GameObject EnvironmentButton;
	public GameObject CombatButton;


	public int numSecs;

	public PeekMenuToggle environmentMenu;
	public PeekMenuToggle combatMenu;


	void Start () {
		numSecs = 1000000;
		lastCheck = 0;
		checkFrequecy = 0.1f;
	}

	void Update()
	{
		if(((Time.timeSinceLevelLoad - lastCheck) > checkFrequecy) && isSetupRound())
		{
			Tick ();

			lastCheck = Time.timeSinceLevelLoad;
		}
	}


	public void Tick()
	{
		if(isStrategyRound())
		{

			if(numSecs > 58 && numSecs < 60)
			{
				timerWarning.Brighten ();

				if(!AudioManager.instance.source.isPlaying)
				{
					AudioManager.instance.PlayTimeLowSound ();
				}

			}
		}
			

	}

	public void postCombatCheck()
	{

		if (!serverInfo.selfData.hasAttackedThisRound) {
			serverInfo.IncrementStarvationCounter ();
		} else 
		{
			serverInfo.ResetStarvationCounter ();
		}

		serverInfo.checkForStarvation ();

		CombatButton.SetActive (false);

		if(combatMenu.active)
		{
			combatMenu.ToggleMenu ();
		}

		serverInfo.hasTeleported = false;
	}
	public void PostStrategyRoundCheck()
	{
		if((!serverInfo.hasTeleported))
		{

			serverInfo.PerformRandomSelfTeleport ();

			if(environmentMenu.active)
			{
				environmentMenu.ToggleMenu ();
			}

		}
		serverInfo.DetermineButtonState (serverInfo.lastTeleport);
		EnvironmentButton.SetActive (false);
		serverInfo.TeleportHistory.Add (serverInfo.lastTeleport);
		serverInfo.hasTeleported = false;
		CombatButton.SetActive (true);

	}
	public void PostSetupRoundCheck()
	{
		serverInfo.DetermineButtonState (PlayerDataBank.instance.RequestPlayerHabitat(serverInfo.selfData.playerID));
	}
	bool isStrategyRound()
	{
		bool retval = false;
		
		if(RoundTitle.text == "Strategy Round")
		{
			retval = true;
		}

		return retval;
	}

	bool isSetupRound()
	{
		bool retval = false;

		if(RoundTitle.text == "Setup Round")
		{
			retval = true;
		}

		return retval;
	}

	bool isCombatRound()
	{
		bool retval = false;

		if(RoundTitle.text == "Combat Round")
		{
			retval = true;
		}

		return retval;
	}

	int getMinsFromText(string time)
	{
		string mins = time.Substring (0, 1);
		int retval = 0;

		int.TryParse (mins, out retval);

		return retval;
	}
	int getSecsFromText(string time)
	{
		string secs = time.Substring (2);
		int retval = 0;

		int.TryParse (secs, out retval);

		return retval;
	}
}
