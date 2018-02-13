using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour 
{
	public string suit;
	public string color;
	public int number;
	public Sprite cardFrontSprite;

	[HideInInspector] public Sprite currentSprite;

	private Vector3 mousePos;
	private Vector3 mouseOffset;

	private float lastClickTime = 0;

	[HideInInspector] public bool interactable;

	public GameObject targetToPlace;

	private Collider2D myCollider;
	public Collider2D touchingCol;

	private List<Card> selectedCards;

	void Awake ()
	{
		currentSprite = GetComponent<SpriteRenderer>().sprite;
		cardFrontSprite = currentSprite;
		selectedCards = GameController.instance.selectedCards;
		myCollider = GetComponent<Collider2D>();
	}

	void OnMouseDrag ()
	{
		if (transform.parent != GameObject.FindWithTag("Draw").transform && interactable)
		{
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			if (selectedCards.Count == 1)
			{
				transform.position = new Vector3(mousePos.x + mouseOffset.x, mousePos.y + mouseOffset.y, -90);
			}
			else
			{
				for (int i = 1; i < selectedCards.Count; i++)
				{
					Card clickedCard = selectedCards[0];
					Card cs = selectedCards[i];
					clickedCard.transform.position = new Vector3(mousePos.x + mouseOffset.x, mousePos.y + mouseOffset.y, -50);
					cs.transform.position = new Vector3(clickedCard.transform.position.x, clickedCard.transform.position.y - (0.4f * i), (-50) - (i));
				}
			}
		}
	}

	void OnMouseDown ()
	{
		if (currentSprite == cardFrontSprite)
		{
			interactable = true;
		}
		else
		{
			interactable = false;
		}

		if (interactable)
		{
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mouseOffset = transform.position - mousePos;

			selectedCards.Add(this);

			if (transform.parent.tag == "Organizing" && transform.parent.childCount > 1)
			{
				for (int i = 0; i < transform.parent.childCount; i++)
				{
					Transform child = transform.parent.GetChild(i);
					if (child.GetSiblingIndex() > transform.GetSiblingIndex())
					{
						selectedCards.Add(child.GetComponent<Card>());
					}
				}
			}

			// Double click to place card on mounting pile
			if (Time.time - lastClickTime < 0.5f)
			{
				GameObject[] targets = GameObject.FindGameObjectsWithTag("Mounting");
				foreach(GameObject target in targets)
				{
					if (target.transform.childCount > 0)
					{
						Transform child = target.transform.GetChild(target.transform.childCount - 1);
						if (CanPlaceCard(selectedCards[0], child.gameObject))
						{
							targetToPlace = child.gameObject;
							break;
						}
					}
					else
					{
						if (CanPlaceCard(selectedCards[0], target))
						{
							targetToPlace = target;
							break;
						}
					}
				}
			}
			lastClickTime = Time.time;
		}
	}

	void OnMouseUp ()
	{
		if (interactable)
		{
			PlaceCard(selectedCards[0], targetToPlace);
		}

		selectedCards.Clear();
	}

	void OnMouseUpAsButton ()
	{
		if (transform.parent.tag == "Draw")
		{
			GameObject newParent = GameObject.FindWithTag("Backup");
			transform.parent = newParent.transform;
			newParent.GetComponent<BackupPile>().OrganizeChild();
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		Transform target = null;
		if (col.transform.parent.tag == "Organizing") // All organizing cards
		{
			if (col.transform.parent != this.transform.parent) // Avoid same parent cards
			{
				target = col.transform.parent.GetChild(col.transform.parent.childCount - 1);
				targetToPlace = target.gameObject; // Last child card is the target
			}
		}
		else if (col.tag == "Organizing") // All organizing blank spaces (parents)
		{
			if (col.transform.childCount > 0) // If it has child cards
			{
				target = col.transform.GetChild(col.transform.childCount - 1);
				if (target != this.transform) // If the last child is not this card
				{
					targetToPlace = target.gameObject; // Last child card is the target
				}
			}
			else // If it has no child cards
			{
				target = col.transform;
				targetToPlace = target.gameObject; // Blank space is the target
			}
		}
		else if (col.transform.parent.tag == "Mounting") // All mounting cards
		{
			target = col.transform.parent.GetChild(col.transform.parent.childCount - 1);
			targetToPlace = target.gameObject; // Last child card is the target
		}
		else if (col.tag == "Mounting") // All mounting blank spaces (parents)
		{
			if (col.transform.childCount > 0) // If it has child cards
			{
				target = col.transform.GetChild(col.transform.childCount - 1);
				targetToPlace = target.gameObject; // Last child card is the target
			}
			else // If it has no child cards
			{
				target = col.transform;
				targetToPlace = target.gameObject; // Blank space is the target
			}
		}
	}

	void OnTriggerStay2D (Collider2D col)
	{
		if (col.transform.parent != transform.parent)
		{
			touchingCol = col;
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if (targetToPlace != null)
		{
			if ((col.gameObject == targetToPlace) || (col.gameObject != targetToPlace && myCollider.IsTouching(targetToPlace.GetComponent<Collider2D>()) == false))
			{
				targetToPlace = null;
			}
			if (touchingCol != null) // If touching other object set it as target
			{
				targetToPlace = touchingCol.gameObject;
			}
		}
	}

	void PlaceCard (Card selectedCard, GameObject target)
	{
		if (CanPlaceCard(selectedCard, target))
		{
			// Player can't place more than 1 card at once in Mounting piles
			if ((target.tag == "Mounting" || target.transform.parent.tag == "Mounting") && selectedCards.Count > 1)
			{
				GameController.instance.OrganizeAllChild();
				return;
			}
			else
			{
				if (target.GetComponent<Card>() != null) // If target is a card (not a blank space)
				{
					foreach (Card card in selectedCards)
					{
						card.transform.parent = target.transform.parent;
					}
				}
				else // Target is a blank space
				{
					foreach (Card card in selectedCards)
					{
						card.transform.parent = target.transform;
					}
				}
			}
		}
			
		GameController.instance.OrganizeAllChild();
	}

	bool CanPlaceCard (Card selectedCard, GameObject target)
	{
		if (selectedCard != null && target != null)
		{
			if (target.GetComponent<Card>() != null) // If target is a card (not a blank space)
			{
				Card t = null;

				// If target is not the last child of its parent, the last child will be the target
				if (target.transform.GetSiblingIndex() == target.transform.parent.childCount - 1)
				{
					t = target.GetComponent<Card>();
				}
				else
				{
					Transform lastChild = target.transform.parent.GetChild(target.transform.parent.childCount - 1);
					t = lastChild.GetComponent<Card>();
				}
				
				/* Return true if target's parent is tagged "Organizing" AND selectedCard 
			    is 1 number lower and has the same color from target*/
				if (t.transform.parent.tag == "Organizing")
				{
					if (selectedCard.color != t.color && selectedCard.number == t.number - 1)
					{
						return true;
					}
				}
				
				/* Return true if target's parent is tagged "Mounting" AND selectedCard is 1 number
			    higher AND has the same suit from target AND only one card is selected*/
				else if (t.transform.parent.tag == "Mounting")
				{
					if (selectedCard.suit == t.suit && selectedCard.number == t.number + 1 && selectedCards.Count == 1)
					{
						return true;
					}
				}
			}
			else // Target is a blank space 
			{
				// If target has child, set the last one as target and restart method
				if (target.transform.childCount > 0)
				{
					target = target.transform.GetChild(target.transform.childCount - 1).gameObject;
					return CanPlaceCard(selectedCard, target);
				}
					
				// Return true if target is tagged "Organizing" AND selectedCard is a "K" card (number 13)
				if (target.tag == "Organizing" && selectedCard.number == 13)
				{
					return true;
				}
				
				/* Return true if target is tagged "Mounting" AND selectedCard is
				an "A" card (number 1) AND only one card is selected*/
				if (target.tag == "Mounting" && selectedCard.number == 1 && selectedCards.Count == 1)
				{
					return true;
				}
			}
		}

		return false; // If none from above returns true, returns false
	}
}