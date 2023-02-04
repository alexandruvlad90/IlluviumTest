using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatePathState : State
{
    public override void StartState(Ball playerBall)
    {
        playerBall.CalculatePath();
    }

    //public override void UpdateState(Ball playerBall)
    //{
    //    throw new System.NotImplementedException();
    //}
}
