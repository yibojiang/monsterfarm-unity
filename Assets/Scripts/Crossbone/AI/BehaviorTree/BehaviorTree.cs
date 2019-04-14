using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Crossbone.AI.BehaviorTree
{
    public class BehaviorTree
    {
        private MonoBehaviour _container;
        private Node _root;
        private float _updateInterval;
        private bool _isRunning = false;
        private Dictionary<string, object> _data;

        public static BehaviorTree CreateWanderBehavior(MobPawn mob)
        {
            var data = new Dictionary<string, object>();
            data.Add("move_loc", new Vector3(0, 0, 0));
            data.Add("is_moving", false);
            data.Add("is_waiting", false);
            data.Add("wait_timer", 0f);
            data.Add("wait_interval", 2f);
            float updateInterval = 0.5f;
            ActionNode randomLocation = new ActionNode(() =>
            {
                if ((bool)data["is_moving"] != true)
                {
                    var rad = Random.Range(0f, 2 * Mathf.PI);
                    var len = Random.Range(1.5f, 2.5f);
                    data["move_loc"] = mob.transform.position + len * new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
                }

                return NodeState.Success; 
            });

            ActionNode moveTo = new ActionNode(() =>
            {
                if ((bool) data["is_waiting"] == true)
                {
                    return NodeState.Success;
                }

                if ((bool)data["is_moving"] == false)
                {
                    mob.SetDestination((Vector3)data["move_loc"], 0.1f, 0.6f);
                    data["is_moving"] = true;
                }
                
                if (mob.DestinationReached() || mob.DestinationCannotReached())
                {
//                    Debug.Log("DestinationEnded");
                    data["is_moving"] = false;
                    return NodeState.Success;
                }
                else
                {   
//                    Debug.Log("Moving");
                    return NodeState.Running;
                }
            });
            
            ActionNode wait = new ActionNode(() =>
            {
                if ((bool) data["is_waiting"] == false)
                {
                    data["is_waiting"] = true;
                    data["wait_interval"] = Random.Range(1.5f, 2.5f);
                }

                data["wait_timer"] = (float)data["wait_timer"] + updateInterval;
                if ((float)data["wait_timer"] >= (float)data["wait_interval"])
                {
                    data["wait_timer"] = 0f;
                    data["is_waiting"] = false;
                    return NodeState.Success;
                }
                
//                Debug.Log("Waiting");
                return NodeState.Running;
            });
            
            Sequence wander = new Sequence(new List<Node>(){randomLocation, moveTo, wait});
            Node root = new Selector(new List<Node>(){wander});
            BehaviorTree bt = new BehaviorTree(root, data, updateInterval, mob); 
            return bt;
        }

        public static BehaviorTree CreateWanderChaseBehavior(MobPawn mob)
        {
            var data = new Dictionary<string, object>();
            data.Add("move_loc", new Vector3(0, 0, 0));
            data.Add("is_moving", false);
            data.Add("is_waiting", false);
            data.Add("wait_timer", 0f);
            data.Add("wait_interval", 2f);
            data.Add("chase_target", null);
            float updateInterval = 0.5f;

            ActionNode detect = new ActionNode(() =>
            {
                if (data["chase_target"] != null)
                {
                    return NodeState.Failure;
                }
                else
                {
                    return NodeState.Success;
                }
            });
                
            ActionNode randomLocation = new ActionNode(() =>
            {
                if ((bool)data["is_moving"] != true)
                {
                    var rad = Random.Range(0f, 2 * Mathf.PI);
                    var len = Random.Range(1.5f, 2.5f);
                    data["move_loc"] = mob.transform.position + len * new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
                }

                return NodeState.Success; 
            });

            ActionNode moveTo = new ActionNode(() =>
            {
                if ((bool) data["is_waiting"] == true)
                {
                    return NodeState.Success;
                }

                if ((bool)data["is_moving"] == false)
                {
                    data["is_moving"] = true;
                    mob.SetDestination((Vector3)data["move_loc"], 0.1f, 0.8f);
                }
                
                if (mob.DestinationReached() || mob.DestinationCannotReached())
                {
//                    Debug.Log("DestinationEnded");
                    data["is_moving"] = false;
                    return NodeState.Success;
                }
                else
                {   
//                    Debug.Log("Moving");
                    return NodeState.Running;
                }
            });
            
            ActionNode wait = new ActionNode(() =>
            {
                if ((bool) data["is_waiting"] == false)
                {
                    data["is_waiting"] = true;
                    data["wait_interval"] = Random.Range(1.5f, 2.5f);
                }

                data["wait_timer"] = (float)data["wait_timer"] + updateInterval;
                if ((float)data["wait_timer"] >= (float)data["wait_interval"])
                {
                    data["wait_timer"] = 0f;
                    data["is_waiting"] = false;
                    return NodeState.Success;
                }
                
//                Debug.Log("Waiting");
                return NodeState.Running;
            });

            ActionNode hasTarget = new ActionNode(() =>
            {
                if (data["chase_target"] != null)
                {
                    return NodeState.Success;
                }
                else
                {
                    return NodeState.Failure;
                }
            });
            
            ActionNode chase = new ActionNode(() =>
            {   
                Transform target = (Transform)data["chase_target"];
                mob.SetDestination(target.position, 0.1f, 1.8f);

                var dist = target.position - mob.transform.position;
                if (dist.magnitude > 10f)
                {
                    data["chase_target"] = null;
                    return NodeState.Failure;
                }
                
                if (mob.DestinationReached() || mob.DestinationCannotReached())
                {
                    Debug.Log("ChaseEnded");
                    return NodeState.Success;
                }
                else
                {   
                    Debug.Log("Chasing");
                    return NodeState.Running;
                }
            });

            ActionNode attack = new ActionNode(() => { return NodeState.Success; });
            
            Sequence wander = new Sequence(new List<Node>(){detect, randomLocation, moveTo, wait});
            Sequence chaseAttack = new Sequence(new List<Node>(){hasTarget, chase, attack});   
            Node root = new Selector(new List<Node>(){wander, chaseAttack});
            BehaviorTree bt = new BehaviorTree(root, data, updateInterval, mob); 
            return bt;
        }

        BehaviorTree(Node root, Dictionary<string, object> data, float updateInterval, MonoBehaviour container)
        {
            _root = root;
            _updateInterval = updateInterval;
            _container = container;
            _data = data;
//            root.SetBehaviorTree(this);
        }

        public void SetKeyValue<T>(string key, T value)
        {
            _data[key] = (object)value;
        }

        [CanBeNull]
        public T GetValue<T>(string key)
        {
            if (!_data.ContainsKey(key) || _data[key] == null)
            {
                Debug.LogError("Key doesn't exist");
            }

            return (T) _data[key];
        }

        public bool HasKey(string key)
        {
            return _data.ContainsKey(key) && _data[key] != null;
        }

        public IEnumerator Evaluate()
        {
            while (true)
            {
                NodeState state = _root.Evaluate();
                yield return new WaitForSeconds(_updateInterval);
            }
        }
        
        public void Run()
        {
            _isRunning = true;
            _container.StartCoroutine(Evaluate());
        }

        public void Stop()
        {
            _isRunning = false;
            _container.StopCoroutine(Evaluate());
        }
    }
}