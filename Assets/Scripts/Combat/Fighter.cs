using UnityEngine;
using RPG.Movement;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        private PlayerMovement playerMovement;

        [SerializeField]
        float weaponRange = 2f;
        Transform target;


        bool isInRangeAttack;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            isInRangeAttack = IsInRangeAttack(transform.position, target.position, weaponRange);
            if (target != null && !isInRangeAttack)
            {
                playerMovement.MoveTo(target.position);
            } else
            {
                playerMovement.Stop();
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;      
        }

        private bool IsInRangeAttack(Vector3 playerPos, Vector3 targetPos, float weaponRange)
        {
            if (Vector3.Distance(playerPos, targetPos) < weaponRange) return true;
            return false;
        }
    }
}
