using RPG.Movement;
using RPG.Combat;
using System;
using UnityEngine;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovement playerMovement;
        private Fighter fighter;
        private Character character;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            fighter = GetComponent<Fighter>();
            character = GetComponent<Character>();  
        }

        private void Update()
        {
            if (character.IsDead()) return;

            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
           
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    fighter.Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    playerMovement.StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

