using System.Collections.Generic;
using UnityEngine;

public class MobPawn : BasePawn
{
    public List<MobPawn> Followers { get; private set; }
    public Interact interactTarget;

    public bool outDoor = true;
    protected Vector2 _movingVel;
    public float maxMovingSpeed = 1.0f;
    protected Rigidbody2D _rigidBody;

    protected void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

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

    public virtual void SetVel(Vector2 movingVel)
    {
        this._movingVel = movingVel;
    }

    void FixedUpdate()
    {
        _rigidBody.MovePosition(_rigidBody.position + _movingVel * Time.fixedDeltaTime);
    }
}