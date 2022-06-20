using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        float chaseDistance = 5f;
        [SerializeField]
        float suspicionTime = 3f;
        [SerializeField]
        PatrolPath patrolPath;
        [SerializeField]
        float waypointTolerance = 1f;
        [SerializeField]
        float waypointDwellTime = 3f;
        [SerializeField]
        [Range(0f, 1f)]
        float patrolSpeedFraction = 0.2f;

        private GameObject player;
        private ActionScheduler scheduler;
        private Fighter fighter;
        private Character character;
        private PlayerMovement mover;
        private Vector3 guardPos;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedLastWaypoint = Mathf.Infinity;
        private int currentWaypointIndex = 0;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            fighter = GetComponent<Fighter>();
            character = GetComponent<Character>();
            mover = GetComponent<PlayerMovement>();
            scheduler = GetComponent<ActionScheduler>();
            guardPos = transform.position;
        }

        private void Update()
        {
            if (character.IsDead()) return;

            if (DistanceToPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                scheduler.CancelCurrentAction();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimer();
        }

        private void UpdateTimer()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedLastWaypoint += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPos = guardPos;

            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrivedLastWaypoint = 0;
                    CycleWaypoint();
                }
                nextPos = GetCurrentWaypoint();
            }

            if (timeSinceArrivedLastWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPos, patrolSpeedFraction);
            }

            
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWayPoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWayPoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWayPoint < waypointTolerance;
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
