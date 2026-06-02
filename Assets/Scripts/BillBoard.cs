using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera targetCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetCamera == null) return;
        transform.rotation = Quaternion.LookRotation(
            targetCamera.transform.forward,
            targetCamera.transform.up
            );
    }
}
