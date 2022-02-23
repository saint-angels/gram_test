using UnityEngine;

namespace Tactics.Helpers.ObjectPool
{
    // Added to freshly instantiated objects, to link to the correct pool on despawn.
    public class PoolMember : MonoBehaviour
    {
        public ObjectPool.Pool myPool;
    }
}
