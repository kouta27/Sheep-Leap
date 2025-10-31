using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("レーン設定")]
    public float[] laneY = { -1f, 1.75f, 4.5f }; //３つのy座標
    private int currentLane = 0; //現在のレーン（0=下、1=中、2=上）

    [Header("浮遊アニメーション設定")]
    public float floatAmplitude = 0.15f;//上下振れ幅
    public float floatFrequency = 1.2f;//浮く速さ
    private float floatOffset;//サイン波位相、ズレ

    private float baseY; // 現在レーンの基準Y位置

    void Start()
    {
        //下から登場
        Vector3 pos = transform.position;
        pos.y = laneY[currentLane];
        transform.position = pos;

        baseY = pos.y;
        floatOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        //上へ
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveLane(1);
            if (EffectManager.Instance != null)
                EffectManager.Instance.PlayEffect(EffectType.jumpup, transform.position);
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(SoundId.Player_Jump);
        }
        //下へ
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveLane(-1);
            if (EffectManager.Instance != null)
                EffectManager.Instance.PlayEffect(EffectType.jumpdown, transform.position);
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(SoundId.Player_Jump);
        }

        //浮遊アニメーション
        ApplyFloating();
    }

    void MoveLane(int direction)
    {
        int newLane = currentLane + direction;

        //0〜2の範囲に制限
        if (newLane >= 0 && newLane < laneY.Length)
        {
            currentLane = newLane;
            baseY = laneY[currentLane];
        }
    }

    void ApplyFloating()
    {
        float floatY = baseY + Mathf.Sin(Time.time * floatFrequency + floatOffset) * floatAmplitude;
        Vector3 pos = transform.position;
        pos.y = floatY;
        transform.position = pos;
    }
}

