using UnityEngine;

public class PathManager : MonoBehaviour
{
    [Header("wayPoint List")]
    [SerializeField] private Transform[] wayPoints;

    //현재 등록된 웨이포인트 개수를 외부에서 읽기 위한 프로퍼티
    public int WayPointCount
    {
        get
        {
            //웨이 포인트의 길이를 리턴
            return wayPoints.Length;
        }
    }

    //에너미가 현재 목표지점을 가져올 때 사용 
    public Transform GetWayPoint(int index)
    {
        if (index < 0 || index >= wayPoints.Length)
        {
            return null;
        }
        return wayPoints[index];
    }
    
}
