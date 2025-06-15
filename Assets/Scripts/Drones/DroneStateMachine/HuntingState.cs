using UnityEngine;
using UnityEngine.AI;

namespace Drones
{
    public class HuntingState : IPayloadedState<Loot>
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly BaseStation _base;
        private readonly Drone _drone;
        private Transform _droneTransform;
        private float _range;
        private DroneStateMachine _droneStateMachine;

        public HuntingState(NavMeshAgent navMeshAgent, BaseStation baseStation, Drone drone, Transform droneTransform, float range, DroneStateMachine droneStateMachine)
        {
            _navMeshAgent = navMeshAgent;
            _base = baseStation;
            _drone = drone;
            _droneTransform = droneTransform;
            _range = range;
            _droneStateMachine = droneStateMachine;
        }

        public void Enter(Loot loot)
        {
            if (_base.TryToLoot(loot, _drone)) 
                _navMeshAgent.SetDestination(loot.transform.position);
            else
                _droneStateMachine.Enter<SearchingState>();
        }

        public void Tick()
        {
            if(Vector3.Distance(_droneTransform.position, _navMeshAgent.destination) <= _range)
                _droneStateMachine.Enter<SearchingState>();
        }


        public void Exit() => 
            _navMeshAgent.ResetPath();
    }
}