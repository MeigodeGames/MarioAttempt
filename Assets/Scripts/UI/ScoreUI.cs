using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public int m_Score;
    public string m_Mask = "000000000";
    private Text m_Text;

    private void Start()
    {
        m_Text = GetComponent<Text>();
        m_Score = DataManager.instance.score;
        UpdateUI();
    }

    public void UpdateUI()
    {
        m_Score = DataManager.instance.score;
        m_Text.text = m_Score.ToString(m_Mask);
    }
}

