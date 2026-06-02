using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private LayerMask buildTowerlayer;

    private Camera mainCam;
    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            BuildTower();
        }
    }
    private void BuildTower()
    {
        if (mainCam == null) return;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition); //현재 마우스 위치 기준으로?

        if(Physics.Raycast(ray, out RaycastHit hit, 100.0f, buildTowerlayer))
        {
            if(hit.collider.TryGetComponent(out BuildTile buildTile))
            {
                if (buildTile.CanBuild() == false)
                {
                    return; 
                }

                Vector3 buildPosition = hit.collider.transform.position;

                buildPosition.y = 0.5f;
                Instantiate(towerPrefab,buildPosition, Quaternion.identity);

                buildTile.SetBuild();
            }
        }

    }
}
