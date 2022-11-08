using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using log4net;
using log4net.Config;
using System.Reflection;

namespace SuperHeroWebAPI.Tools
{

    public class Log
    {
        private static readonly ILog logger =
           LogManager.GetLogger(typeof(Log));

        static Log()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        //public static void WriteLine(string txt)
        //{
        //    logger.Info(txt);
        //}

        public static void WriteLine(string txt, int i = 0)
        {
            switch (i)
            {
                // Lors du debug
                case 1:
                    logger.Debug(txt);
                    break;

                // Exception ou Erreur
                case 2:
                    logger.Warn(txt);
                    break;

                // Cas ordinaire
                default:
                    logger.Info(txt);
                    break;
            }
        }
    }
}