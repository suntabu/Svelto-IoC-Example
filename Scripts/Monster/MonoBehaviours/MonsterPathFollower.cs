using System;
using UnityEngine;
using Holoville.HOTween;
using Svelto.IoC;

public class MonsterPathFollower
{
	[Inject] public PathController		pathController 			{ set; private get; }
	
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
			
			Tweener tweener = HOTween.To(_transform, 2, paramaters);
			
			tweener.Play();
			
			_currentCheckPoint++;
		}
		else
		{
            _monster.CommitSuicide(); //in a real project I would have used a command, not directly the presenter
		}
	}

    public void SetMonster(MonsterPresenter monster)
    {
        _monster = monster;
    }
	
	int              _currentCheckPoint = 0;
    MonsterPresenter _monster;
    Transform        _transform;
}

