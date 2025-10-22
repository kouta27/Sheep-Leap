using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public Text stageScoreText;
    public Text totalScoreText; // 最後のステージのみ
    public GameObject nextButton;
    public GameObject rankingButton;

    void Start()
    {
        int stageIndex = GameManager.instance.currentStage - 1; // 直前のステージ番号
        int stageScore = GameManager.instance.stageScores[stageIndex];

        stageScoreText.text = "Stage " + (stageIndex + 1) + " Score: " + stageScore;

        if (stageIndex == 2) //ステージ３の場合
        {
            int total = GameManager.instance.GetTotalScore();
            totalScoreText.text = "Total Score: " + total;

            nextButton.SetActive(false);
            rankingButton.SetActive(true);
        }
        else
        {
            totalScoreText.text = "";
            nextButton.SetActive(true);
            rankingButton.SetActive(false);
        }
    }

    public void OnNextButton()
    {
        SceneManager.LoadScene("Stage" + (GameManager.instance.currentStage + 1));
    }

    public void OnRankingButton()
    {
        SceneManager.LoadScene("Ranking");
    }
}

