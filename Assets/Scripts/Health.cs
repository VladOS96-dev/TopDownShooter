using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]private CollisionTarget collisionTarget;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ammo"))
        {

        Projectile  projectile = collision.gameObject.GetComponent<Projectile>();
            DamagePlayer(projectile.collisionTarget);
        }
    }
    public void DamagePlayer(CollisionTarget projectileCollisionTarget)
    {

        if (projectileCollisionTarget == collisionTarget)
        {
            GlobalEventHit.InvokeOnHit(collisionTarget);
            CameraFollow.ToggleShake(0.3f);
        }
    }
}
