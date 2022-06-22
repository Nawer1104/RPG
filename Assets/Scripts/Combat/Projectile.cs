using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1;
         
        private Character target = null;
        private CapsuleCollider targetCapsule;
        private float damge = 0;

        private void Start()
        {
            targetCapsule = target.GetComponent<CapsuleCollider>();
            transform.LookAt(GetAimLocation());
        }

        public void SetTarget(Character target, float damge)
        {
            this.target = target;
            this.damge = damge;
        }

        private void Update()
        {
            if (target == null) return;       
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            if (targetCapsule == null)
            {
                return target.transform.position;
            } else
            {
                return target.transform.position + Vector3.up * targetCapsule.height / 2;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Character>() != target) return;
            if (target.IsDead()) return;
            target.TakeDamge(damge);
            Destroy(gameObject);
        }
    }
}

