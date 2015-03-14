using System;
using UnityEngine;
using Svelto.IoC;

public class PathModel:MonoBehaviour
{
	[Inject] public PathController pathController { private get; set; }
	
	public GameObject[] placeHolders;
	
	void Start()
	{
		pathController.pathDTO = this.placeHolders;
		
		GameObject.Destroy(this);
	}
}

