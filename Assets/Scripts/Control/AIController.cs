using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        float chaseDistance = 5f;

        private GameObject player;
        private Fighter fighter;
        private Character character;
        private PlayerMovement mover;
        private Vector3 guardPos;


        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            fighter = GetComponent<Fighter>();
            character = GetComponent<Character>();
            mover = GetComponent<PlayerMovement>();
            guardPos = transform.position;
        }

        private void Update()
        {
            if (character.IsDead()) return;

            if (DistanceToPlayer() && fighter.CanAttack(player))
            {
                fighter.Attack(player);
            } else
            {
                mover.StartMoveAction(guardPos);
            }
        }

        private bool DistanceToPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
