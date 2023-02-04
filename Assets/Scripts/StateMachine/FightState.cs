using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightState : State
{
    public override void StartState(Ball playerBall)
    {
        playerBall.FightWrapper();
    }

    //public override void UpdateState(Ball playerBall)
    //{
    //    throw new System.NotImplementedException();
    //}
}
