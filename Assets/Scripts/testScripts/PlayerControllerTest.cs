using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerTest : MonoBehaviour
{
    public float moveSpeed = 10f; // �ő�ړ����x
    public float velocityChangeRate = 10f; // �ő呬�x�ύX�ʁi�����x�j
    public bool isGrounded = true; // �ڒn����

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // ���͏���
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        // ���͂Ɋ�Â��ڕW���x
        Vector3 targetVelocity = new Vector3(inputX, 0, inputZ);
        targetVelocity = transform.TransformDirection(targetVelocity) * moveSpeed;

        // ���݂̑��x���擾
        Vector3 currentVelocity = rb.linearVelocity;

        // Y���̑��x�͈ێ��i�d�͂̉e�����󂯂邽�߁j
        currentVelocity.y = 0;

        // �ő呬�x�ύX�ʂ𐧌����ĕ��
        Vector3 newVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, velocityChangeRate * Time.fixedDeltaTime);

        // Rigidbody�̑��x���X�V
        rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �n�ʂɐڒn���Ă��邩�𔻒肷���
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // �n�ʂ��痣�ꂽ��ڒn������
        isGrounded = false;
    }
}