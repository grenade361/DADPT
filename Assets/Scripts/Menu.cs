﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{


    public Text[] missionDescription, missionReward, missionProgress;
    public GameObject[] rewardButton;
    public Text coinText;
    public Text costText;
    public GameObject[] characters;

    private int characterIndex = 0;
        


    // Start is called before the first frame update
    void Start()
    {
        SetMission();
        UpdateCoins(GameManager.gm.coins);

    }
     void UpdateCoins(int coins)
        {
          coinText.text = coins.ToString();
        }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartRun()
    {
        if (GameManager.gm.characterCost[characterIndex] <= GameManager.gm.coins)
            {
            GameManager.gm.coins -= GameManager.gm.characterCost[characterIndex];
            GameManager.gm.characterCost[characterIndex] = 0;
            GameManager.gm.Save();
            GameManager.gm.StartRun(characterIndex);
        }
        
    }


    public void SetMission()
    {
        for (int i = 0; i < 2; i++)
        {
            MissionBase mission = GameManager.gm.GetMission(i);
            missionDescription[i].text = mission.GetMissionDescription();
            missionReward[i].text = " Mục tiêu " + mission.reward;
            missionProgress[i].text = mission.progress + mission.currentProgress + " / " + mission.max;
            if(mission.GetMissionComplete())
            {
                rewardButton[i].SetActive(true);
            }
        }
        GameManager.gm.Save();
    }

    public void GetReward(int missionIndex)
    {
        GameManager.gm.coins += GameManager.gm.GetMission(missionIndex).reward;
        UpdateCoins(GameManager.gm.coins);
        rewardButton[missionIndex].SetActive(false);
        GameManager.gm.GenerateMission(missionIndex);
    }

    public void ChangeCharacter(int index)
    {
        characterIndex += index;
        if (characterIndex >= characters.Length)
        {
            characterIndex = 0;
        }
        else if (characterIndex < 0)
        {

            characterIndex = characters.Length - 1;
        }

        for (int i = 0; i < characters.Length; i++)
        {
            if (i== characterIndex) 
                characters[i].SetActive(true);
                else 
                characters[i].SetActive(false);
        }
        string cost = "";
        if (GameManager.gm.characterCost[characterIndex] != 0)
        {
            cost = GameManager.gm.characterCost[characterIndex].ToString();
        }
        costText.text = cost;
    }
}
