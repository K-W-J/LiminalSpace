using System;
using UnityEngine;

namespace Code.Entities
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimation : MonoBehaviour, IEntityComponent
    {
        public Action OnAnimationEnd;
        public Action OnAttack;
        public Action OnFireAttack;
        public Action<bool> IsAttackAnimation;
        
        [SerializeField] private Animator animator;
        
        [SerializeField] private bool onAwakeLookTarget;
        
        public Transform LookTarget { get; set; }
        
        public void Initialize(Entity entity)
        {
            
        }
        
        public void SetBool(int hash, bool value) => animator.SetBool(hash, value);
        
        public void SetAnimationActive(bool value) => animator.enabled = value;
        
        public void StartAttackAnimation() => IsAttackAnimation?.Invoke(true);
        public void EndAttackAnimation() => IsAttackAnimation?.Invoke(false);
        public void OnAnimation() => OnAnimationEnd?.Invoke();
        public void AttackAnimation() => OnAttack?.Invoke();
        public void FireAttackAnimation() => OnFireAttack?.Invoke();

        private void Awake()
        {
            if(animator ==null)
                animator = GetComponent<Animator>();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if(LookTarget == null) return;
            
            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(LookTarget.position);
        }
    }
}