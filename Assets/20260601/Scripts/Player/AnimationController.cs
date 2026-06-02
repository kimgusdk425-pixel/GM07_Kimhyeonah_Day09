using UnityEngine;

public class AnimationController : MonoBehaviour
{
    //애니메이터 참조변수
    [SerializeField] private Animator refAnimator;

    private void OnEnable()
    {
        if (refAnimator == null)
        {
            refAnimator = GetComponentInParent<Animator>();
        }
    }
    public void OnStateChanged(PlayerStateManager.State newState)
    {
        if (refAnimator == null) return;

        refAnimator.SetInteger("State", (int)newState);
    }
    //플레이어가 점프했다가 착지할 때
    public void OnLanding()
    {
        refAnimator.SetTrigger("Landing");
    }
    //공격 콤보 값을 변경할때
    public void SetAttackComboState(int attackCombo)
    {
        refAnimator.SetInteger("AttackCombo", attackCombo);
    }
    //현재 재생중인 애니메이션 스테이트를 반환 
    public AnimatorStateInfo GetCurrentStateInfo()
    {
        //애니메이터에서 재생하는 스테이트 정보를 반환 
        return refAnimator.GetCurrentAnimatorStateInfo(0);
    }
}
