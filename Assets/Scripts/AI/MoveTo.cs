using UnityEngine;
using UnityEngine.AI;
namespace MonsterFarm
{
    public class MoveTo : MonoBehaviour
    {
        public Transform goal;
        private void Start()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = goal.position; 
        }
    }
}