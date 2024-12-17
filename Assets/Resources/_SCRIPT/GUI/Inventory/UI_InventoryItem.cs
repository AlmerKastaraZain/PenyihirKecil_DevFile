using System;
using System.Collections;
using System.Collections.Generic;
using Name;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory_UI
{
    public class UI_InventoryItem : MonoBehaviour, IPointerClickHandler,
        IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private Image TxtBackground;
        [SerializeField]
        private TMP_Text quantityText;

        [SerializeField]
        private Image backgroundObject;

        public event Action<UI_InventoryItem> OnItemClicked,
         OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;

        private bool isEmpty = true;

        public void Awake()
        {
            ResetData();
            Deselect();
        }

        public void ResetData()
        {
            backgroundObject.gameObject.SetActive(false);
            itemImage.gameObject.SetActive(false);
            isEmpty = true;
        }

        public void Deselect()
        {
            backgroundObject.gameObject.SetActive(false);
        }

        public void SetData(Sprite sprite, int quantity)
        {
            this.itemImage.gameObject.SetActive(true);
            this.TxtBackground.gameObject.SetActive(true);
            this.itemImage.sprite = sprite;
            this.quantityText.text = quantity + "";
            isEmpty = false;
        }

        public void Select()
        {
            backgroundObject.gameObject.SetActive(true);
        }


        public void OnPointerClick(PointerEventData pointerData)
        {
            if (pointerData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClick?.Invoke(this);
            }
            else
            {
                OnItemClicked?.Invoke(this);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isEmpty)
                return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {

        }
    }
}