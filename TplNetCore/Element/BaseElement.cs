namespace TplNetCore.Element
{
    public abstract class BaseElement : IElement
    {
        public bool Visible = true;

        public void Hide()
        {
            Visible = false;
        }

        public void Show()
        {
            Visible = true;
        }

        #region IElement Members

        public virtual bool RegisterComponent(string name, IComponent component)
        {
            return false;
        }

        public virtual bool RegisterComponent(IComponent component)
        {
            return RegisterComponent(null, component);
        }

        #endregion
    }
}
