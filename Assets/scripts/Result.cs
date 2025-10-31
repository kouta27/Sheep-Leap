using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultUI : MonoBehaviour
{
    public TextMeshProUGUI stageScoreText;
    public TextMeshProUGUI totalScoreText;
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
            TitleManager.totalScoreForRanking = total;//ResetGame()が行われてもトータルスコアは残る
        }
    }


    public void OnTitleButton()
    {
        GameManager.instance.ResetGame();
        SceneManager.LoadScene("Title");
    }
}
