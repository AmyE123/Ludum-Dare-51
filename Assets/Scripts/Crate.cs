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
        List<Pushable> movingChildren = GetPushablesThatCanMove(direction, new List<Pushable>(children), new List<Pushable>());

        if (children.Count != movingChildren.Count)
            movingChildren = CheckPushablesOnTop(null, movingChildren);

        StartCoroutine(PushRoutine(direction, player, movingChildren));
    }


    private List<Pushable> GetPushablesThatCanMove(Vector3 direction, List<Pushable> willMove, List<Pushable> wontMove)
    {
        bool somethingChanged = false;

        foreach (Pushable child in willMove)
        {
            if (child.CheckIfCanMove(direction) == false)
            {
                wontMove.Add(child);
                somethingChanged = true;
            }
        }

        foreach (Pushable child in wontMove)
        {
            if (willMove.Contains(child))
                willMove.Remove(child);
        }

        return willMove;
    }
}
