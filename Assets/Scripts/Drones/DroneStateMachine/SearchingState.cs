using Extentions;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Drones
{
    public class SearchingState : IState
    {
        private readonly Transform _droneTransform;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly LootDetector _lootDetector;
        private readonly Vector3 _lowPoint;
        private readonly Vector3 _highPoint;
        private readonly float _range;

        public SearchingState(Transform droneTransform, NavMeshAgent navMeshAgent,
            float range, Vector3 lowPoint, Vector3 highPoint, LootDetector lootDetector)
        {
            _droneTransform = droneTransform;
            _navMeshAgent = navMeshAgent;
            _range = range;
            _lowPoint = lowPoint;
            _highPoint= highPoint;
            _lootDetector = lootDetector;
        }

        public void Enter()
        {
            //_lootDetector.FindLoot();
            _navMeshAgent.SetDestination(RandomExts.GeneratePoint(_lowPoint, _highPoint));
        }

        public void Exit() => 
            _navMeshAgent.ResetPath();

        public void Tick()
        {
            if(Vector3.Distance(_droneTransform.position, _navMeshAgent.destination) <= _range)
                Enter();
        }
    }
}