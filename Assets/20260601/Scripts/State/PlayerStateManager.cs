using UnityEngine;
using UnityEngine.Events;

public class PlayerStateManager : MonoBehaviour
{
    //플레이어의 스테이트를 나타내는 열거형
    public enum State
    {
        None = -1,Idle,Move,Jump,Attack,Length
    }
    //공격 콤보용 열거
    public enum AttackCombo
    {
        None = -1, Combo1, Combo2, Combo3, Combo4, Length
    }
    //플레이어의 현재 상태를 나타내는 열거형 변수
    [SerializeField] private State state = State.None;
    //플레이어의 각 스테이트 컴포넌트를 제어하기 위한 배열
    [SerializeField] private PlayerStateBase[] states;
    //플레어어 스테이트가 변경될때 발행될 이벤트
    [SerializeField] private UnityEvent<State> OnStateChanged;

    //캐릭터가 지면에 있는지 확인하기 위해서 캐릭터 컨트롤러를 사용하자.
    private CharacterController characterController;
    //애니메이션 컨트롤러
    private AnimationController animationController;

    //플레이어가 지면에 있는지 
    public bool IsGrounded { get; private set; }
    //다음에 이어질 공격 콤보 값을 보여줌
    public AttackCombo NextAttackCombo {  get; private set; } = AttackCombo.None;

    private void Awake()
    {
        if(characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
        if(animationController == null)
        {
            animationController = GetComponentInChildren<AnimationController>();
        }
        //
        PlayerAttackState attackState = GetComponent<PlayerAttackState>();
        if(attackState != null)
        {
            //넣어야 됨.
            attackState.SubscribeOnAttackEnd(OnAttackEnd);
        }
    }

    private void OnEnable()
    {
        //게임이 시작되면 대기 스테이트로..
        SetState(State.Idle);
    }
    private void Update()
    {
        //++ 점프 스테이트라면 고려하지 말자.
        if (state == State.Jump) return;

        if (InputManager.IsAttack)
        {
            //현재 스테이트가 공격이 아니면 공격으로..
            if (state != State.Attack) 
            {
                //콤보 1로
                NextAttackCombo = AttackCombo.Combo1;
                //공격 스테이트로 전환
                SetState(State.Attack);

                //애니메이터의 AttackCombo값을 Combo1로 설정
                animationController.SetAttackComboState((int)NextAttackCombo);
                return;
            }
            AnimatorStateInfo currentAnimationState = animationController.GetCurrentStateInfo();
            //현재 재생중인 공격 애니메이션의 스테이트의 이름이 AttackCombo1라면 다음 콤보는 Combo2
            if (currentAnimationState.IsName("AttackCombo1"))
            {
                NextAttackCombo =AttackCombo.Combo2;
            }
            else if (currentAnimationState.IsName("AttackCombo2"))
            {
                NextAttackCombo =AttackCombo.Combo3;
            }
            else if (currentAnimationState.IsName("AttackCombo3"))
            {
                NextAttackCombo =AttackCombo.Combo4;
            }
            else
            {
                NextAttackCombo = AttackCombo.None;
            }
            return;
        }
        if (state == State.Attack) return;
        //++
        if(IsGrounded && state == State.Move && InputManager.IsJump)
        {
            //플레이어는 공중에 있음
            IsGrounded = false;
            SetState(State.Jump);
            return;
        }

        if(InputManager.Movement==Vector2.zero)
        {
            SetState(State.Idle);
        }
        else
        {
            SetState(State.Move);
        }
    }
    //플레이어의 스테이트를 변경하는 메서드
    public void SetState(State newState)
    {
        //플레이어의 현재 스테이트와 변경하려는 스테이트가 같다면 리턴
        if(state==newState) return;
        //현재 스테이트가 none이 아니면 현재 스테이트를 비활성화
        if (state != State.None)
        {
            states[(int)state].enabled = false;
        }
        //새로운 스테이트를 활성화 하고
        states[(int)newState].enabled = true;
        //ㅎ현재 스테이트를 나타내는 변수의 값을 새로운 스테이트로 바꾸자
        state = newState;
        //스테이트가 변경되었다는 이벤트 발행하자.
        OnStateChanged?.Invoke(state);

    }
    //++
    private void OnAnimatorMove()
    {
        //캐릭터 컨트롤러를 통해서 플레이어가 지면에 있는지 확인하고 저장
        IsGrounded = characterController.isGrounded; 
    }
    //플레이어의 공격 스테이트가 종료될 때
    private void OnAttackEnd()
    {
        NextAttackCombo = AttackCombo.None;
        SetState(State.Idle);
        animationController.SetAttackComboState((int)NextAttackCombo);
    }
}
