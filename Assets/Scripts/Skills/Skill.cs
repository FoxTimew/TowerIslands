using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public int id;
    public TMP_Text titletext;
    public TMP_Text descriptionText;

    public int[] connectedSkills;

    public void UpdateUI()
    {
        titletext.text = $"{SkillTree.instance.skillLevels[id]}/{SkillTree.instance.skillCaps[id]}/{SkillTree.instance.skillNames[id]}";
        descriptionText.text = $"{SkillTree.instance.skillDescriptions[id]}\nCost : {SkillTree.instance.skillPoints}/1 SP";

        GetComponent<Image>().color = SkillTree.instance.skillLevels[id] >= SkillTree.instance.skillCaps[id]
            ?
            Color.yellow
            : SkillTree.instance.skillPoints > 1
                ? Color.green
                : Color.white;
    }

    public void Buy()
    {
        if (SkillTree.instance.skillPoints < 1 ||
            SkillTree.instance.skillLevels[id] >= SkillTree.instance.skillCaps[id])
        {
            return;
        }

        SkillTree.instance.skillPoints -= 1;
        SkillTree.instance.skillLevels[id]++;
        SkillTree.instance.UpdateAllSkillUI();
    }
}
