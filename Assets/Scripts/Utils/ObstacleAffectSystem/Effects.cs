using System;
using Unity.VisualScripting;
using UnityEngine;

public class Effects
{
    public int Damage { get; set; } 
    public Vector3 Force { get; set; }
    public float Durration { get; set; }
    public Func<object, object> CallBack { get; set; }
}
