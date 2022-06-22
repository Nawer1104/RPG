using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private PlayerMovement playerMovement;
        private ActionScheduler actionScheduler;
        private Animator animator;
         
        [SerializeField]
        private Weapon defaultWeapon = null;
        [SerializeField]
        private Transform handTransform = null;


        private Weapon currentWeapon = null;
        private Character target;
        private float timeSinceLastAttack = Mathf.Infinity;
 
        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            
            EquipWeapon(defaultWeapon);
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            weapon.Spawn(handTransform, animator);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead()) return;

            if (!IsInRangeAttack())
            {
                playerMovement.MoveTo(target.transform.position, 1f);
            } else
            {
                playerMovement.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > currentWeapon.GetTimeBetweenAttacks())
            {
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Character enemyToTest = combatTarget.GetComponent<Character>();
            return enemyToTest != null && !enemyToTest.IsDead();
        }
        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Character>();      
        }

        private bool IsInRangeAttack()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

        // Animation Event
        void Hit ()
        {
            if (target == null) return;
            target.TakeDamge(currentWeapon.GetDamge());
        }
    }
}
