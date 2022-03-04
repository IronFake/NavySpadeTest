using UnityEngine;
using UnityEngine.AI;

namespace Utils
{
    public static class NavMeshUtil
    {
        public static bool TryGetRandomPoint(Vector3 center, float maxDistance, out Vector3 result)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * maxDistance + center;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, maxDistance, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = Vector3.zero;
            return false;
        }
    }
}