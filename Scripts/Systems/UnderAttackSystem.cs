using System;
using Svelto.IoC;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Svelto.Ticker;

public class UnderAttackSystem: ITickable, IMonsterCounter
{
    public UnderAttackSystem()
	{
        _freeWeapons = new List<WeaponPresenter>();
        _monsters = new HashSet<MonsterPresenter>();
        _monstersDic = new Dictionary<Transform, MonsterPresenter>();
	}

    //IMonsterCounter Interface

    public int monsterCount { get { return _monsters.Count; } }

    //ITickable Interface

    public void Tick(float delta)
    {
        CheckTargets();
    }

    /// <summary>
    /// public methods
    /// </summary>

    public void AddWeapon(WeaponPresenter weapon)
    {
        _freeWeapons.Add(weapon);
    }

    public void AddMonster(MonsterPresenter monster)
    {
        _monsters.Add(monster);
        _monstersDic[monster.target] = monster;

        monster.OnKilled += OnMonsterKilled;
    }

    void CheckTargets()
    {
        foreach (MonsterPresenter currentMonster in _monsters)
        {
            for (int i = 0; i < _freeWeapons.Count; i++)
            {
                WeaponPresenter currentWeapon = _freeWeapons[i];

                if (currentWeapon.CheckAndAcquireTarget(currentMonster.target) == true)
                {
                    currentMonster.StartBeingHit();
                    
                    currentWeapon.OnTargetNotFound += TargetOutOfRange;

                    _freeWeapons.RemoveAt(i);

                    return;
                }
            }
        }
    }

    void OnMonsterKilled(MonsterPresenter monster)
    {
        monster.OnKilled -= OnMonsterKilled;

        _monsters.Remove(monster);
        _monstersDic.Remove(monster.target);
    }

    void TargetOutOfRange(WeaponPresenter weapon)
    {
        weapon.OnTargetNotFound -= TargetOutOfRange;

        _monstersDic[weapon.target].StopBeingHit();

        AddWeapon(weapon);
    }

    List<WeaponPresenter>       _freeWeapons; 
    HashSet<MonsterPresenter>   _monsters;

    Dictionary<Transform, MonsterPresenter> _monstersDic;
}
