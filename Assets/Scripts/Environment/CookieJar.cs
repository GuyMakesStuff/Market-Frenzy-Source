using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarketFrenzy.Environment
{
    public class CookieJar : MonoBehaviour
    {
        public Color[] Colors;
        public Material Mat;
        public MeshRenderer Jar;

        // Start is called before the first frame update
        void Start()
        {
            Color Col = Colors[Random.Range(0, Colors.Length)];
            Material NewMat = new Material(Mat);
            NewMat.color = Col;
            Jar.sharedMaterial = NewMat;
        }
    }
}