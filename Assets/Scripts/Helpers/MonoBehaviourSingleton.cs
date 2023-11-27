using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Helpers
{
    /// <summary>
    /// makes a MonoBehaviour class to a singleton class (to use it, inherit your class from it) 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<T>();
                if (_instance != null) return _instance;
                //Debug.Log("Creating new object Singleton : "+ typeof(T));
                var newGo = new GameObject(typeof(T).Name);
                _instance = newGo.AddComponent<T>();
                return _instance;
            }
        }

        [CanBeNull]
        public static T InstanceNoCreate
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<T>();
                return _instance;
            }
        }

        protected void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}