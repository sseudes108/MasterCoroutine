using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
    public static List<Star> AllStars = new();
    public bool occupied = false;

    private void Awake() {
        AllStars.Add(this);
    }
}