using System;
using System.Collections;
using Extentions;
using UnityEngine;
using UnityEngine.AI;

namespace Drones
{
    public abstract class Drone : MonoBehaviour
    {
        public event Action<int> Unload;
        public event Action<Loot> TookLoot;
        protected NavMeshAgent _navMeshAgent;

        [SerializeField] private LootDetector lootDetector;
        [SerializeField] private float _range = 1f;

        private DroneStateMachine _droneStateMachine;
        private int _countDelivery;

        public void Init(Vector3 lowPoint, Vector3 highPoint, Vector3 basePosition, BaseStation baseStation)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _droneStateMachine = new DroneStateMachine(this, _navMeshAgent, _range, lowPoint, highPoint, baseStation, 
                basePosition, lootDetector);
            lootDetector.FoundLoot += TryToSwitchTarget;
        }

        private void Update() => 
            _droneStateMachine.Tick();

        private void OnTriggerEnter(Collider other)
        {
            if (_droneStateMachine.GetActiveState().GetType()==typeof(HuntingState))
            {
                var loot = other.GetComponent<Loot>();
                _countDelivery = loot.Take();
                _droneStateMachine.Enter<LootingState, Loot>(loot);
                TookLoot?.Invoke(loot);
                Destroy(loot.gameObject);
                StartCoroutine(LootResource());
            }
        }

        public void StartSearchingLoot() => 
            _droneStateMachine.Enter<SearchingState>();

        public void SetSpeed(int speed) =>
            _navMeshAgent.speed = speed;

        public void UnloadLoot(int countDelivery) => 
            Unload?.Invoke(countDelivery);

        public IEnumerator LootResource()
        {
            yield return new WaitForSeconds(2f);
            _droneStateMachine.Enter<DeliveringState, int>(_countDelivery);
        }

        private void TryToSwitchTarget(Loot loot)
        {
            if(_droneStateMachine.GetActiveState().GetType()==typeof(SearchingState))
                _droneStateMachine.Enter<HuntingState, Loot>(loot);
        }

        private void OnDestroy() =>
            lootDetector.FoundLoot -= TryToSwitchTarget;
    }
}