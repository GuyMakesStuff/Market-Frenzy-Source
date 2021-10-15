using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarketFrenzy.Environment
{
    public class Can : MonoBehaviour
    {
        void Start()
        {
            float Rot = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0, Rot, 0);
        }
    }
}