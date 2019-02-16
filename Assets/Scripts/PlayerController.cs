using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerController : MonoBehaviour
    {
        private static PlayerController instance_;
        public static PlayerController Instance {
            get {
                if (!instance_) {
                    instance_ = (PlayerController) FindObjectOfType(typeof(PlayerController));
                }
                return instance_;
            }
        }

        public PlayerPawn playerPawn;
    }
}