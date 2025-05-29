using Drones;
using UnityEngine;

namespace Fabric
{
    public class DroneFabricBlue : DroneFabric
    {
        public override Drone GenerateDrone()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Drone_blue");
            var go = Object.Instantiate(prefab, SpawnPoint.position, Quaternion.identity);
            var droneComponent = go.GetComponent<BlueDrone>();

            return droneComponent;
        }
    }
}