using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Note : MonoBehaviour
{
    public float moveSpeed = 5f; //�ړ����x
    public NoteData noteData;

    private bool handled = false; //�����ς݂��ǂ���
    public float missOffset = 0.2f; //���胉�C���������߂�����~�X
    private float hitX;

    // Spawner����Ă΂�鏉����
    public void Init(Vector3 target, float speed, NoteData data)
    {
        hitX = target.x;
        moveSpeed = speed;
        noteData = data;
    }

    void Update()
    {
        //�m�[�c�����ֈړ�
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        //���胉�C����ʂ�߂����̂ɏ�������Ă��Ȃ���΃~�X����
        if (!handled && transform.position.x <= hitX - missOffset)
        {
            handled = true;
            if (ScoreManager.instance != null)
                ScoreManager.instance.OnMiss();
        }

        //��ʊO�ɗ��ꂽ��폜
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

            //�G�t�F�N�g��SE�͂����ōĐ��I
            if(EffectManager.Instance != null)
            {
                var effectObj = EffectManager.Instance.PlayEffect(EffectType.GetEffect, transform.position, Quaternion.identity);
                // PooledEffect�Ƃ��Ă̏��������K�v�Ȃ炱���ōs��
            }
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(SoundId.Coin_Get);
            }
            Destroy(gameObject);
        }
    }
}
