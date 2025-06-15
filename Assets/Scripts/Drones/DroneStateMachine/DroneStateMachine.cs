using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Drones
{
    public class DroneStateMachine : IDroneStateMachine
    {
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public DroneStateMachine(Drone drone, NavMeshAgent navMeshAgent, float range, Vector3 lowPoint,
            Vector3 highPoint, BaseStation baseStation, Vector3 basePosition, LootDetector lootDetector)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(SearchingState)] = new SearchingState(drone.transform, navMeshAgent, range, lowPoint, highPoint, lootDetector),
                [typeof(HuntingState)] = new HuntingState(navMeshAgent, baseStation, drone, drone.transform, range, this),
                [typeof(LootingState)] = new LootingState(drone),
                [typeof(DeliveringState)] = new DeliveringState(drone, navMeshAgent, this, basePosition, range),
                
            };
        }
    
        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }
        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        public void Tick() => 
            _activeState?.Tick();

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
      
            TState state = GetState<TState>();
            _activeState = state;
      
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;

        public IExitableState GetActiveState() => 
            _activeState;
    }
}