using UnityEngine;
using UnityEngine.Events;

public class PlayerJumpState : PlayerStateBase
{
    //점프를 할 때 위로 향하는 속도
    [SerializeField] private float jumpPower = 8.0f;
    //점프 상태 일 때 플레이어의 Y축 속도
    [SerializeField] private float verticalSpeed = 0.0f;
    //점프할 때 적용할 중력
    [SerializeField] private float gravityInJump = 10.0f;
    //플레이어가 공중에 떠 있을 때 앞으로 이동하는 속력
    [SerializeField] private float moveSpeed = 5.0f;
    //플레이어가 점프를 했다가 지면에 발을 딛을 때 발생하는 이벤트
    [SerializeField] private UnityEvent OnJumpEnd; 

    protected override void OnEnable()
    {
        base.OnEnable();
        //점프가 시작되면 플레이어가 위로 올라갈 수 있도록 힘 적용하자.
        verticalSpeed = jumpPower;
    }
    protected override void Update()
    {
        //이동방향을 계산할 때 사용할 변수
        Vector3 movement = Vector3.zero;
        //캐릭터 컨트롤러가 지면에 닿으면 기본스테이트로 바꾸자
        if(verticalSpeed < 0.0f && manager.IsGrounded)
        {
            //점프 종료 알림
            OnJumpEnd?.Invoke();
            //기본스테이트 전환
            manager.SetState(PlayerStateManager.State.Idle);
        }
        else
        {
            //위로 향하는 힘이 0보다 크면 위쪽으로 이동하는 상태
            if(verticalSpeed > 0.0f)
            {
                //위로 적용되는 힘을 줄이자.(그래야 떨어지니까)
                verticalSpeed -= gravityInJump * Time.deltaTime;
            }
            //점프 높이의 정점에 도달했는지 확인하자.
            //verticalSpeed값이 0과 매우 비슷한지 확인
            if(Mathf.Approximately(verticalSpeed, 0.0f))
            {
                verticalSpeed = 0.0f;
            }
            //떠 있는 동안에는 중력적용
            verticalSpeed += Physics.gravity.y * Time.deltaTime;
            //플레이어가 공중에 떠 있을 때 앞방향으로 이동할 수 있도록 속도 설정
            movement = moveSpeed * refTransform.forward * Time.deltaTime;
            //계산된 위쪽 방향으로 힘을 이동방향에 더하자
            movement += Vector3.up * verticalSpeed * Time.deltaTime;
        }
        //캐릭터 컨트롤러를 통해 이동하자.
        characterController.Move(movement);
    }
}
