namespace Physicist.Types.Interfaces
{
    public interface IGUIElement : IUpdate, IDraw, IName
    {
        object Parent { get; set; }

        void UnloadContent();
    }
}
