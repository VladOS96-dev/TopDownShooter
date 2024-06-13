using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ammo"))
        { collision.collider.GetComponent<Projectile>().DestroyProjectile(CollisionTarget.NONE); }
    }
}
