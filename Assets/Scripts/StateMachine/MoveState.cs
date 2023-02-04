using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    public override void StartState(Ball playerBall)
    {
        playerBall.MoveWrapper();
    }

    //public override void UpdateState(Ball playerBall)
    //{
    //    throw new System.NotImplementedException();
    //}
}
