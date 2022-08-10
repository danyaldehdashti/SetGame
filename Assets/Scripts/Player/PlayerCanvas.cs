using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [Header("Dependency")]
    
    [SerializeField] private TMP_Text tmpText;

    [SerializeField] private List<GameObject> bullets;

    [SerializeField] private List<GameObject> playerObjects;

    [SerializeField] private List<TMP_Text> playerScores;
    
    [SerializeField] private TMP_Text score;

    [SerializeField] private Image timerImage;
    


    public void SetTimer(float value)
    {
        tmpText.text = value.ToString("0");

        timerImage.fillAmount = value / 10;
    }

    public void SetBullets(int count)
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i].SetActive(false);
        }
        
        for (int i = 0; i < count; i++)
        {
            bullets[i].SetActive(true);
        }
    }

    public void SetScoreBoard(List<PlayerData> playersData, int id)
    {
        PlayerData thisPlayer = new PlayerData();
        
        for (int i = 0; i < playersData.Count; i++)
        {
            if (playersData[i].inGameId == id)
            {
                thisPlayer = playersData[i];
                playersData.RemoveAt(i);
            }
        }

        score.text = thisPlayer.score.ToString();

        for (int i = 0; i < playersData.Count; i++)
        {
            playerObjects[i].SetActive(true);
            playerScores[i].text = playersData[i].score.ToString();
        }
    }
}
