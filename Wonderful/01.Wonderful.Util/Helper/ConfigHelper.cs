using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wonderful.Util.Helper
{
    public class ConfigHelper
    {
        static ConfigHelper()
        {
            IConfiguration config = null;
            if (config == null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json");

                config = builder.Build();
            }

            _config = config;
        }
        private static IConfiguration _config { get; }

        /// <summary>
        /// 从AppSettings获取key的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return _config[key];
        }

    }
}
