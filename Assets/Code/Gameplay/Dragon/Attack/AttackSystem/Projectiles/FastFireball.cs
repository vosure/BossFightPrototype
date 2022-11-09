using Code.Gameplay.Player;
using DG.Tweening;
using UnityEngine;

namespace Code.Gameplay.Dragon.Attack.AttackSystem.Projectiles
{
    public class FastFireball : MonoBehaviour
    {
        [SerializeField] private float timeToFly;
        [SerializeField] private int damage = 10;

        private Tween _currentTween;
        
        public void SetTarget(Transform target) => 
            _currentTween = transform.DOMove(target.position, timeToFly);

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(damage);
                _currentTween.Kill();
                Destroy(gameObject);
            }
        }
    }
}
