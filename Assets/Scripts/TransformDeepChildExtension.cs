using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// Credit: https://answers.unity.com/questions/799429/transformfindstring-no-longer-finds-grandchild.html
public static class TransformDeepChildExtension
{
    //Breadth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == aName)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }

    /*
    //Depth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        foreach(Transform child in aParent)
        {
            if(child.name == aName )
                return child;
            var result = child.FindDeepChild(aName);
            if (result != null)
                return result;
        }
        return null;
    }
    */
}

public static class Utils {
  public static void SetLayerRecursively(GameObject obj, int newLayer)
   {
       obj.layer = newLayer;
       foreach(Transform child in obj.transform)
       {
           SetLayerRecursively(child.gameObject, newLayer);
       }
   }

}