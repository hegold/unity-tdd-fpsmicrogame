using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        public GameObject Owner { get; private set; }
        public Vector3 InitialPosition { get; private set; }
        public Vector3 InitialDirection { get; private set; } // Note: this is never used
        public Vector3 InheritedMuzzleVelocity { get; private set; }
        public float InitialCharge { get; private set; }

        public UnityAction OnShoot;

        public void Shoot(WeaponController controller)
        {
            Shoot(controller.Owner, controller.MuzzleWorldVelocity, controller.CurrentCharge);
        }

        public void Shoot(GameObject owner, Vector3 muzzleWorldVelocity, float currentCharge)
        {
            Owner = owner;
            InitialPosition = transform.position;
            InitialDirection = transform.forward;
            InheritedMuzzleVelocity = muzzleWorldVelocity;
            InitialCharge = currentCharge;

            OnShoot?.Invoke();
        }
    }
}