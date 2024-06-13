using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAttackCommand : ICommand
{
    private Transform gunPivot;
    private GameObject projectilePrefab;
    private float projectileSpeed;
    private GameObject shooter;
    private ObjectPool<Projectile> pool;
    public DefaultAttackCommand(Transform gunPivot, GameObject projectilePrefab, GameObject shooter, float projectileSpeed, ObjectPool<Projectile> pool)
    {
        this.gunPivot = gunPivot;
        this.projectilePrefab = projectilePrefab;
        this.projectileSpeed = projectileSpeed;
        this.shooter = shooter;
        this.pool = pool;
    }

    public void Execute()
    {
        Projectile bullet =pool.Get();
        bullet.transform.position=gunPivot.position;
        bullet.transform.rotation=gunPivot.rotation;
        bullet.Initialize(shooter, projectileSpeed,CollisionTarget.PLAYER,pool);  
    }
}
