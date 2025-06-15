namespace Drones
{
    public interface IExitableState
    {
        void Exit();
        void Tick();
    }
}