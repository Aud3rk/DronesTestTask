using Drones;
using UnityEngine;

namespace Fabric
{
    public abstract class DroneFabric
    {
        public Transform SpawnPoint;
        public abstract Drone GenerateDrone();

    }
}