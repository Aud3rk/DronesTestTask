namespace Drones
{
    public interface IDroneStateMachine
    {
        void Enter<TState>() where TState : class, IState;
    }
}