using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Transform refTransform;

    //이동 관련 입력 액션
    private InputAction moveAction;
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private float rotationSpeed = 720.0f;
    [SerializeField] private Animator refAnimator;

    private void Awake()
    {
        if (refTransform == null)
        {
            refTransform = transform;
        }
        if (moveAction == null)
        {
            moveAction = InputSystem.actions.FindAction("Move");
        }
        if (refAnimator == null)
        {
            refAnimator = GetComponentInChildren<Animator>(); //자식 오브젝트에서 애니메이터 컴포넌트 찾아서 참조하기
        }

    }
    void Update()
    {
        //WASD / 방향키 입력 값 읽기
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        //입력 받은 방향 값을 사용해 이동할 방향 벡터 만들기
        Vector3 direction = new Vector3(moveValue.x, 0.0f, moveValue.y);
        //모든 방향에 대해서 크기가 같아지도록 벡터를 정규화
        direction.Normalize();
        //새로운 위치 = 현재위치 + 이동할 방향 벡터 * 속도 * 프레임시간
        refTransform.position = refTransform.position + direction * moveSpeed * Time.deltaTime;

        //회전
        if (direction != Vector3.zero)
        {
            //이동방향을 바라보는 회전값 만들고
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            //현재 회전에서 목표 회전으로 돌리자

            refTransform.rotation = Quaternion.RotateTowards(
                refTransform.rotation, //현재회전
                targetRotation, //목표회전
                rotationSpeed * Time.deltaTime); //회전량
        }

        //애니메이터 컴포넌트에 State 파라미터 설정하자.
        //입력값이 0,0이면 Idle
        if (moveValue == Vector2.zero)
        {
            refAnimator.SetInteger("State", 0);
        }
        //입력값이 움직이면 Move
        else
        {
            refAnimator.SetInteger("State", 1);
        }
    }
}
