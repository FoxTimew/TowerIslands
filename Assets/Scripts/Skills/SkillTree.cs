using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{ 
    public static SkillTree instance;
    public int[] skillLevels;
    public int[] skillCaps;
    public string[] skillNames;
    public string[] skillDescriptions;

    public List<Skill> skillList;
    public GameObject skillHolder;

    public int skillPoints;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        skillPoints = 20;
        foreach (var skill in skillHolder.GetComponentsInChildren<Skill>())
        {
            skillList.Add((skill));
        }

        for (int i = 0; i < skillList.Count; i++)
        {
            skillList[i].id = i;
        }
        UpdateAllSkillUI();
    }

    public void UpdateAllSkillUI()
    {
        foreach (Skill skill in skillList)
        {
            skill.UpdateUI();
        }
    }
}
