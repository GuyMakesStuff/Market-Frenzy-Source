using System.Collections;
using MarketFrenzy.Audio;
using UnityEngine;
using MarketFrenzy.Managers;

namespace MarketFrenzy.Gameplay.ConveyorItems
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class ConveyorItem : MonoBehaviour
    {
        Rigidbody Body;
        public float Speed;
        public static readonly int Value = 5;

        // Start is called before the first frame update
        void Start()
        {
            Body = GetComponent<Rigidbody>();
            Body.useGravity = false;
            Body.constraints = RigidbodyConstraints.FreezeRotation;
        }
        void FixedUpdate()
        {
            Body.velocity = (GameManager.IsPaused) ? Vector3.zero : (Vector3.left * Speed);
        }
        void OnCollisionEnter(Collision Info)
        {
            if (Info.collider.name == "LeftOvenEntrance")
            {
                OnEnterLeftOven();
            }
        }

        public virtual void OnClick()
        {
            Debug.Log(gameObject.name + " Has Been Clicked!");
            Destroy(gameObject);
        }
        public virtual void OnEnterLeftOven()
        {
            Debug.Log(gameObject.name + " Has Entered The Left Oven!");
            Destroy(gameObject);
        }

        protected void EncreaseScore()
        {
            AudioPlayer.Instance.InteractWithSound("Good Conveyor Remove", SoundBehaviourType.Play);
            GameManager.Instance.AddScore(Value);
        }
        protected void DecreaseScore()
        {
            AudioPlayer.Instance.InteractWithSound("Bad Hit", SoundBehaviourType.Play);
            GameManager.Instance.DecreaseScore(Value);
        }
    }
}