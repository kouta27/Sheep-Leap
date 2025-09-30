using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public Text stageScoreText;
    public Text totalScoreText; // �Ō�̃X�e�[�W�̂�
    public GameObject nextButton;
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
}

