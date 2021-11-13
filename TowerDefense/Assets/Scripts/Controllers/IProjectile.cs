using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    void HitTarget();
    void TurnOff();
    void ResetProjectileData();
    void Seek(Transform target, float damage);
}
