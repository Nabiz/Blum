using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
    new private void Start()
    {
        health = 2;
        base.Start();
    }
}
