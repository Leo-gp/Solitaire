using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour 
{
	private Text timerText;
	private float timer = 0;
	private float minutes;
	private float seconds;

	void Awake ()
	{
		timerText = GetComponent<Text>();
	}
		
	void Update () 
	{
		if (timer/60 <= 59.99f)
		{
			timer += Time.deltaTime;
			minutes = Mathf.Floor(timer / 60);
			seconds = Mathf.Floor(timer - minutes * 60);
			timerText.text = minutes.ToString("0") + ":" + seconds.ToString("00");
		}
		else
		{
			timerText.text = "59:59+";
		}
	}
}