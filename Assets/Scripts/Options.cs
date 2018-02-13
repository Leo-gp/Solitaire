using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour 
{
	public GameObject restartButton;
	public GameObject quitButton;
	public GameObject toggleTimerButton;
	public GameObject toggleZoomButton;
	public Text timerText;


	private Vector3 originalScale;
	private Vector3 originalPosition;
	private Vector3 offset;

	void OnMouseUpAsButton ()
	{
		if (!restartButton.activeInHierarchy)
		{
			restartButton.SetActive(true);
		}
		else
		{
			restartButton.SetActive(false);
		}

		if (!quitButton.activeInHierarchy)
		{
			quitButton.SetActive(true);
		}
		else
		{
			quitButton.SetActive(false);
		}

		if (!toggleTimerButton.activeInHierarchy)
		{
			toggleTimerButton.SetActive(true);
		}
		else
		{
			toggleTimerButton.SetActive(false);
		}

		if (!toggleZoomButton.activeInHierarchy)
		{
			toggleZoomButton.SetActive(true);
		}
		else
		{
			toggleZoomButton.SetActive(false);
		}
	}

	void Start ()
	{
		Transform child = transform.GetChild(0);
		originalScale = child.localScale;
		originalPosition = child.localPosition;
		offset = new Vector3(20, 0, 0);
	}

	void OnMouseOver ()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			child.localScale = (originalScale) + (Vector3.one * 3f);
			child.localPosition = (originalPosition) - (Vector3.one * 3f) + offset * i;
		}
	}

	void OnMouseExit ()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			child.localScale = originalScale;
			child.localPosition = originalPosition + offset * i;
		}
	}

	public void ToggleTimerText ()
	{
		if (timerText.GetComponent<Text>().enabled)
		{
			timerText.GetComponent<Text>().enabled = false;
		}
		else
		{
			timerText.GetComponent<Text>().enabled = true;
		}
	}

	public void ToggleZoom ()
	{
		if (Camera.main.orthographicSize == 5)
		{
			Camera.main.orthographicSize = 7;
		}
		else if (Camera.main.orthographicSize == 7)
		{
			Camera.main.orthographicSize = 9;
		}
		else
		{
			Camera.main.orthographicSize = 5;
		}
	}

	public void Restart ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Quit ()
	{
		Application.Quit();
	}
}