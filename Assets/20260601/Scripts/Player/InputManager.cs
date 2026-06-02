using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   public static Vector2 Movement { get; private set; } = Vector2.zero;
    //점프 입력 저장용
    public static bool IsJump { get; private set; } = false;
    //공격 입력 저장용
    public static bool IsAttack { get; private set; } = false;

    //이동관련 입력 액션
    private InputAction moveAction;
    //점프 입력 액션
    private InputAction jumpAction;
    //공격 입력 액션
    private InputAction attackAction;

    private void Awake()
   {
        if(moveAction == null)
        {
            moveAction = InputSystem.actions.FindAction("Move");
        }
        if(jumpAction == null)
        {
            jumpAction = InputSystem.actions.FindAction("Jump");
        }
        if(attackAction == null)
        {
            attackAction = InputSystem.actions.FindAction("Attack");
        }
    }
    private void Update()
    {        
        Movement = moveAction.ReadValue<Vector2>();
        IsJump = jumpAction.WasPressedThisFrame();
        IsAttack = attackAction.WasPressedThisFrame();
    }
}
