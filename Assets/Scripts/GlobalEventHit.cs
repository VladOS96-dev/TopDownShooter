using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEventHit 
{
    public static System.Action<CollisionTarget> OnHit { get; set; }
    public static System.Action<string> OnInfo { get; set; }
    public static void InvokeOnHit(CollisionTarget collisionTarget)
    { 
    
    OnHit?.Invoke(collisionTarget);
    }
    public static void InvokeOnInfo(string info)
    {
        OnInfo?.Invoke(info);
    }
}
