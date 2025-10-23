using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int[] stageScores = new int[4]; //各ステージのスコア保存
    public int currentStage = 0;           //今どのステージか
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
        if (currentStage > 0 && currentStage < stageScores.Length)
        {
            stageScores[currentStage - 1] = score;
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
        if (currentStage == 0)//stage0なら
        {
            SceneManager.LoadScene("Stage1");
        }
        else
        {
            SceneManager.LoadScene("Result");
        }
    }
}

