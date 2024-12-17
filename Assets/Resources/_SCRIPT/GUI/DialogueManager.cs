using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] int letterPerSecond;
    [SerializeField] public Image potraitImage_1;
    [SerializeField] public Image potraitImage_2;
    [SerializeField] public TextMeshProUGUI CharName1;
    [SerializeField] public TextMeshProUGUI CharName2;
    public event Action OnShowDialogue;
    public event Action OnHideDialogue;
    private List<AudioClip> audioClipsList;
    [SerializeField] private AudioSource audioSources;
    public static DialogueManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        Instance = this;
    }

    Dialogue dialogue;
    int currentLine = 0;
    bool isTyping;
    public void HandleUpdate()
    {
        if (dialogue.Turns[currentLine] == 0)
        {
            PlayerTurn();
        }
        else if (dialogue.Turns[currentLine] == 1)
        {
            NPCTurn();
        }


        GracePeriod();
        if (Input.GetKeyUp(KeyCode.E) && !isTyping)
        {
            ++currentLine;

            if (currentLine < dialogue.Lines.Count)
            {
                PlayDialogueAudio();
                timeRemaining = (float)0.5;

                StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
            }
            else
            {
                StopDialogueAudio();
                currentLine = 0;
                OnHideDialogue?.Invoke();
                dialogueBox.SetActive(false);
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && isTyping && (timeRemaining <= 0))
        {
            timeRemaining = 0.5f;

            isTyping = false;
            dialogueText.text = "";
            dialogueText.text = dialogue.Lines[currentLine];
        }
    }

    private void PlayDialogueAudio()
    {
        audioSources.clip = audioClipsList[currentLine];
        audioSources.Play();
    }

    private void StopDialogueAudio()
    {
        audioSources.Stop();
    }

    private void PlayerTurn()
    {
        potraitImage_1.color = Color.white;
        potraitImage_2.color = Color.gray;


        CharName1.color = Color.white;
        CharName2.color = Color.gray;
    }

    private void NPCTurn()
    {
        potraitImage_1.color = Color.gray;
        potraitImage_2.color = Color.white;

        CharName1.color = Color.gray;
        CharName2.color = Color.white;
    }

    public void ChangePotrait(Sprite sprite)
    {
        potraitImage_2.sprite = sprite;
    }

    public void ChangeName(string NPCname)
    {
        CharName2.text = NPCname;
    }

    public IEnumerator ShowDialogue(Dialogue dialogue, AudioSource audioSourcee)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialogue?.Invoke();
        audioClipsList = dialogue.Clips;
        audioSources = audioSourcee;
        PlayDialogueAudio();
        this.dialogue = dialogue;
        dialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[0]));
    }

    public float timeRemaining = (float)0.5;
    private void GracePeriod()
    {
        if (timeRemaining > 0 && isTyping)
        {
            timeRemaining -= Time.deltaTime;
        }
    }

    public IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            if (isTyping == false) yield break;
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
        isTyping = false;
    }
}
