using Svelto.IoC;
using Svelto.Ticker;
using UnityEngine;

public class MonsterSpawner:ITickable, IInitialize
{
	[Inject] public IGameObjectFactory  gameObjectFactory   { set; private get; }
    [Inject] public UnderAttackSystem   underAttackSystem   { set; private get; }
    [Inject] public IMonsterCounter     monsterCounter      { set; private get; }
	
	public MonsterSpawner()
	{
        _monstersRoot = new GameObject("Monsters");
		_frequency = 3;
		_timeLapsed = _frequency;
	}
	
	public void OnDependenciesInjected()
	{
        DesignByContract.Check.Require(underAttackSystem != null);
        DesignByContract.Check.Require(monsterCounter != null);
        DesignByContract.Check.Require(gameObjectFactory != null);
	}
	
	public void Tick(float delta)
	{
		_timeLapsed += delta;
		
		if (_timeLapsed >= _frequency)
		{
            if (monsterCounter.monsterCount < 5)
            {
                GameObject monster = gameObjectFactory.Build(CreateMonster());

                monster.transform.parent = _monstersRoot.transform;
            }
			_timeLapsed = 0;
			_frequency = UnityEngine.Random.Range(0.5f, 4.0f);
		}
	}

    GameObject CreateMonster()
    {
        return _originalGO;
    }

	float       _frequency;
	float       _timeLapsed;
    GameObject  _monstersRoot;

    static GameObject _originalGO = Resources.Load("Monster") as GameObject;
}

