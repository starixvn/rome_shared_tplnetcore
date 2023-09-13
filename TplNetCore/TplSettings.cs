using System;
using System.Collections.Generic;
using System.IO;

namespace TplNetCore
{

    public class TplSettings
    {
        private static object locker = new object();
        private static Dictionary<string, string> mappingUrls = new Dictionary<string, string>();
        private static Dictionary<string, List<string>> externalUrls = new Dictionary<string, List<string>>();

        public const char Question = '?';
        internal const char Dot = '.';
        internal const string DOTJS = ".js";
        internal const string DOTCSS = ".css";
        internal const char UrlSlash = '/';
        public static string UrlVersion = string.Empty;

        public const string LanguageSuffix = "_en-US.";

        internal static bool UseCookieless = false;

        internal const string CURRENT_THEME = "TPLCORE.CURRENT_THEME";

        private ITplSettingHelper tplSettingHelper;
        private static readonly TplSettings Current = new TplSettings();

        public static void Init(ITplSettingHelper tplSettingHelper)
        {
            Current.tplSettingHelper = tplSettingHelper;
        }

        public static string CurrentTheme
        {
            get => Current.tplSettingHelper.GetSession<string>(CURRENT_THEME);
            set => Current.tplSettingHelper.SetSession(CURRENT_THEME, value);
        }

        internal static string FindUrl(string relativeUrl)
        {
            // Correct relative Url and save to memory cache
            if (!mappingUrls.ContainsKey(relativeUrl))
            {
                lock (locker)
                {
                    string matching = '*' + Path.GetExtension(relativeUrl);
                    if (!externalUrls.ContainsKey(matching))
                    {
                        // Find correct url
                        string[] files = Directory.GetFiles(Current.tplSettingHelper.PhysicalApplicationPath, matching, SearchOption.AllDirectories);
                        List<string> urls = new List<string>();
                        foreach (string file in files)
                        {
                            string url = file.Replace(Current.tplSettingHelper.PhysicalApplicationPath, "").Replace("\\", "/");
                            urls.Add(url);
                            if (url.EndsWith(relativeUrl, StringComparison.OrdinalIgnoreCase))
                            {
                                if (mappingUrls.ContainsKey(relativeUrl)) continue;
                                mappingUrls.Add(relativeUrl, url);
                            }
                        }
                        externalUrls.Add(matching, urls);
                    }
                    else
                    {
                        List<string> urls = externalUrls[matching];
                        foreach (string url in urls)
                        {
                            if (url.EndsWith(relativeUrl, StringComparison.OrdinalIgnoreCase))
                            {
                                if (mappingUrls.ContainsKey(relativeUrl)) continue;
                                mappingUrls.Add(relativeUrl, url);
                            }
                        }
                    }
                }
            }

            relativeUrl = mappingUrls[relativeUrl];
            return relativeUrl;
        }

        public static string GetBaseUrl()
        {
            string appPath = Current.tplSettingHelper.ApplicationPath;

            string url = /*Protocol + hostName +*/ appPath;

            if (appPath.Length > 1)
            {
                url += UrlSlash;
            }

            return url;
        }

        internal static string GetBaseUrl2()
        {
            string appPath = Current.tplSettingHelper.ApplyAppPathModifier(Current.tplSettingHelper.ApplicationPath);
            if (appPath[appPath.Length - 1] != UrlSlash)
            {
                appPath += UrlSlash;
            }

            return /*Protocol + hostName +*/ appPath;
        }


        internal const string DOMAIN_LIST = "TPLCORE.DOMAIN_LIST";
        internal static List<string> Domains
        {
            get => Current.tplSettingHelper.GetSession<List<string>>(DOMAIN_LIST);
            set => Current.tplSettingHelper.SetSession(DOMAIN_LIST, value);
        }
        public static void SetCookielessDomains(string[] domains)
        {
            if (domains == null || domains.Length == 0)
            {
                UseCookieless = false;
                Domains = null;
                return;
            }

            UseCookieless = true;

            LENGTH = domains.Length;
            Domains = new List<string>(LENGTH);
            Domains.AddRange(domains);
        }

        internal static int LENGTH = 3;
        internal static string GetCookielessDomain(string url)
        {
            if (Domains == null) return GetBaseUrl();
            return Domains[GetHashNumber(LENGTH, url)];
        }

        private static int GetHashNumber(int length, string url)
        {
            if (length <= 1) return 0;

            /*
             * Get last "seed" characters, convert to int, sum and modulo to "length"
             */

            int seed = 10;
            int sum = 0;
            int count = url.Length;
            if (seed > count) seed = count;
            for (int i = 1; i <= seed; i++)
            {
                sum += (int)(url[count - i]);
            }

            return sum % length;
        }

        public static string AbsoluteUrl(string relativeUrl)
        {
            return GetBaseUrl() + FindUrl(relativeUrl) + Question + UrlVersion;
        }

        /// <summary>
        /// Return absolute url from relative url, including cookieless
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static string AbsoluteUrl2(string relativeUrl)
        {
            return GetBaseUrl2() + FindUrl(relativeUrl) + Question + UrlVersion;
        }
    }
}
