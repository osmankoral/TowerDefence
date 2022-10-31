using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointManager : MonoBehaviour
{
   public static Transform[] points;
   public static int childCount;

 private void Awake()
 {
     points = new Transform[transform.childCount];
     childCount = points.Length;

     for(int i = 0; i < points.Length; i++ )
     {
         points[i] = transform.GetChild(i);
     }
 }


}
