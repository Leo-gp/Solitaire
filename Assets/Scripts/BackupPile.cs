using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackupPile : MonoBehaviour 
{
	public void OrganizeChild ()
	{
		if (transform.childCount > 0)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				child.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - i - 1);
				child.GetComponent<Card>().targetToPlace = null;
				GameController.instance.FlipCardToFront(child.gameObject);
			}
		}
	}
}