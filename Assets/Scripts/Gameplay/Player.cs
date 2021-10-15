using MarketFrenzy.Managers;
using MarketFrenzy.Audio;
using UnityEngine;

namespace MarketFrenzy.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        public float Speed;
        float X;
        Rigidbody Body;

        [Header("Shooting")]
        public GameObject BulletPrefab;
        public Transform FirePoint;
        public float BulletSpeed;

        [Header("Other")]
        public GameObject KillFX;

        // Start is called before the first frame update
        void Start()
        {
            Body = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            X = Input.GetAxisRaw("Horizontal") * Speed;

            Body.constraints = (GameManager.IsPaused) ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.FreezeRotation;

            if (Input.GetKeyDown(KeyCode.Space) && !GameManager.IsPaused)
            {
                Shoot();
            }
        }
        void FixedUpdate()
        {
            Body.velocity = new Vector3(X, Body.velocity.y, 0f);
        }

        void Shoot()
        {
            AudioPlayer.Instance.InteractWithSound("Shoot", SoundBehaviourType.Play);
            GameObject NewBullet = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
            NewBullet.GetComponent<Bullet>().Speed = BulletSpeed;
        }

        public void ShowKillFX()
        {
            GameObject InstantiatedKillFX = Instantiate(KillFX, transform.position, Quaternion.identity);
            Destroy(InstantiatedKillFX, 5f);
            Destroy(gameObject);
        }
    }
}