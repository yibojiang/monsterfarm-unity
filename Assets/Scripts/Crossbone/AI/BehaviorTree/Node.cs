namespace Crossbone.AI.BehaviorTree
{
    public enum NodeState
    {
        Running,
        Success,
        Failure
    }
    public abstract class Node
    {
        private BehaviorTree _bt;

        public delegate NodeState NodeCallback();  
        
        public Node() {}

        protected NodeState state;

        public NodeState nodeState
        {
            get { return state; }
        }

        public abstract NodeState Evaluate();

        public virtual void SetBehaviorTree(BehaviorTree bt)
        {
            _bt = bt;
        }
        
    }
}