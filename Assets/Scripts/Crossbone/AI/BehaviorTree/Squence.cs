using System.Collections.Generic;
using JetBrains.Annotations;

namespace Crossbone.AI.BehaviorTree
{
    public class Sequence : Node
    {
        protected List<Node> nodes;
        public Sequence(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            bool running = false;
            for (int i = 0; i < nodes.Count; i++)
            {
                NodeState childState = nodes[i].Evaluate();
                if (childState == NodeState.Failure)
                {
                    state = NodeState.Failure;
                    return state;
                }
                else if (childState == NodeState.Running)
                {
                    running = true;
                    break;
                }
            }

            state = running ? NodeState.Running : NodeState.Success;
            return state;
        }
        
        public override void SetBehaviorTree(BehaviorTree bt)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].SetBehaviorTree(bt);
            }
        }
    }
}