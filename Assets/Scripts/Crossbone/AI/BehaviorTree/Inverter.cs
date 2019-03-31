namespace Crossbone.AI.BehaviorTree
{
    public class Inverter : Node
    {
        protected Node _node;

        public Inverter(Node node)
        {
            _node = node;
        }

        public Node node
        {
            get { return _node; }
        }

        public override NodeState Evaluate()
        {
            NodeState childState = _node.Evaluate();
            if (childState == NodeState.Running)
            {
                state = NodeState.Running;
            }
            else if (childState == NodeState.Failure)
            {
                state = NodeState.Success;
            }
            else if (childState == NodeState.Running)
            {
                state = NodeState.Failure;
            }

            return state;
        }
        
        public override void SetBehaviorTree(BehaviorTree bt)
        {
            base.SetBehaviorTree(bt);
            _node.SetBehaviorTree(bt);
        }
    }
}