using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("�̵� ����")]
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float drag = 5.0f; // ������ (�ڿ������� ����)

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // ������ٵ� ���� ����ȭ
        rb.linearDamping = drag;
        rb.useGravity = true;
        rb.freezeRotation = true; // �̵� �� �Ѿ��� ����
        rb.interpolation = RigidbodyInterpolation.Interpolate; // ȭ�� ���� ����
    }

    void FixedUpdate()
    {
        // 1. �Է� �ޱ� (Input Manager ���)
        // GetAxis�� -1.0���� 1.0 ���̸� �ε巴�� �����ϴ� (���ӵ� ����)
        float h = Input.GetAxis("Horizontal"); // A, D
        float v = Input.GetAxis("Vertical");   // W, S

        // 2. �̵� ���� ���
        Vector3 moveDir = new Vector3(h, 0, v).normalized;

        // 3. ���� �� ���ϱ� (Velocity�� ���� �ǵ帮�� �ͺ��� �ε巯��)
        // Time.fixedDeltaTime�� ���� ������ ����
        if (moveDir.magnitude > 0.1f)
        {
            rb.AddForce(moveDir * moveSpeed * 10f, ForceMode.Force);
        }
    }
}