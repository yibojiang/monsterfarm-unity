using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MobAttributeData
{
    public int hp = 2;
    public int hitDamage = 1;
}

public class MobPawn : BasePawn
{
    public List<MobPawn> Followers { get; private set; }
    public Interact interactTarget;

    public bool outDoor = true;
    protected Vector2 _movingVel;
    public float maxMovingSpeed = 1.0f;
    protected Rigidbody2D _rigidBody;
    public int maxHp;
    public int Hp;
    protected SpriteRenderer _sprite;
    protected Animator _animSm;
    public int _idAlive;

    public int HitDamage
    {
        get { return data.hitDamage; }
    }

    public bool alive = true;
    

    public MobAttributeData data;
    protected void Awake()
    {
        _sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        _animSm = _sprite.gameObject.GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        maxHp = data.hp;
        Hp = maxHp;
        
        foreach (var p in _animSm.parameters)
        {
            if (p.name == "Alive")
            {
                _idAlive = Animator.StringToHash("Alive");
            }
        }
    }

    public virtual void SetDestination(Vector3 targetPosition, float minDest)
    {
        
    }

    public virtual bool DestinationReached()
    {
        return true;
    }

    public virtual bool DestinationCannotReached()
    {
        return false;
    }

    protected void Start()
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

    public virtual void Hurt(Vector2 pos, int damage)
    {
        if (alive)
        {
            Hp -= damage;
            Debug.Log($"hurt: {damage}, Hp: {Hp}");
            if (Hp <= 0)
            {
                Die();
            }    
        }
    }

    public virtual void Die()
    {
        Debug.Log("Die");
        alive = false;
        if (_idAlive != 0)
        {
            _animSm.SetBool(_idAlive, false);    
        }
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        _rigidBody.MovePosition(_rigidBody.position + _movingVel * Time.fixedDeltaTime);
    }
}
