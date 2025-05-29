using Drones;
using UnityEngine;

namespace Fabric
{
    public class DroneFabricRed : DroneFabric
    {
        public override Drone GenerateDrone()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Drone_red");
            var go = Object.Instantiate(prefab, SpawnPoint.position, Quaternion.identity);
            var droneComponent = go.GetComponent<RedDrone>();

            return droneComponent;
        }
    }
}