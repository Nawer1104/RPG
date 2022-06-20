using RPG.Core;
using UnityEngine;
using UnityEngine.AI;


namespace RPG.Movement
{
    public class PlayerMovement : MonoBehaviour, IAction
    {
        private NavMeshAgent agent;
        private Animator anim;
        private ActionScheduler actionScheduler;
        private Character character;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();          
            actionScheduler = GetComponent<ActionScheduler>();
            character = GetComponent<Character>();
        }

        // Update is called once per frame
        void Update()
        {
            agent.enabled = !character.IsDead();

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            actionScheduler.StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            agent.destination = destination;
            agent.isStopped = false;
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            anim.SetFloat("forwardSpeed", speed);
        }


    }
}

