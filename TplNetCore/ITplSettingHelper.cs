namespace TplNetCore
{

    public interface ITplSettingHelper
    {
        string PhysicalApplicationPath { get; }

        string ApplicationPath { get; }

        string ApplyAppPathModifier(string virtualPath);

        T GetSession<T>(string key);

        void SetSession<T>(string key, T value);
    }
}
