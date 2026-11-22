using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatroPath : MonoBehaviour
{
    [SerializeField] float wayPointGizmosRadius = 1;//waypoint的半徑

    //取得下一個waypoint編號
    public int GetNextWayPointNum(int wayPointNum)
    {
        if (wayPointNum + 1 > transform.childCount - 1)
        {
            return 0;
        }
        return wayPointNum + 1;
    }

    //取得waypoint的位置
    public Vector3 GetWayPointPosition(int wayPointNum)
    {
        return transform.GetChild(wayPointNum).position;
    }

    //畫出巡邏路徑
    //此為UNITY自動呼叫不用放在UPDATE
    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.cyan;
            int j = GetNextWayPointNum(i);//下一個waypoint編號
            Gizmos.DrawLine(GetWayPointPosition(i), GetWayPointPosition(j));//從編號i到編號j畫一條線
            Gizmos.DrawSphere(GetWayPointPosition(i), wayPointGizmosRadius);//畫出waypoint
        }
    }
}
