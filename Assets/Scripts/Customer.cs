using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public enum CustomerState {
        Queueing,
        Seated,
        Ordered,
        Served
    }
}
