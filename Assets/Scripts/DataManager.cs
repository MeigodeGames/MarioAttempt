using UnityEngine;

/// <summary>Manages data for persistance between levels.</summary>
public class DataManager : MonoBehaviour 
{
    /// <summary>Static reference to the instance of our DataManager</summary>
    public static DataManager instance;

    public int score = 0;
    public int coins = 0;
    public int lives = 3;
    private int m_PowerLevel = 0;

    public ScoreUI m_ScoreUI;

    /// <summary>Awake is called when the script instance is being loaded.</summary>
    void Awake()
    {
        // If the instance reference has not been set, yet, 
        if (instance == null)
        {
            // Set this instance as the instance reference.
            instance = this;
        }
        else if(instance != this)
        {
            // If the instance reference has already been set, and this is not the
            // the instance reference, destroy this game object.
            Destroy(gameObject);
        }

        // Do not destroy this object, when we load a new scene.
        DontDestroyOnLoad(gameObject);
    }

    public void addScore(int score) {
        score += score;
        m_ScoreUI.UpdateUI();
    }

    public void setPowerLevel(int level) {
        m_PowerLevel = level;
    }

    public bool isBig() {
        return (m_PowerLevel>0);
    }
}