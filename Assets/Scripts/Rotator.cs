using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarketFrenzy
{
    public class Rotator : MonoBehaviour
    {
        public Vector3 RotDir;
        public float Speed;

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(RotDir, Speed * Time.deltaTime);
        }
    }
}