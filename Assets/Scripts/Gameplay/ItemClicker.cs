using MarketFrenzy.Gameplay.ConveyorItems;
using MarketFrenzy.Managers;
using UnityEngine;

namespace MarketFrenzy.Gameplay
{
    [RequireComponent(typeof(Camera))]
    public class ItemClicker : MonoBehaviour
    {
        Camera Cam;

        // Start is called before the first frame update
        void Start()
        {
            Cam = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !GameManager.IsPaused)
            {
                TryClick();
            }
        }
        void TryClick()
        {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                ConveyorItem item = hit.collider.GetComponent<ConveyorItem>();
                if (item != null)
                {
                    item.OnClick();
                }
            }
        }
    }
}