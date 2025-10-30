using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Note : MonoBehaviour
{
    public float moveSpeed = 5f; //移動速度
    public NoteData noteData;

    private bool handled = false; //処理済みかどうか
    public float missOffset = 0.2f; //判定ラインを少し過ぎたらミス
    private float hitX;

    // Spawnerから呼ばれる初期化
    public void Init(Vector3 target, float speed, NoteData data)
    {
        hitX = target.x;
        moveSpeed = speed;
        noteData = data;
    }

    void Update()
    {
        //ノーツを左へ移動
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        //判定ラインを通り過ぎたのに処理されていなければミス扱い
        if (!handled && transform.position.x <= hitX - missOffset)
        {
            handled = true;
            if (ScoreManager.instance != null)
                ScoreManager.instance.OnMiss();
        }

        //画面外に流れたら削除
        if (transform.position.x < hitX - 20f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (handled) return;

        if (other.CompareTag("Player"))
        {
            handled = true;
            int baseScore = 100;

            if (ScoreManager.instance != null)
                ScoreManager.instance.OnNoteHit(baseScore);

            //エフェクトやSEはここで再生！
            if(EffectManager.Instance != null)
            {
                var effectObj = EffectManager.Instance.PlayEffect(EffectType.GetEffect, transform.position, Quaternion.identity);
                // PooledEffectとしての初期化が必要ならここで行う
            }
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(SoundId.Coin_Get);
            }
            Destroy(gameObject);
        }
    }
}
