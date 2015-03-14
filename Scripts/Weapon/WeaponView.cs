using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Svelto.IoC;
using System.Collections.Generic;

public class WeaponView : MonoBehaviour
{
    [Inject] public WeaponPresenter weapon { private get; set; }
	// Use this for initialization
	void Start () 
	{
        _tweener = HOTween.To(this.transform, 4, new TweenParms()
           .Prop("rotation", new Vector3(0, 180, 0))
           .Ease(EaseType.EaseInOutQuad)
           .Loops(-1, LoopType.Yoyo)
         );
		
		PlayIdle();

        weapon.SetView(this);
	}

	public void PlayIdle()
	{
		_tweener.Play();
	}
	
	void Update()
	{
        weapon.Update();
	}
	
	public void PauseTweener()
	{
        _tweener.Pause();
	}
	
	WeaponState 		_currentViewState;
	Tweener				_tweener;
    GameObject          _lockedTarget;
}
