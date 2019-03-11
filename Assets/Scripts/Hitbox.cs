using UnityEngine;

namespace MonsterFarm
{
    public class Hitbox : MonoBehaviour
    {
        private MobPawn _mob;
        public AudioClip hitClip;

        private void Awake()
        {
            _mob = GetComponentInParent<MobPawn>();
        }

        public void GetHit (Vector3 pos, int damage)
        {
            var fxBloodhitPrefab = Resources.Load("Prefab/fx_bloodhit");
            GameObject.Instantiate(fxBloodhitPrefab, pos, Quaternion.identity);
            var am = AudioManager.Instance;
            am.PlaySFX(hitClip);
            if (_mob)
            {
                _mob.Hurt(damage);    
            }
        }
    }
}