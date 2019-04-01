namespace Crossbone.AI.BehaviorTree
{
    public class ActionNode : Node
    {
        private NodeCallback _actionCallback;
        public ActionNode(NodeCallback actionCallback)
        {
            _actionCallback = actionCallback;
        }

        public override NodeState Evaluate()
        {
            state = _actionCallback();
            return state;
        }
    }
}