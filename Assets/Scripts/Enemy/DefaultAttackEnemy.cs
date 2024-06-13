using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAttackEnemy : IAttackEnemy
{ 
    private ICommand attackCommand;

    public DefaultAttackEnemy(ICommand attackCommand)
    {
        this.attackCommand = attackCommand;
    }

    public void Attack(Enemy enemy)
    {
        attackCommand.Execute();
    }
}
