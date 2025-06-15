using Zenject;

namespace Drones
{
    public interface IState : IExitableState
    {
        public void Enter();
    }
}