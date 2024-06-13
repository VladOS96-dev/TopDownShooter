using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    private Timer attackActionTimer = new Timer();
    private bool actionDone;

    public void EnterState(Enemy enemy)
    {
        enemy.navMeshAgent.isStopped = true;
        enemy.navMeshAgent.velocity = Vector3.zero;
        enemy.StartAttacking();
        attackActionTimer.StartTimer(enemy.weaponTime);
        actionDone = false;

    }

    public void UpdateState(Enemy enemy)
    {
        attackActionTimer.UpdateTimer();
        if (!actionDone && attackActionTimer.IsFinished())
        {
            EndAttack(enemy);
            actionDone = true;
        }
    }

    public void ExitState(Enemy enemy)
    {
        enemy.StopAttacking();
    }

    private void EndAttack(Enemy enemy)
    {
        enemy.SetState(NPC_EnemyState.INSPECT);
    }
}
