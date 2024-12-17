using System;
using Shop_Action;
using Shop_UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class IInputField : MonoBehaviour
    {
        public IInventory UIInventory;
        public TMP_InputField inputField;
        public Button button;
        public GameObject input;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
            inputField.onEndEdit.AddListener(OnEndEdit);
        }

        virtual public void OnEndEdit(string text)
        {

        }

        virtual public void OnClick()
        {

        }


        public void Show()
        {
            inputField.SetTextWithoutNotify("");
            input.gameObject.SetActive(true);
        }


        public void Hide()
        {
            inputField.SetTextWithoutNotify("");
            input.gameObject.SetActive(false);
        }

    }
}
