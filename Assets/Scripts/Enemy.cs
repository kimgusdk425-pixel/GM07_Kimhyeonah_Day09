using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("move Setting")]
    [SerializeField] private float moveSpeed = 2.5f;
    [Header("Hp Setting")]
    [SerializeField] private int maxHp = 3;
    [Header("Score Setting")]
    [SerializeField] private int scoreValue = 10;
    [Header("Hp UI")]
    [SerializeField] private Slider hpSlider;

    private int currentHp;
    private int currentWayPointIndex;
    private PathManager pathManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp; //현재 체력을 최대 체력으로 초기화
        currentWayPointIndex = 0;
        pathManager = FindAnyObjectByType<PathManager>();
        UpdateHPUI();
    }

    // Update is called once per frame
    void Update()
    {
        MovePath();
    }
    private void MovePath()
    {
        if(pathManager == null) return;

        Transform targetWayPoint = pathManager.GetWayPoint(currentWayPointIndex);
        
        if(targetWayPoint == null) return;

        Vector3 dir = targetWayPoint.position - transform.position;

        transform.position += dir.normalized * moveSpeed * Time.deltaTime;
        
        transform.LookAt(targetWayPoint);

        float distance = Vector3.Distance(transform.position,targetWayPoint.position);

        if (distance < 0.2f)
        {
            currentWayPointIndex++;
            if(currentWayPointIndex >= pathManager.WayPointCount)
            {
                //죽었음?
                EndPoint();
            }
        }
    }
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        UpdateHPUI();
        if (currentHp < 0)
        {
            Die();
        }
    }
    private void Die()
    {
        //적이 죽었을때 점수를 올리고 파괴를 하자
        if(GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }
        Destroy(gameObject);
    }
    private void EndPoint()
    {
        //여기서 라이프감소 기능을..추가할 수도 있지 않겠음?
        Destroy(gameObject);
    }
    private void UpdateHPUI()
    {
        if (hpSlider == null) return;
        hpSlider.value = (float)currentHp / maxHp;

    }
}
