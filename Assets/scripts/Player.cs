/*using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    bool isGrounded;               // 地面にいるかどうか
    public float jumpForce = 7f;   // ジャンプの強さ
    public float holdDuration = 0.5f; // 最大で何秒ふんばれるか

    float holdTimer;
    float defaultGravity;          // 通常の重力値

    void Start()
    {
        Debug.Log("start1");
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale; // 初期重力を記録
        Debug.Log("start2");
    }

    void Update()
    {
        // スペースキーを押した瞬間 → ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("keyDown1");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            holdTimer = 0;
            Debug.Log("keyDown2");
        }

        // スペースキー長押し → 空中でふんばり
        if (Input.GetKey(KeyCode.Space) && !isGrounded)
        {
            Debug.Log("key1");
            if (holdTimer < holdDuration)
            {
                Debug.Log("key1to1");
                rb.gravityScale = 0f; // 重力をゼロにして空中で止まる
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // 上下移動を止める
                holdTimer += Time.deltaTime;
                Debug.Log("key1to2");
            }
            Debug.Log("key2");
        }

        // キーを離した or 最大ふんばり時間を超えた → 重力を戻す
        if (Input.GetKeyUp(KeyCode.Space) || holdTimer >= holdDuration)
        {
            Debug.Log("keyup1");
            rb.gravityScale = defaultGravity;
            Debug.Log("keyup2");
        }
    }

    // 地面に着地したらリセット
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colliIn");
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("colli1");
            isGrounded = true;
            rb.gravityScale = defaultGravity; // 念のためリセット
            Debug.Log("colli2");
        }
        Debug.Log("colliOut");
    }
}*/
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    bool isGrounded;//地面にいるかどうか
    public float jumpForce = 18.0f;//ジャンプの強さ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //スペースキーを押してジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    //接地判定
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
