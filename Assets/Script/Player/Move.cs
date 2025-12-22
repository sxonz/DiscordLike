using UnityEngine;
using UnityEngine.InputSystem; // New Input System 네임스페이스

public class Move : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 moveInput;
    private Rigidbody rb;

    void Awake()
    {
        // 물리 기반 이동을 위해 Rigidbody 참조 (없으면 자동으로 추가 권장)
        rb = GetComponent<Rigidbody>();

        // 만약 Rigidbody가 없다면 에러 방지를 위해 체크
        if (rb == null)
        {
            Debug.LogError("GameObject에 Rigidbody 컴포넌트가 필요합니다!");
        }
    }

    // Input System의 'OnMove' 메시지 수신 (Player Input 컴포넌트 설정 필요)
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        // 물리 연산은 FixedUpdate에서 처리하는 것이 가장 부드럽습니다.
        MovePlayer();
    }

    void MovePlayer()
    {
        // 입력받은 방향으로 속도 계산
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Rigidbody를 이용한 물리 이동 (떨림 현상 방지)
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}