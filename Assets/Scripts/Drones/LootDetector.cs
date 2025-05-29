using System;
using UnityEngine;

namespace Drones
{
    public class LootDetector : MonoBehaviour
    {
        public event Action<Loot> FoundLoot;
        public LayerMask layer;
        private float _radius;

    
        private void OnTriggerEnter(Collider other) => 
            FindLoot();

        public void FindLoot()
        {
            Loot loot = null;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10, layer);
            float minimalDistance = float.MaxValue;
            if (hitColliders.Length == 0)
                return;
            foreach (var collider in hitColliders)
            {
                float newDistance = Vector3.Distance(transform.position, collider.transform.position);
                if (newDistance<minimalDistance)
                {
                    minimalDistance = newDistance;
                    loot = collider.GetComponent<Loot>();
                }
            }
            FoundLoot?.Invoke(loot);
        }

    }
}