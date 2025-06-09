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
    
        [SerializeField] private LootDetector lootDetector;
        [SerializeField] private float _range =1f;
    
        protected NavMeshAgent _navMeshAgent;
    
        private Vector3 _basePosition;
        private Vector3 _lowPoint;
        private Vector3 _highPoint;

        private bool _isSearching = false;
        private bool _isDelivering = false;
        private int _countDelivery =0;
        private BaseStation _base;
        private bool isMovingToTarget=false;

        public void Init(Vector3 lowPoint, Vector3 highPoint, Vector3 basePosition, BaseStation baseStation)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _lowPoint = lowPoint;
            _highPoint = highPoint;
            _basePosition = basePosition;
            _base = baseStation;
            lootDetector.FoundLoot += TryToSwitchTarget;
            
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _navMeshAgent.destination) <= _range)
            {
                if(_isDelivering)
                    UnloadLoot();
                if(_isSearching)
                    StartSearchingLoot();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isDelivering)
            {
                var loot = other.GetComponent<Loot>();
                _navMeshAgent.destination = transform.position;
                _countDelivery = loot.Take();
                TookLoot?.Invoke(loot);
                Destroy(loot.gameObject);
                StartCoroutine(LootResource());
            }
        }

        public void StartSearchingLoot()
        {
            _isSearching = true;
            _isDelivering = false;
            isMovingToTarget = false;
            _navMeshAgent.destination = RandomExts.GeneratePoint(_lowPoint, _highPoint);
            lootDetector.FindLoot();
        }

        public void SetSpeed(int speed) => 
            _navMeshAgent.speed = speed;

        private void UnloadLoot()
        {
            Unload?.Invoke(_countDelivery);
            _isDelivering = false;
            StartSearchingLoot();
        }

        private IEnumerator LootResource()
        {
            yield return new WaitForSeconds(2f);
            _isDelivering = true;
            _navMeshAgent.destination = _basePosition;
        }

        private void TryToSwitchTarget(Loot loot)
        {
            if (_base.TryToLoot( loot, this)&&!isMovingToTarget&&!_isDelivering)
            {
                _isSearching = false;
                isMovingToTarget = false;
                _navMeshAgent.destination = loot.transform.position;
            }
        }

        private void OnDestroy() => 
            lootDetector.FoundLoot -= TryToSwitchTarget;
    }
}