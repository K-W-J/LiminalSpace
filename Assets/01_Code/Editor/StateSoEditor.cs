using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Code.Entities.FSM;
using UnityEditor;

namespace Code.Editor
{
    [CustomEditor(typeof(StateSO))]
    public class StateSoEditor : UnityEditor.Editor
    {
        private SerializedProperty _stateIndex;
        private SerializedProperty _animationName;
        private SerializedProperty _stateType;

        
        private void OnEnable()
        {
            _stateIndex = serializedObject.FindProperty("_stateIndex");
            _animationName = serializedObject.FindProperty("animationName");
            _stateType = serializedObject.FindProperty("stateType");
        }

        public override void OnInspectorGUI()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(EntityState));
            
            List<string> StateType = assembly.GetTypes().Where(type => type.IsAbstract == false && 
                    type.IsSubclassOf(typeof(EntityState))).Select(name => name.FullName).ToList();

            
            StateSO state = (StateSO)target;
            
            _stateIndex.intValue = EditorGUILayout.Popup("State Type", _stateIndex.intValue, StateType.ToArray());
            //_stateIndex.intValue = StateType.IndexOf(state.className);

            EditorGUILayout.PropertyField(_stateType); 
            state.className = EditorGUILayout.TextField("Class Name",StateType[_stateIndex.intValue]);
            EditorGUILayout.PropertyField(_animationName);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
