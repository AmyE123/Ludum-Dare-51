using System.Collections;
using System.Collections.Generic;
using ThirdPersonMovement;
using UnityEngine;

public class Crate : Pushable
{
    public override void Push(Vector3 direction, PersonPushController player)
    {
        if (CheckIfCanMove(direction) == false)
        {
            player.CancelPushing();
            return;
        }

        List<Pushable> children = CheckPushablesOnTop();
        StartCoroutine(PushRoutine(direction, player, children));
    }

}
