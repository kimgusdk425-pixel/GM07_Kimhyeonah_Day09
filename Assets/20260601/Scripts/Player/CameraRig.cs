using UnityEngine;

public class CameraRig : MonoBehaviour
{
    //Å¸°Ù
    [SerializeField] private Transform followTarget;
    [SerializeField] private float movementDelay = 5.0f;

    private Transform refTransform;
    
    private void Awake()
    {
        if(refTransform == null)
        {
            refTransform = transform;
        }
        if(followTarget == null)
        {
            followTarget = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void LateUpdate()
    {
        refTransform.position = Vector3.Lerp(
            refTransform.position,
            followTarget.position, 
            movementDelay * Time.deltaTime
            );
    }
}
