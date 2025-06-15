using UnityEngine;
using UnityEngine.AI;

namespace Drones
{
    public class DeliveringState : IPayloadedState<int>
    {
        private Drone _drone;
        private Transform _droneTransform;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly DroneStateMachine _droneStateMachine;
        private readonly Vector3 _baseStationPosition;
        private int _countDelivery;
        private float _range;


        public DeliveringState(Drone drone, NavMeshAgent navMeshAgent, DroneStateMachine droneStateMachine,
            Vector3 baseStationPosition, float range)
        {
            _drone = drone;
            _navMeshAgent = navMeshAgent;
            _baseStationPosition = baseStationPosition;
            _droneStateMachine = droneStateMachine;
            _droneTransform = drone.transform;
            _range = range;

        }
        public void Enter(int countDeliver)
        {
            _countDelivery = countDeliver;
            _navMeshAgent.SetDestination(_baseStationPosition);
        }

        public void Exit()
        {
            
        }

        public void Tick()
        {
            if(Vector3.Distance(_droneTransform.position,_navMeshAgent.destination ) <= _range*1.5f)
            {
                _drone.UnloadLoot(_countDelivery);
                _droneStateMachine.Enter<SearchingState>();
            }
        }
    }
}