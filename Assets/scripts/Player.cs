using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*Rigidbody2D rb;
    bool isGrounded; //地面にいるかどうか

    public float smallJumpForce = 12f; //小ジャンプの力
    public float bigJumpForce = 20f; //大ジャンプの力
    public float holdborder = 0.2f; //押す時間の境目

    private float pressTime = 0f;      //押した時間を計測する
    private bool isPressing = false;   //押しているかどうか

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // スペースキーを押し始めたら計測開始
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            pressTime = Time.time;
            isPressing = true;
        }

        // スペースキーを離した瞬間にジャンプ判定
        if (Input.GetKeyUp(KeyCode.Space) && isPressing && isGrounded)
        {
            float heldTime = Time.time - pressTime;
            float jumpPower;

            if (heldTime >= holdborder)
            {
                jumpPower = bigJumpForce;
            }
            else
            {
                jumpPower = smallJumpForce;
            }

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isGrounded = false;
            isPressing = false;
        }
    }

    // 接地判定
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }*/
    public float[] laneY = { -1f, 1.75f, 4.5f }; //３つのy座標
    private int currentLane = 0; //現在のレーン（0=下、1=中、2=上）

    void Start()
    {
        //下から登場
        Vector3 pos = transform.position;
        pos.y = laneY[currentLane];
        transform.position = pos;
    }

    void Update()
    {
        //上移動
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveLane(1);
            if(EffectManager.Instance != null)
            {
                EffectManager.Instance.PlayEffect(EffectType.jumpup, transform.position);
            }
        }
        //下移動
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveLane(-1);
            if(EffectManager.Instance != null)
            {
                EffectManager.Instance.PlayEffect(EffectType.jumpdown, transform.position);
            }
        }
    }

    void MoveLane(int direction)
    {
        int newLane = currentLane + direction;

        //レーン0or1or2だけにするための制限
        if (newLane >= 0 && newLane < laneY.Length)
        {
            currentLane = newLane;

            //移動
            Vector3 pos = transform.position;
            pos.y = laneY[currentLane];
            transform.position = pos;
        }
    }
}
