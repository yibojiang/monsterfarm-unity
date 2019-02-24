using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
namespace MonsterFarm
{
    public class MoveTo : MonoBehaviour
    {
        public Transform goal;
        private Seeker _seeker;

        public Transform dest;
//        private AIDestinationSetter _destinationSetter;
        private void Start()
        {
//            _destinationSetter = GetComponent<AIDestinationSetter>();
            _seeker = GetComponent<Seeker>();
            var path = _seeker.StartPath(transform.position, dest.position);
            foreach (var n in path.path)
            {
                Debug.Log(n.position);
            }
//            foreach (var p in path)
//            {
//                Debug.Log();
//            }
//            _destinationSetter.target = PlayerController.Instance.playerPawn.transform;
            //_seeker.GetCurrentPath();
//            transform.position += 

        }
    }
}