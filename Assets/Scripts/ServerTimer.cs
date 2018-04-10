using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class ServerTimer : NetworkBehaviour {

	LocalServerInfo localServerObserver;

	public GameObject TimerObject;

	public float lastCheck;
	public bool hasSpawnedTimer;

	private TimerObj scriptInstance;

	public int numMins;
	public int numSecs;
	public int COUNTDOWN_TIME = 360;
	public const int ROUND_COUNTDOWN = 180;
	public GameObject HostControl;

	RoundNameUpdate roundTitleInstance;
	TimerTextUpdater timerTextUpdateInstance;
	public GameObject timerTextUpdateObj;
	public GameObject roundTitleObj;

	int numRoundsPassed;
	public GameObject EnvironmentButton;

	void updateTimerTextForAll()
	{
		if(timerTextUpdateInstance == null)
		{
			GameObject textUpdate = (GameObject) Instantiate (timerTextUpdateObj);
			timerTextUpdateInstance = textUpdate.GetComponent<TimerTextUpdater> ();
			NetworkServer.Spawn (textUpdate);
		}

		timerTextUpdateInstance.RpcUpdateClientTimerText (numMins, numSecs);

	}

	[Command]
	public void CmdSetMins(int mins)
	{
		numMins = mins;
		COUNTDOWN_TIME = numMins * 60 + numSecs;
		updateTimerTextForAll ();
	}
	[Command]
	public void CmdSetSecs(int secs)
	{
		numSecs = secs;
		COUNTDOWN_TIME = numMins * 60 + numSecs;
		updateTimerTextForAll ();

	}

	void Start () {


		lastCheck = 0;

		if(!isServer)
		{
			return;
		}

		HostControl.SetActive (true);
		numRoundsPassed = 0;
		hasSpawnedTimer = false;
		localServerObserver = FindObjectOfType<LocalServerInfo> ();


		GameObject roundTitleUpdater = (GameObject) Instantiate (roundTitleObj);
		roundTitleInstance = roundTitleUpdater.GetComponent<RoundNameUpdate> ();
		NetworkServer.Spawn (roundTitleUpdater);

	}
	public IEnumerator RoundCountdownRoutine() 
	{
		roundTitleInstance.RpcSetRoundCombatTitle (numRoundsPassed + 1);
		float remainingTime = ROUND_COUNTDOWN;
		float currentTime = ROUND_COUNTDOWN;

		while (currentTime > 0) 
		{

			yield return null;

			int newFloor = Mathf.FloorToInt (remainingTime);
			if(newFloor != currentTime)
			{
				currentTime = newFloor;
				scriptInstance.RpcUpdateCountdown ((int)currentTime);

			}

			remainingTime -= Time.deltaTime;
		}

		//increment round
		numRoundsPassed++;

		scriptInstance.RpcCombatTick ();

		if (numRoundsPassed < 4) {
			
			StartCoroutine (CountdownRoutine ());

		} else 
		{
			roundTitleInstance.RpcSetRoundGameOverTitle ();
			roundTitleInstance.RpcCallGameOverCheck ();

		}
	}

	public IEnumerator CountdownRoutine() 
	{
		roundTitleInstance.RpcSetRoundStrategyTitle (numRoundsPassed + 1);
		roundTitleInstance.RpcActivateMoveButton ();
		HostControl.SetActive (false);

		float remainingTime = COUNTDOWN_TIME;
		float currentTime = COUNTDOWN_TIME;

		while (currentTime > 0) 
		{
		
			yield return null;
		
			int newFloor = Mathf.FloorToInt (remainingTime);
			if(newFloor != currentTime)
			{
				currentTime = newFloor;
				scriptInstance.RpcUpdateCountdown ((int)currentTime);
				
			}

			remainingTime -= Time.deltaTime;
		}

		scriptInstance.RpcPostStrategyTick ();

		StartCoroutine (RoundCountdownRoutine ());

	}
	
	void Update () {
		if(!isServer || hasSpawnedTimer)
		{
			return;
		}
		if(Time.timeSinceLevelLoad - lastCheck > 4)
		{
			if(localServerObserver.AllAreDonePeeking())
			{
				GameObject newTimer = (GameObject)Instantiate (TimerObject);
				scriptInstance = newTimer.GetComponent<TimerObj> ();
				if(scriptInstance == null)
				{
					scriptInstance = newTimer.GetComponent<TimerObj> ();

				}
				NetworkServer.Spawn (newTimer);
				StartCoroutine (CountdownRoutine ());
				hasSpawnedTimer = true;
			}


			lastCheck = Time.timeSinceLevelLoad;
		}

	}


}
