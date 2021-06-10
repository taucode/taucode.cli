namespace TauCode.Lab.Data.Graphs
{
    public interface IEdge<T>
    {
        INode<T> From { get; }
        INode<T> To { get; }
        void Disappear();
    }
}
