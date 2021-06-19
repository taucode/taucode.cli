namespace TauCode.Lab.Data.Graphs.Rho
{
    public interface IRhoEdge
    {
        IRhoNode From { get; }
        IRhoNode To { get; }
        void Disappear();
    }
}