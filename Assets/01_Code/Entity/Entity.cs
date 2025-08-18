using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Code.Entities.FSM;

namespace Code.Entities
{
    [DefaultExecutionOrder(-100)]
    public class Entity : MonoBehaviour
    {
        public UnityEvent OnDeath;
        public UnityEvent OnTakeDamage;

        protected Dictionary<Type, IEntityComponent> m_components = new Dictionary<Type, IEntityComponent>();
        
        protected EntityAnimation m_entityAnimation;
        protected EntityStateMachine m_entityStateMachine;
        protected EntityHealth m_entityHealth;

        protected virtual void Awake()
        {
            AddComponent();
            InitializeComponent();
            AfterInitializeComponent();
            
            m_entityAnimation = GetCompo<EntityAnimation>();
            m_entityStateMachine = GetCompo<EntityStateMachine>();
            m_entityHealth = GetCompo<EntityHealth>();
        }

        private void AddComponent()
        {
            //비활성화된 컴포넌트도 가져오기
            GetComponentsInChildren<IEntityComponent>(true).ToList()
                .ForEach(component => m_components.Add(component.GetType(), component));
        }

        private void InitializeComponent()
        {
            m_components.Values.ToList().ForEach(component => component.Initialize(this));
        }
        
        private void AfterInitializeComponent()
        {
            m_components.Values.OfType<IAfterInitialize>().ToList().ForEach(component => component.AfterInitialize());
        }
        
        public T GetCompo<T>() where T : IEntityComponent
            => (T)m_components.GetValueOrDefault(typeof(T));
        
        public void EnitiyDestroy()
        {
            Destroy(gameObject);
        }
    }
}