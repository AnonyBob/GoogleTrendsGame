using System;
using UnityEngine;

namespace GoogleTrends
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                InternalAwake();
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            InternalOnDestroy();
        }

        protected virtual void InternalAwake()
        {
            
        }

        protected virtual void InternalOnDestroy()
        {
            
        }
    }
}