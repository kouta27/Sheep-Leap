using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public GameObject player;    // プレイヤーキャラ
    public float musicEndTime = 180f; // 曲が終わる時間（秒）
    public float moveSpeed = 2f; // プレイヤー移動速度
    private bool stageClear = false;
    private float endTimer = 0f;

    void Update()
    {
        // 曲が終わったら処理開始
        if (!stageClear && Time.time >= musicEndTime)
        {
            stageClear = true;
            endTimer = 0f;
        }

        // ステージクリア中は右に移動
        if (stageClear)
        {
            if (player != null)
            {
                player.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }

            endTimer += Time.deltaTime;
            if (endTimer >= 5f)
            {
                // 5秒後に次のシーンへ移動
                SceneManager.LoadScene("Stage2");
            }
        }
    }
}

