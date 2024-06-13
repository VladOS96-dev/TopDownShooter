using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStaticState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        enemy.navMeshAgent.SetDestination(enemy.startingPos);
        enemy.navMeshAgent.isStopped = false;
    }

    public void UpdateState(Enemy enemy)
    {
       
    }

    public void ExitState(Enemy enemy)
    {
        
    }
}