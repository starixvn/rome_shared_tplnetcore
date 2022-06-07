namespace TplNetCore.Element
{
    public interface IElement
    {
        bool RegisterComponent(string name, IComponent component);
        bool RegisterComponent(IComponent component);
    }
}
