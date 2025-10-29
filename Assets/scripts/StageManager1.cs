using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public GameObject player;    // �v���C���[�L����
    public float musicEndTime; //�Ȃ��I��鎞�ԁi�b�j
    public float moveSpeed; // �v���C���[�ړ����x
    private bool stageClear = false;
    private float endTimer = 0f;
    private float sceneStartTime; //�V�[���J�n����

    void Start()
    {
        //�V�[���ǂݍ��ݎ��̎��Ԃ��L�^
        sceneStartTime = Time.time;
    }
    void Update()
    {
        float elapsedTime = Time.time - sceneStartTime;
        // �Ȃ��I������珈���J�n
        if (!stageClear && elapsedTime >= musicEndTime)
        {
            stageClear = true;
            endTimer = 0f;
        }

        // �X�e�[�W�N���A���͉E�Ɉړ�
        if (stageClear)
        {
            if (player != null)
            {
                player.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }

            endTimer += Time.deltaTime;
            if (endTimer >= 4f)
            {
                // �X�R�A��GameManager�ɕۑ�
                if (ScoreManager.instance != null && GameManager.instance != null)
                {
                    GameManager.instance.SaveStageScore(ScoreManager.instance.score);
                }
                // 4�b��Ɏ��̃V�[���ֈړ�
                GameManager.instance.GoToNextStage();
            }
        }
    }
}

