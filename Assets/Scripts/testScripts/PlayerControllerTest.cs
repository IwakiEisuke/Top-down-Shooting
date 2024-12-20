using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerTest : MonoBehaviour
{
    public float moveSpeed = 10f; // 最大移動速度
    public float velocityChangeRate = 10f; // 最大速度変更量（加速度）
    public bool isGrounded = true; // 接地判定

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 入力処理
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        // 入力に基づく目標速度
        Vector3 targetVelocity = new Vector3(inputX, 0, inputZ);
        targetVelocity = transform.TransformDirection(targetVelocity) * moveSpeed;

        // 現在の速度を取得
        Vector3 currentVelocity = rb.linearVelocity;

        // Y軸の速度は維持（重力の影響を受けるため）
        currentVelocity.y = 0;

        // 最大速度変更量を制限して補間
        Vector3 newVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, velocityChangeRate * Time.fixedDeltaTime);

        // Rigidbodyの速度を更新
        rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 地面に接地しているかを判定する例
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 地面から離れたら接地を解除
        isGrounded = false;
    }
}