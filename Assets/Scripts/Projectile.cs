using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CollisionTarget { PLAYER, ENEMIES,NONE }
public class Projectile : MonoBehaviour
{
    
    public CollisionTarget collisionTarget;
    public float speed = 1.5f;
    private Rigidbody rb;
    private GameObject shooter;
    private ObjectPool<Projectile> pool;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GlobalEventHit.OnHit += DestroyProjectile;
    }

    void Update()
    {
        rb.velocity = transform.forward * speed*Time.deltaTime;
    }

    public void Initialize(GameObject shooter, float projectileSpeed, CollisionTarget collisionTarget, ObjectPool<Projectile> pool)
    {
        this.shooter = shooter;
        speed = projectileSpeed;
        this.collisionTarget = collisionTarget;
        this.pool = pool;
    }

    void OnCollisionEnter(Collision collision)
    {

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);
        transform.rotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);
    }
 
    public void DestroyProjectile(CollisionTarget collisionTarget)
    {
        pool.ReturnToPool(this);
    }
    
    private void OnDestroy()
    {
        GlobalEventHit.OnHit -= DestroyProjectile;
    }
}
