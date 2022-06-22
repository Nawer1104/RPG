using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Create New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField]
        private GameObject weaponPreb = null;
        [SerializeField]
        AnimatorOverrideController animatorOverride = null;
        [SerializeField]
        private float weaponRange = 2f;
        [SerializeField]
        private float weaponDamge = 5f;
        [SerializeField]
        private float timeBetweenAttacks = 1f;

        public void Spawn(Transform handTransform, Animator anim)
        {
            if (weaponPreb != null)
            {
                Instantiate(weaponPreb, handTransform);
            }     
            if (animatorOverride != null)
            {
                anim.runtimeAnimatorController = animatorOverride;
            }      
        }

        public float GetDamge() { return weaponDamge; }
        

        public float GetRange() { return weaponRange; }

        public float GetTimeBetweenAttacks() { return timeBetweenAttacks; }
    }
}

