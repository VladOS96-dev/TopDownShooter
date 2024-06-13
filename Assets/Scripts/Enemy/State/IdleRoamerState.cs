using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRoamerState : IEnemyState
{
    private Timer idleTimer = new Timer();
    private Timer idleRotateTimer = new Timer();
    private bool idleWaiting, idleMoving;

    public void EnterState(Enemy enemy)
    {
        enemy.navMeshAgent.speed = 7.0f;
        idleTimer.StartTimer(Random.Range(2.0f, 4.0f));
        enemy.RandomRotate();
        AdvanceIdle(enemy);
        idleWaiting = false;
        idleMoving = true;
    }

    public void UpdateState(Enemy enemy)
    {
        idleTimer.UpdateTimer();

        if (idleMoving)
        {
            if (enemy.HasReachedMyDestination())
            {
                AdvanceIdle(enemy);
            }
        }
        else if (idleWaiting)
        {
            idleRotateTimer.UpdateTimer();
            if (idleRotateTimer.IsFinished())
            {
                enemy.RandomRotate();
                idleRotateTimer.StartTimer(Random.Range(1.5f, 3.25f));
            }
        }

        if (idleTimer.IsFinished())
        {
            if (idleMoving)
            {
                enemy.navMeshAgent.isStopped = true;
                float waitTime = Random.Range(2.5f, 6.5f);
                float randomTurnTime = waitTime / 2.0f;
                idleRotateTimer.StartTimer(randomTurnTime);
                idleTimer.StartTimer(waitTime);
            }
            else if (idleWaiting)
            {
                idleTimer.StartTimer(Random.Range(2.0f, 4.0f));
                AdvanceIdle(enemy);
            }

            idleMoving = !idleMoving;
            idleWaiting = !idleMoving;
        }
    }

    public void ExitState(Enemy enemy)
    {
        
    }

    private void AdvanceIdle(Enemy enemy)
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.transform.position, enemy.transform.forward * 5.0f, out hit, 50.0f, enemy.hitTestLayer))
        {
            if (hit.distance < 3.0f)
            {
                Vector3 dir = hit.point - enemy.transform.position;
                Vector3 reflectedVector = Vector3.Reflect(dir, hit.normal);
                Physics.Raycast(enemy.transform.position, reflectedVector, out hit, 50.0f, enemy.hitTestLayer);
            }

            enemy.navMeshAgent.isStopped = false;
            enemy.navMeshAgent.SetDestination(hit.point);
        }
    }
}