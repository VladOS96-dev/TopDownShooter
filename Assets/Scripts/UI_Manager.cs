using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_Manager : MonoBehaviour
{
    public int scorePlayer = 0;
    public int scoreEnemy = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI infoText;
    public int count = 0;
    void OnEnable()
    {
        GlobalEventHit.OnHit += UpdateScore;
        GlobalEventHit.OnInfo += UpdateInfo;
    }
    public void UpdateInfo(string info)
    {
        infoText.text += info + '\n';
        count++;
        if (count == 10)
        { count = 0;
            infoText.text = info + '\n';
        }
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
        GlobalEventHit.OnInfo -= UpdateInfo;
    }
}
