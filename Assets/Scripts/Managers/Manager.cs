using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarketFrenzy.Managers
{
    public class Manager<T> : MonoBehaviour
    {
        public bool IsGlobal;
        public static T Instance { get; private set; }
        [Space]
        [HideInInspector]
        public bool IsInstanced;

        protected void Init(T OBJ)
        {
            if (IsGlobal)
            {
                DontDestroyOnLoad(gameObject);
            }
            Instance = OBJ;
        }

        void LateUpdate()
        {
            IsInstanced = (Instance != null);
        }
    }
}