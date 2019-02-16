using UnityEngine;

public class MobPawn : BasePawn
{
    public Interact interactTarget;
    public virtual void SetInteractTarget(Interact target)
    {
        interactTarget = target;
    }
}