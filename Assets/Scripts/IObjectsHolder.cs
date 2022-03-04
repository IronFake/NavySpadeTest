using System;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IObjectsHolder
    {
        event Action<int> OnObjectCountChanged;
        
        int Count { get; }

        float GetNearestObjectTo(Vector3 pos);
    }
}