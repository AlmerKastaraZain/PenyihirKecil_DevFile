using UnityEngine;

namespace StaminaBar_Controller
{
    public class StaminaBarController : MonoBehaviour
    {
        private void Start()
        {
            //Dialogue
            DialogueManager.Instance.OnShowDialogue += () =>
            {
                staminaBarToggle(true);
            };
            DialogueManager.Instance.OnHideDialogue += () =>
            {
                staminaBarToggle(false);
            };
        }
        [SerializeField] public StaminaBar staminaBar;
        private void staminaBarToggle(bool value)
        {
            staminaBar.gameObject.SetActive(value);
        }
    }
}
