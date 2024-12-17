using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface NPCInterface
{
    public Dialogue dialogue { get; set; }

    public string NPCName { get; set; }

    public AudioSource audioSource { get; set; }

    public Sprite NPCPotrait { get; set; }


}
