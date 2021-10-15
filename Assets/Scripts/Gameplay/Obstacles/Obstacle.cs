using System;
using MarketFrenzy.Audio;
using UnityEngine;
using MarketFrenzy.Managers;

namespace MarketFrenzy.Gameplay.Obstacles
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Obstacle : MonoBehaviour
    {
        Rigidbody Body;
        public float FallSpeed;
        [Min(0)]
        public int Value;
        public enum CollisionState { NULL, Floor, Player, Bullet }
        CollisionState ColState;
        [HideInInspector]
        protected string Message = "Message";

        void Start()
        {
            Body = GetComponent<Rigidbody>();
            Body.useGravity = false;
            Body.constraints = RigidbodyConstraints.FreezeRotation;
        }

        void FixedUpdate()
        {
            if (GameManager.IsPaused)
            {
                Body.velocity = Vector3.zero;
                return;
            }

            Body.velocity = Vector3.down * FallSpeed;
        }

        void OnCollisionEnter(Collision Info)
        {
            switch (Info.collider.tag)
            {
                case "Player":
                    {
                        ColState = CollisionState.Player;
                        break;
                    }
                case "Bullets":
                    {
                        ColState = CollisionState.Bullet;
                        break;
                    }
                default:
                    {
                        ColState = CollisionState.Floor;
                        break;
                    }
            }

            OnCollide();
        }
        protected virtual void OnCollide()
        {
            Debug.Log(ColState);
        }
        protected void HandleCollision(Action PlayerAction, Action BulletAction, Action FloorAction, string message)
        {
            Message = message;

            switch (ColState)
            {
                case CollisionState.Player:
                    {
                        PlayerAction();
                        break;
                    }
                case CollisionState.Bullet:
                    {
                        BulletAction();
                        break;
                    }
                case CollisionState.Floor:
                    {
                        FloorAction();
                        break;
                    }
            }

            Destroy(gameObject);
        }

        public void IncreaseScore()
        {
            AudioPlayer.Instance.InteractWithSound("Hit", SoundBehaviourType.Play);
            GameManager.Instance.AddScore(Value);
        }
        public void DecreaseScore()
        {
            AudioPlayer.Instance.InteractWithSound("Bad Hit", SoundBehaviourType.Play);
            GameManager.Instance.DecreaseScore(Value);
        }
        public void DecreaseLives()
        {
            GameManager.Instance.Damage(Message);
        }
    }
}