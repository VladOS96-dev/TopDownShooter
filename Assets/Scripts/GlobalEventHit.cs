using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEventHit 
{
    public static System.Action<CollisionTarget> OnHit { get; set; }
    public static int CountEventSubscribe;
    public static void InvokeOnHit(CollisionTarget collisionTarget)
    { 
    
    OnHit?.Invoke(collisionTarget);
    }
    public static void SubscribeEvent()
    {
        CountEventSubscribe++;
    }
    public static void UnsubcribeEvent()
    {
        CountEventSubscribe--;
    }
}
