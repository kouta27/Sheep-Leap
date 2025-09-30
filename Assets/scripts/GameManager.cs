using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int[] stageScores = new int[3]; //各ステージのスコア保存
    public int currentStage = -1;           //今どのステージか
    public int maxStage = 3;

    void Awake()
    {
        // シングルトン化
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //シーン切り替えても消えない
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveStageScore(int score)
    {
        if (currentStage >= 0 && currentStage < stageScores.Length)
        {
            stageScores[currentStage] = score;
        }
    }

    public int GetTotalScore()
    {
        int total = 0;
        foreach (int s in stageScores) total += s;
        return total;
    }

    public void GoToNextStage()
    {
        currentStage++;
        SceneManager.LoadScene("Result");
    }
}

