using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{
	public Sprite cardBackSprite;
	public Text winText;

	private List<GameObject> cards;
	private GameObject[] cardsArray;

	[HideInInspector] public List<Card> selectedCards;

	public static GameController instance;

	void Awake ()
	{
		instance = this;
		cardsArray = Resources.LoadAll<GameObject>("");
		cards = new List<GameObject>(cardsArray);
		cardsArray = null;

		FillBoard();
	}

	void Update ()
	{
		if (GameIsCompleted())
		{
			winText.gameObject.SetActive(true);
			Card[] cards = FindObjectsOfType<Card>();
			foreach (Card card in cards)
			{
				card.GetComponent<Card>().interactable = false;
			}
			Timer timer = FindObjectOfType<Timer>();
			timer.GetComponent<Timer>().enabled = false;
		}
	}
		
	void FillBoard ()
	{
		//Fill the seven spaces where the player organizes the cards
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Organizing");

		for (int i = 0; i < gos.Length; i++)
		{
			for (int j = 0; j < i + 1; j++)
			{
				InstantiateCard(GameObject.Find("organizing_" + i));
			}
			GameObject.Find("organizing_" + i).GetComponent<OrganizingPile>().OrganizeChildOnStart();
		}

		//Fill the pile space where the player draws a card
		int cc = cards.Count;
		for (int i = 0; i < cc; i++)
		{
			InstantiateCard(GameObject.FindWithTag("Draw"));
		}
		GameObject.FindWithTag("Draw").GetComponent<DrawPile>().OrganizeChild();
	}

	void InstantiateCard (GameObject parent)
	{
		GameObject randomCard = cards[Random.Range(0, cards.Count)];
		GameObject rc = Instantiate(randomCard, parent.transform);
		rc.name = randomCard.name;
		cards.Remove(randomCard);
	}

	public void FlipCardToFront (GameObject target)
	{
		SpriteRenderer t = target.GetComponent<SpriteRenderer>();
		t.sprite = target.GetComponent<Card>().cardFrontSprite;
		t.GetComponent<Card>().currentSprite = t.sprite;
	}

	public void FlipCardToBack (GameObject target)
	{
		SpriteRenderer t = target.GetComponent<SpriteRenderer>();
		t.sprite = cardBackSprite;
		t.GetComponent<Card>().currentSprite = t.sprite;
	}

	public void OrganizeAllChild ()
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Organizing");

		for (int i = 0; i < gos.Length; i++)
		{
			GameObject.Find("organizing_" + i).GetComponent<OrganizingPile>().OrganizeChild();
		}

		GameObject[] gos2 = GameObject.FindGameObjectsWithTag("Mounting");

		for (int i = 0; i < gos2.Length; i++)
		{
			GameObject.Find("mounting_" + i).GetComponent<MountingPile>().OrganizeChild();
		}
		GameObject.FindWithTag("Draw").GetComponent<DrawPile>().OrganizeChild();
		GameObject.FindWithTag("Backup").GetComponent<BackupPile>().OrganizeChild();
	}

	bool GameIsCompleted ()
	{
		int mountingsCompleted = 0;

		GameObject[] mountingsParents = GameObject.FindGameObjectsWithTag("Mounting");

		foreach(GameObject mt in mountingsParents)
		{
			if (mt.transform.childCount == 13)
			{
				mountingsCompleted++;
			}
		}

		if (mountingsCompleted == 4)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}