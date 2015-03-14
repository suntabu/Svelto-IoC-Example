using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Svelto.IoC;
using Svelto.Ticker;

//Main is the Application Composition Root.
//Composition Root is the place where the framework can be initialised.

public class Main:ICompositionRoot
{
	public Svelto.IoC.IContainer container { get; private set; }
	
	public Main()
	{
		HOTween.Init(false, false, true);
		HOTween.EnableOverwriteManager();
		
		SetupContainer();
		StartGame();
	}
	
	void SetupContainer()
	{
		container = new Container();
		
		container.Bind<IGameObjectFactory>().AsSingle<GameObjectFactory>(new GameObjectFactory(container));
        container.Bind<IMonsterCounter>().AsSingle<UnderAttackSystem>();

        container.Bind<WeaponPresenter>().ToFactory(new MultiProvider<WeaponPresenter>());
        container.Bind<MonsterPresenter>().ToFactory(new MultiProvider<MonsterPresenter>());

        container.BindSelf<UnderAttackSystem>();
		container.BindSelf<PathController>();
		container.BindSelf<MonsterSpawner>();
	}
	
	void StartGame()
	{
		//tickEngine could be added in the container as well
		//if needed to other classes!
        UnityTicker tickEngine = new UnityTicker(); 
		
		tickEngine.Add(container.Build<MonsterSpawner>());
        tickEngine.Add(container.Build<UnderAttackSystem>());
	}
}

//A GameObject containing GameContext must be present in the scene
//All the monobehaviours present in the scene file that need dependencies 
//injected must be component of GameObjects children of GameContext.

public class GameContext: UnityRoot<Main>
{
}
