using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int[] stageScores = new int[3]; //�e�X�e�[�W�̃X�R�A�ۑ�
    public int currentStage = -1;           //���ǂ̃X�e�[�W��
    public int maxStage = 3;

    void Awake()
    {
        // �V���O���g����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //�V�[���؂�ւ��Ă������Ȃ�
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

