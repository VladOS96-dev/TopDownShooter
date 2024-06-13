using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_Manager : MonoBehaviour
{
    public int scorePlayer = 0;
    public int scoreEnemy = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    void OnEnable()
    {
        GlobalEventHit.OnHit += UpdateScore;
    }

    public void UpdateScore(CollisionTarget collisionTarget)
    {
        switch (collisionTarget)
        { 
        case CollisionTarget.ENEMIES: scorePlayer++; break;
        case CollisionTarget.PLAYER: scoreEnemy++; break;
        
        }
        scoreText.text=$"{scorePlayer}:{scoreEnemy}";
    }
    private void OnDisable()
    {
        GlobalEventHit.OnHit -= UpdateScore;
    }
}
