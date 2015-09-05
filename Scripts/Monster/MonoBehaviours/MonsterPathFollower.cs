using System;
using UnityEngine;
using Holoville.HOTween;
using Svelto.IoC;

public class MonsterPathFollower
{
	[Inject] public PathController		pathController 			{ set; private get; }

    public event System.Action OnPathEnded;
	
	public void Start(Transform transform)
	{
        _transform = transform;
		_transform.position = pathController.CheckPoint(_currentCheckPoint);
		
		MoveNext();
	}
	
	void MoveNext()
	{
		if (pathController.IsEndReached(_currentCheckPoint) == false)
		{
            TweenParms paramaters = new TweenParms().Prop("position", pathController.CheckPoint(_currentCheckPoint + 1)).Ease(EaseType.Linear).OnComplete(MoveNext);
			
			_tweener = HOTween.To(_transform, 2, paramaters);
			
			_tweener.Play();
			
			_currentCheckPoint++;
		}
		else
		{
            OnPathEnded();
		}
	}

    internal void CleanUP()
    {
        _tweener.Kill();
    }

	
	int              _currentCheckPoint = 0;
    Transform        _transform;
    Tweener         _tweener;
}

