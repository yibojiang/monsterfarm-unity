using UnityEngine;

public class MobPawn : BasePawn
{
    public Interact interactTarget;

    public bool outDoor = true;

    private void Start()
    {
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
}