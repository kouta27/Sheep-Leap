using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
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
    }
}
