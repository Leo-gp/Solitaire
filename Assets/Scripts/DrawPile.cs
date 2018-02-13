using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPile : MonoBehaviour 
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
				GameController.instance.FlipCardToBack(child.gameObject);
			}
		}
	}
		
	void OnMouseUpAsButton ()
	{
		Transform backupPile = GameObject.FindWithTag("Backup").transform;
		int cc = backupPile.childCount;
		for (int i = 0; i < cc; i++)
		{
			Transform child = backupPile.GetChild(backupPile.childCount - 1);
			child.parent = this.transform;
		}
		OrganizeChild();
	}
}