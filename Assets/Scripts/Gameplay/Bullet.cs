using MarketFrenzy.Audio;
using MarketFrenzy.Managers;
using UnityEngine;

namespace MarketFrenzy.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        Rigidbody Body;
        [HideInInspector]
        public float Speed;

        void Start()
        {
            Body = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            Body.velocity = (GameManager.IsPaused) ? Vector3.zero : transform.forward * Speed;
        }

        void OnCollisionEnter(Collision Info)
        {
            if (Info.collider.name == "Ceilling")
            {
                AudioPlayer.Instance.InteractWithSound("Bad Hit", SoundBehaviourType.Play);
                GameManager.Instance.DecreaseScore(1);
            }
            Destroy(gameObject);
        }
    }
}