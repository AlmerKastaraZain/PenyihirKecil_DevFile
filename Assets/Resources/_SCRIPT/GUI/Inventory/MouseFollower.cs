using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory_UI
{
    public class MouseFollower : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private UI_InventoryItem item;

        public void Awake()
        {
            canvas = transform.root.GetComponent<Canvas>();
            item = GetComponentInChildren<UI_InventoryItem>();
        }

        public void SetData(Sprite sprite, int quantity)
        {
            item.SetData(sprite, quantity);
        }

        void Update()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform,
                Input.mousePosition,
                canvas.worldCamera,
                out position
            );
            transform.position = canvas.transform.TransformPoint(position);
        }

        public void Toggle(bool val)
        {
            gameObject.SetActive(val);
        }
    }
}