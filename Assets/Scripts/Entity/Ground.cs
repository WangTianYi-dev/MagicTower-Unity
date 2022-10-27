using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : Entity
{
    protected override void Awake()
    {
        base.Awake();
        this.type = EntityType.Ground;
    }

    protected override void Start()
    {
        base.Start();
        this.passable = true;
    }
}
