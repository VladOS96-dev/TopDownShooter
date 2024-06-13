using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private float attackTime = 0.4f;
    private Timer attackTimer = new Timer();
    public Transform gunPivot;
    public Projectile projectilePrefab;
    public float projectileSpeed = 10.0f;  // Скорость пули
    private ObjectPool<Projectile> projectilePool;
    private IAttack attackStrategy;

    void Start()
    {
        attackTimer.StartTimer(0.1f);
        projectilePool = new ObjectPool<Projectile>(projectilePrefab, 2);
        ICommand attackCommand = new CommandAttack(gunPivot, projectilePrefab.gameObject, attackTimer, attackTime, 0.1f, gameObject, projectileSpeed, projectilePool);
        attackStrategy = new DefaultAttack(attackCommand);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && attackTimer.IsFinished())
        {
            attackStrategy.Attack();
        }
        attackTimer.UpdateTimer();
    }
}
