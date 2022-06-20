using UnityEngine;

namespace RPG.Core
{
    public class Character : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        private Animator animator;
        private ActionScheduler action;
        private bool isDead = false;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            action = GetComponent<ActionScheduler>();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamge(float damge)
        {
            health = Mathf.Max(health - damge, 0);
            if (health <= 0)
            {
                Die();
            }
            print(health);
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            animator.SetTrigger("die");
            action.CancelCurrentAction();
        }
    }
}

