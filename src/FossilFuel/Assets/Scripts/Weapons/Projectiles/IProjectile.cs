using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    float ExplodeTimer { get; }

    void Explode();
}
