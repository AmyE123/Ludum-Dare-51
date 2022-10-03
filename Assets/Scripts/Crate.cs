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

    // This is some recursive nonsense
    private List<Pushable> GetPushablesThatCanMove(Vector3 direction, List<Pushable> willMove, List<Pushable> wontMove, int depth=0)
    {
        if (depth > 50)
        {
            Debug.LogWarning("Hit a recursion depth of 50 uh oh!");
            return willMove;
        }

        bool somethingChanged = false;

        foreach (Pushable child in willMove)
        {
            if (child.CheckIfCanMove(direction, willMove) == false)
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

        foreach (Pushable child in wontMove)
        {
            if (child.CheckIfCanMove(direction, willMove))
            {
                willMove.Add(child);
                somethingChanged = true;
            }
        }

        foreach (Pushable child in willMove)
        {
            if (wontMove.Contains(child))
                wontMove.Remove(child);
        }

        if (somethingChanged)
            GetPushablesThatCanMove(direction, willMove, wontMove, depth + 1);

        return willMove;
    }
}
