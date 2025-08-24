using System;
using UnityEngine;

namespace Code.Entities.FSM
{
    public enum EntityStateType
    {
        None = -1,
        
        Idle,
        Walk,
        Run,
        Chase,
        Patrol,
        Crouch,
        Jump,
        Fall,
        Attack,
        Fire,
        Hit,
        
        Max
    }
    
    [CreateAssetMenu(fileName = "StateSo", menuName = "SO/StateSo", order = 0)]
    public class StateSO : ScriptableObject
    {
        public EntityStateType stateType;
        public string className;
        public string animationName;

        public int animationHash;
        
        [SerializeField] private int _stateIndex;

        private void OnValidate()
        {
            if(animationName != null)
                animationHash = Animator.StringToHash(animationName);
        }
    }
}