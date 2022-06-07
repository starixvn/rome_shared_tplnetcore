namespace TplNetCore.Element
{
    public interface IComponent
    {
        /// <summary>
        /// Exclude external resources when the component appear in more than one place in the parent template.
        /// </summary>
        void HideLinkedResources();

        /// <summary>
        /// Render component to string.
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}
