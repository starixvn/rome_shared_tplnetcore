using System.Threading;

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

        protected static string AbsoluteUrl(string relativeUrl)
        {
            return AbsoluteUrl(relativeUrl, true);
        }

        protected static string AbsoluteUrl(string relativeUrl, bool autoLanguage)
        {
            relativeUrl = TplSettings.FindUrl(relativeUrl);

            relativeUrl = AddThemeName(relativeUrl);

            if (autoLanguage)
            {
                string currentLanguageSuffix = "_" + Thread.CurrentThread.CurrentUICulture.Name + ".";
                relativeUrl = relativeUrl.Replace(TplSettings.LanguageSuffix, currentLanguageSuffix);
            }

            if (relativeUrl.Contains(TplSettings.DOTJS) || !TplSettings.UseCookieless)
            {
                return TplSettings.GetBaseUrl() + relativeUrl;
            }

            return TplSettings.GetCookielessDomain(relativeUrl) + relativeUrl;
        }

        protected static string AbsoluteUrl(string relativeUrl, char nextMetaChar)
        {
            relativeUrl = TplSettings.FindUrl(relativeUrl);

            string version = string.IsNullOrEmpty(TplSettings.UrlVersion) ? string.Empty : (nextMetaChar + TplSettings.UrlVersion);
            return AbsoluteUrl(relativeUrl) + version;
        }

        protected static string AbsoluteUrl2(string relativeUrl)
        {
            relativeUrl = TplSettings.FindUrl(relativeUrl);

            return TplSettings.GetBaseUrl2() + relativeUrl;
        }

        protected static string AbsoluteUrl2(string relativeUrl, char nextMetaChar)
        {
            relativeUrl = TplSettings.FindUrl(relativeUrl);

            string version = string.IsNullOrEmpty(TplSettings.UrlVersion) ? string.Empty : (nextMetaChar + TplSettings.UrlVersion);
            return TplSettings.GetBaseUrl2() + relativeUrl + version;
        }

        private static string AddThemeName(string relativeUrl)
        {
            return relativeUrl.Replace(TplSettings.DOTCSS,
                (string.IsNullOrEmpty(TplSettings.CurrentTheme) ? null : (TplSettings.Dot + TplSettings.CurrentTheme))
                + TplSettings.DOTCSS);
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
