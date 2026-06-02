using UnityEngine;

public class PlayerMoveState : PlayerStateBase
{
    [SerializeField] private float rotationSpeed = 540.0f;

    protected override void Update()
    {
        base.Update();

        //입력받은 방향값을 사용해서 이동할 방향 벡터를 만들자.
        Vector3 direction = new Vector3(InputManager.Movement.x, 0.0f, InputManager.Movement.y);

        if (direction.sqrMagnitude > 1.0f)
        {
            direction.Normalize();
        }
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            refTransform.rotation = Quaternion.RotateTowards(refTransform.rotation,
                targetRotation,
               rotationSpeed * Time.deltaTime);
        }
    }

}
