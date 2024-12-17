using System.Collections;
using NPC_Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneInitialization : MonoBehaviour
{
    [SerializeField] private QuestNPCSO Dialogue1;
    [SerializeField] private QuestNPCSO Dialogue2;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private PlayableAsset lookAround;
    [SerializeField] private PlayableAsset blackScreen;
    private bool dialogueIsRunning = false;
    private int order = 0;

    public void Update()
    {
        if (director.state != PlayState.Playing)
        {
            if (order == 0)
            {
                StartDialogue(Dialogue1);
            }
            else if (order == 1)
            {
                StartDialogue(Dialogue2);
            }
        }
    }

    private void Start()
    {
        GameController.instance.disableMovement = true;
    }
    private void Awake()
    {
        Dialogue1.audioSource = gameObject.GetComponent<AudioSource>();
        Dialogue2.audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void StartDialogue(QuestNPCSO npc)
    {
        if (dialogueIsRunning == true)
            return;

        dialogueIsRunning = true;

        DialogueManager.Instance.ChangeName(npc.name);
        DialogueManager.Instance.ChangePotrait(npc.NPCPotrait);
        GameController.state = GameState.Dialogue;
        StartCoroutine(DialogueManager.Instance.ShowDialogue(npc.dialogue, npc.audioSource));
        StartCoroutine(CheckIfDialogueOver());
    }


    private IEnumerator CheckIfDialogueOver()
    {
        while (GameController.state == GameState.Dialogue)
            yield return null;

        DialogueIsOver();
        StopCoroutine(CheckIfDialogueOver());
    }

    private void DialogueIsOver()
    {
        if (order == 0)
        {
            LoadAsset(lookAround);
            dialogueIsRunning = false;
            order++;
        }
        else if (order == 1)
        {
            LoadAsset(blackScreen);
            ToSampleScene();
            dialogueIsRunning = false;
            order++;
        }
    }
    private void LoadAsset(PlayableAsset asset)
    {
        director.Play(asset);
    }

    private void ToSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}