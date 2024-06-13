using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    [SerializeField] private Transform playerPointRespawn;
    [SerializeField] private Transform enemyPointRespawn;
    [SerializeField] private GameObject enemy;
    [SerializeField] private PlayerMovement player;
    void OnEnable()
    {
        GlobalEventHit.OnHit += ResetPositionCharacter;
        ResetPositionCharacter(CollisionTarget.PLAYER);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetPositionCharacter(CollisionTarget collisionTarget)
    {
        enemy.transform.position = enemyPointRespawn.position;
        player.transform.position = playerPointRespawn.position;
        player.ResetPosition();
    }
    private void OnDisable()
    {
        GlobalEventHit.OnHit -= ResetPositionCharacter;
    }
}
