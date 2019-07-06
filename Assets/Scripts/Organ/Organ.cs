using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Organ : MonoBehaviour
{

	public void OnActivate()
	{

	}

	public abstract void OnUse(GameObject player);

}
