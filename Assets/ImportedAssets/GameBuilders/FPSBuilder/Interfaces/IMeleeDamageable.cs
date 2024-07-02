//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

namespace GameBuilders.FPSBuilder.Interfaces
{
    public interface IMeleeDamageable : IDamageable
    {
        void MeleeDamage(float damage, Vector3 targetPosition, Vector3 hitPosition, string hitBodyPartTag = null);
    }
}