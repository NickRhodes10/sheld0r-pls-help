using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Level System tracks players current xp and level, adds experience
/// when called and calculates xp to next level. When an enemy dies,
/// 
/// LevelSystem.instance.AddExperience(experience);
/// Destroy(gameObject);
/// 
/// Will add xp to the player.
/// </summary>
public class LevelSystem : MonoBehaviour
{
    public static LevelSystem instance;                               // Can be referenced from other scripts

    public int level = 0;                                             // Player current level
    public int experience;                                            // Player current EXP
    public int experienceToNextLevel;                                 // EXP to next level
    public int perLevelSkillpoint = 2;
    public int skillPoints;

    public AudioClip expSFX;

    private AudioManager _am;

    private void Awake()
    {
        if (instance != null)                                       // Check for more than one player with LevelSystem script
        {
            Debug.Log("More than one LevelSystem in scene.");
            return;
        }

        instance = this;                                            // instance variable = this object.
        SetLevel(1);                                                // Set Level at 1 upon startup.
    }

    private void Start()
    {
        _am = AudioManager.instance;
    }

    /// <summary>
    /// AddExperience() will add the value of the parameter passed
    /// to the players current xp amount. Checks for level up and 
    /// calls the SetLevel function if true.
    /// </summary>
    /// <param name="experienceToAdd"></param>
    /// <returns></returns>
    public bool AddExperience(int experienceToAdd)
    {
        experience += experienceToAdd;
        _am.PlaySFX(expSFX);

        if (experience >= experienceToNextLevel)
        {
            skillPoints += perLevelSkillpoint;
            SetLevel(level + 1);
            GameManager.PlayerManager.pm.skillPoints++;

            return true;
        }

        return false;
    }


    /// <summary>
    /// SetLevel calculates the amount of xp needed per level after updating
    /// the current player level accordingly. 
    /// </summary>
    /// <param name="value"></param>
    private void SetLevel(int value)
    {
        this.level = value;
        experience = experience - experienceToNextLevel;
        experienceToNextLevel = (int)(50f * (Mathf.Pow(level + 1, 2) - (5 * (level + 1)) + 8));

    }
}
