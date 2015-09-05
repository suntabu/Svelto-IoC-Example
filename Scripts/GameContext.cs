using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Svelto.IoC;
using Svelto.Ticker;
using Svelto.Context;
using System;
using Svelto.Factories;

//Main is the Application Composition Root.
//Composition Root is the place where the framework can be initialised.

public class Main:ICompositionRoot, IUnityContextHierarchyChangedListener
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
        
        //interface is bound to a specific instance
        container.Bind<IGameObjectFactory>().AsSingle(new GameObjectFactory(this));
        //interfaces are bound to specific implementations, the same instance will be used once created
        container.Bind<IMonsterCounter>().AsSingle<MonsterCountHolder>();
        container.Bind<IMonsterCountHolder>().AsSingle<MonsterCountHolder>();
        //once the dependency is requested, a new instance will be created
        container.Bind<WeaponPresenter>().ToFactory(new MultiProvider<WeaponPresenter>());
        container.Bind<MonsterPresenter>().ToFactory(new MultiProvider<MonsterPresenter>());
        container.Bind<MonsterPathFollower>().ToFactory(new MultiProvider<MonsterPathFollower>());
        //once requested, the same instance will be used
        container.BindSelf<UnderAttackSystem>();
        container.BindSelf<PathController>();
    }
    
    void StartGame()
    {
        UnityTicker tickEngine = new UnityTicker(); //note this object can be safely garbage collected
        
        tickEngine.Add(container.Inject(new MonsterSpawner()));
        tickEngine.Add(container.Build<UnderAttackSystem>());
    }

    public void OnContextInitialized()
    {
    }

    public void OnContextDestroyed()
    {
    }
/// <summary>
/// Context and Container are unbound
/// It's the Context responsability to decide
/// if the container must be used on new Monobehaviours
/// </summary>
/// <param name="component"></param>
    public void OnMonobehaviourAdded(MonoBehaviour component)
    {
        container.Inject(component);
    }

    public void OnMonobehaviourRemoved(MonoBehaviour component)
    {
    }

    public void OnGameObjectAdded(GameObject entity)
    {
    }

    public void OnGameObjectRemoved(GameObject entity)
    {
    }
}

//A GameObject containing GameContext must be present in the scene
//All the monobehaviours present in the scene file that need dependencies 
//injected must be component of GameObjects children of GameContext.

public class GameContext: UnityContext<Main>
{
}
