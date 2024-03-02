using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Door link;
    public GravityDirection direction;
}
public enum GravityDirection { Left, Right, Up, Down }