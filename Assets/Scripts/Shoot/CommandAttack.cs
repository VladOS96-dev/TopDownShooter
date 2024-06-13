using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandAttack : ICommand
{
    private readonly Transform gunPivot;
    private readonly GameObject projectilePrefab;
    private readonly Timer attackTimer;
    private readonly float attackTime;
    private readonly float shakeDuration;
    private readonly GameObject shooter;
    private readonly float projectileSpeed;
    private ObjectPool<Projectile> pool;

    public CommandAttack(Transform gunPivot, GameObject projectilePrefab, Timer attackTimer, float attackTime, float shakeDuration, GameObject shooter, float projectileSpeed, ObjectPool<Projectile> pool)
    {
        this.gunPivot = gunPivot;
        this.projectilePrefab = projectilePrefab;
        this.attackTimer = attackTimer;
        this.attackTime = attackTime;
        this.shakeDuration = shakeDuration;
        this.shooter = shooter;
        this.projectileSpeed = projectileSpeed;
        this.pool = pool;
    }

    public void Execute()
    {
        CameraFollow.ToggleShake(shakeDuration);
        Projectile bullet = pool.Get();
        bullet.transform.position = gunPivot.position;
        bullet.transform.rotation = gunPivot.rotation;
        bullet.Initialize(shooter, projectileSpeed,  CollisionTarget.ENEMIES, pool);  // Инициализация скорости и стрелка
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = gunPivot.position.y;
        bullet.transform.LookAt(mousePos);
        bullet.transform.Rotate(0, Random.Range(-7.5f, 7.5f), 0);
        attackTimer.StartTimer(attackTime);
    }
}
