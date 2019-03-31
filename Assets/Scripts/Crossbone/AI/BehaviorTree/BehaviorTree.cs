using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Crossbone.AI.BehaviorTree
{
    public class BehaviorTree
    {
        private MonoBehaviour _container;
        private Node _root;
        private float _updateInterval;
        private bool _isRunning = false;
        private Dictionary<string, object> _data;

        public static BehaviorTree CreateWonderChaseBehavior(MobPawn mob)
        {
            var data = new Dictionary<string, object>();
            data.Add("move_loc", new Vector3(0, 0, 0));
            data.Add("is_chasing", false);
            data.Add("is_moving", false);
            data.Add("is_waiting", false);
            data.Add("wait_timer", 0f);
            data.Add("wait_interval", 2f);
            float updateInterval = 0.5f;
            ActionNode randomLocation = new ActionNode(() =>
            {   
                if ((bool)data["is_moving"] != true)
                {
                    var rad = Random.Range(0f, 2*Mathf.PI);
                    var len = Random.Range(1.5f, 2.5f);
                    data["move_loc"] = mob.transform.position + len * new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
                    Debug.Log(data["move_loc"]);
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
                    mob.SetDestination((Vector3)data["move_loc"], 0.1f);
                }
                
                if (mob.DestinationReached() || mob.DestinationCannotReached())
                {
                    Debug.Log("DestinationEnded");
                    data["is_moving"] = false;
                    return NodeState.Success;
                }
                else
                {   
                    Debug.Log("Moving");
                    return NodeState.Running;
                }
            });
            
            ActionNode wait = new ActionNode(() =>
            {
                if ((bool) data["is_waiting"] == false)
                {
                    data["is_waiting"] = true;
//                    data["wait_interval"] = Random.Range(1.5f, 2.5f);
                    data["wait_interval"] = 2f;
                }

                data["wait_timer"] = (float)data["wait_timer"] + updateInterval;
                Debug.Log(data["wait_timer"]);
                if ((float)data["wait_timer"] >= (float)data["wait_interval"])
                {
                    data["wait_timer"] = 0f;
                    data["is_waiting"] = false;
                    return NodeState.Success;
                }
                
                Debug.Log("Waiting");
                return NodeState.Running;
            });

            ActionNode attack = new ActionNode(() => { return NodeState.Success; });
            
            Sequence wonder = new Sequence(new List<Node>(){randomLocation, moveTo, wait});
//            Sequence chase = new Sequence(new List<Node>());   
//            Node root = new Selector(new List<Node>(){wonder, chase});
            Node root = new Selector(new List<Node>(){wonder});
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