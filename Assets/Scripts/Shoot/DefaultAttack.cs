using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAttack : IAttack
{
    private readonly ICommand attackCommand;

    public DefaultAttack(ICommand attackCommand)
    {
        this.attackCommand = attackCommand;
    }

    public void Attack()
    {
        attackCommand.Execute();
    }
}