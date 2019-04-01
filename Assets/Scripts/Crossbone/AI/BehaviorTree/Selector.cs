using System.Collections.Generic;

namespace Crossbone.AI.BehaviorTree
{
    public class Selector : Node
    {
        protected List<Node> nodes;

        public Selector(List<Node> nodes)
        {
            this.nodes = nodes;
        }
        
        public override NodeState Evaluate()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                NodeState childState = nodes[i].Evaluate();
                if (childState  == NodeState.Success)
                {
                    state = NodeState.Success;
                    return NodeState.Success;
                }
                else if (childState  == NodeState.Running)
                {
                    state = NodeState.Running;
                    return NodeState.Running;
                }
            }

            state = NodeState.Failure;
            return NodeState.Failure;
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