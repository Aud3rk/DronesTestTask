using UnityEngine;
using UnityEngine.AI;

namespace Drones
{
    public class LootingState : IPayloadedState<Loot>
    {
        private readonly Drone _drone;

        public LootingState(Drone drone) => 
            _drone = drone;

        public void Enter(Loot loot) => 
            _drone.LootResource();

        public void Tick()
        {
        }


        public void Exit()
        {
        }
    }
}