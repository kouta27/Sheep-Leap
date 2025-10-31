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
        int stageIndex = GameManager.instance.currentStage - 1; // ���O�̃X�e�[�W�ԍ�
        int stageScore = GameManager.instance.stageScores[stageIndex];

        stageScoreText.text = "Stage " + (stageIndex + 1) + " Score: " + stageScore;

        if (stageIndex == 2) //�X�e�[�W�R�̏ꍇ
        {
            int total = GameManager.instance.GetTotalScore();
            totalScoreText.text = "Total Score: " + total;
            TitleManager.totalScoreForRanking = total;//ResetGame()���s���Ă��g�[�^���X�R�A�͎c��
        }
    }


    public void OnTitleButton()
    {
        GameManager.instance.ResetGame();
        SceneManager.LoadScene("Title");
    }
}
