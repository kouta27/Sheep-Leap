using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public GameObject player;    // �v���C���[�L����
    public float musicEndTime = 180f; // �Ȃ��I��鎞�ԁi�b�j
    public float moveSpeed = 2f; // �v���C���[�ړ����x
    private bool stageClear = false;
    private float endTimer = 0f;

    void Update()
    {
        // �Ȃ��I������珈���J�n
        if (!stageClear && Time.time >= musicEndTime)
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
            if (endTimer >= 5f)
            {
                // 5�b��Ɏ��̃V�[���ֈړ�
                SceneManager.LoadScene("Stage2");
            }
        }
    }
}

