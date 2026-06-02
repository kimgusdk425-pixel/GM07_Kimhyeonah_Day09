using UnityEngine;

public class PlayerStateBase : MonoBehaviour
{
    protected Transform refTransform;
    protected CharacterController characterController;
    protected Animator refAnimator;
    //++
    protected PlayerStateManager manager;
    protected AnimationController animationController;
    
    protected virtual void OnEnable()
    {
        if (refTransform == null)
        {
            refTransform = transform;
        }
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
        if (refAnimator == null)
        {
            refAnimator = GetComponent<Animator>();
        }
        //++
        if(manager == null)
        {
            manager = GetComponent<PlayerStateManager>();
        }
        if(animationController == null)
        {
            animationController = GetComponentInChildren<AnimationController>();
        }
    }
    protected virtual void Update()
    {
        characterController.Move(Physics.gravity * Time.deltaTime);
    }

    protected virtual void OnDisable()
    {
        

    }

    //RootMotion을 사용해 Animator가 이동할때 실행되는 메서드
    //Animator의 RootMotion옵션을 체크하면 애니메이션 시스템이 트랜스폼을 직접 제어함
    //하지만 애니메이션 시스템이 제어하지 않고 이동한 거리값을 받아 직접 이동 기능을 구현할 때 해당 메서드를 사용한다.
    protected virtual void OnAnimatorMove()
    {
        //RootMotion을 통해 애니메이션 이동한 거리는 deltaPosition으로 얻을 수 있다.
        //이 deltaPosition을 characterController.Move()에 전달하여 실제로 캐릭터가 이동하도록 함.
        characterController.Move(refAnimator.deltaPosition);
    }
}
