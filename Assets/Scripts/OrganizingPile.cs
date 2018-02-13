using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganizingPile : MonoBehaviour 
{
	public void OrganizeChild ()
	{
		if (transform.childCount > 0)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				child.position = new Vector3(transform.position.x, transform.position.y - i * (0.4f), transform.position.z - i - 1);
				child.GetComponent<Card>().targetToPlace = null;
			}
			GameObject lastChild = transform.GetChild(transform.childCount - 1).gameObject;
			GameController.instance.FlipCardToFront(lastChild.gameObject);
		}
	}

	public void OrganizeChildOnStart ()
	{
		if (transform.childCount > 0)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				child.position = new Vector3(transform.position.x, transform.position.y - i * (0.4f), transform.position.z - i - 1);
				if (i != transform.childCount - 1)
				{
					GameController.instance.FlipCardToBack(child.gameObject);
				}
			}
		}
	}
}