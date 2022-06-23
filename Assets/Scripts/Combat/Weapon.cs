using RPG.Core;
using System;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Create New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField]
        private GameObject weaponPreb = null;
        [SerializeField]
        private AnimatorOverrideController animatorOverride = null;
        [SerializeField]
        private float weaponRange = 2f;
        [SerializeField]
        private float weaponDamge = 5f;
        [SerializeField]
        private float timeBetweenAttacks = 1f;
        [SerializeField]
        private bool isRightHanded = true;
        [SerializeField]
        private Projectile projectile = null;

        const string weaponName = "Weapon(Clone)"; 

        public void Spawn(Transform rightHand, Transform leftHand, Animator anim)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (weaponPreb != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                Instantiate(weaponPreb, handTransform);
            }

            var overrideController = anim.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                anim.runtimeAnimatorController = animatorOverride;
            } else if (overrideController != null)
            {
                anim.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            Debug.Log(oldWeapon);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }

            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded)
            {
                handTransform = rightHand;
                
            }
            else
            {
                handTransform = leftHand;
            }

            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }


        public void LaunchProjectile(Transform rightHand, Transform leftHand, Character target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamge);
        }

        public float GetDamge() { return weaponDamge; }
        

        public float GetRange() { return weaponRange; }

        public float GetTimeBetweenAttacks() { return timeBetweenAttacks; }
    }
}

