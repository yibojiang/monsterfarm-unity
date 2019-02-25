using System.Collections.Generic;
using UnityEngine;

public class MobPawn : BasePawn
{
    public Interact interactTarget;

    public bool outDoor = true;

    public List<MobPawn> Followers { get; private set; }

    private void Start()
    {
        Followers = new List<MobPawn>();
        if (outDoor) {
            transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }
        else {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public virtual void SetInteractTarget(Interact target)
    {
        interactTarget = target;
    }

    public virtual void AddFollower(MobPawn mob)
    {
        Followers.Add(mob);
    }
}