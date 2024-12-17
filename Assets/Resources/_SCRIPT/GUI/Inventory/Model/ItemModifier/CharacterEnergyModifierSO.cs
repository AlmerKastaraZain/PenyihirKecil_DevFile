using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Modifier", menuName = "ScriptableObjects/Modifier/CharacterEnergyModifierSO", order = 1)]
public class CharacterEnergyModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        character.GetComponent<PlayerController>().AddEnergy(val);
    }
}
