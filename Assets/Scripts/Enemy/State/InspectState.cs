using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectState : IEnemyState
{
    private Timer inspectTimer = new Timer();
    private Timer inspectTurnTimer = new Timer();
    private bool inspectWait;

    public void EnterState(Enemy enemy)
    {
        enemy.navMeshAgent.speed = 16.0f;
        enemy.navMeshAgent.isStopped = false;
        inspectTimer.StopTimer();
        inspectWait = false;
      
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.HasReachedMyDestination() && !inspectWait)
        {
            inspectWait = true;
            inspectTimer.StartTimer(2.0f);
            inspectTurnTimer.StartTimer(1.0f);
        }

        enemy.navMeshAgent.SetDestination(enemy.targetPos);
        RaycastHit hit;
        if (Physics.Raycast(enemy.transform.position, enemy.transform.forward, out hit, enemy.weaponRange, enemy.hitTestLayer))
        {
          
            if (hit.collider != null && hit.collider.tag == "Player")
            {

                enemy.SetState(NPC_EnemyState.ATTACK);
            }
        }


        if (inspectWait)
        {
            inspectTimer.UpdateTimer();
            inspectTurnTimer.UpdateTimer();
            if (inspectTurnTimer.IsFinished())
            {

                enemy.RandomRotate();
                inspectTurnTimer.StartTimer(Random.Range(0.5f, 1.25f));
            }
            if (inspectTimer.IsFinished())
            {
           
                enemy.SetState(enemy.idleState);
            }
        }
    }

    public void ExitState(Enemy enemy)
    {
      
    }
}
