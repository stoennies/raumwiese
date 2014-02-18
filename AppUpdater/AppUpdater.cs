using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.Timers;

namespace AppUpdater
{
    public class AppUpdater
    {
        public enum UpdateAvailable
        {
            No,
            Yes,
            Unknown
        }

        public class AppVersionInfo
        {
            public String version;
            public String packageUrl;
            public String name;
        }


        protected String appVersionInfoFile;
        protected String currentAppVersionString;
        protected String appId;
        protected Version currentAppVersion;
        protected AppVersionInfo newVersionInfo;
        protected Int64 pollTimeInSeconds;

        protected System.Timers.Timer pollTimer;

        public delegate void delegate_updateCheckDone(UpdateAvailable _updateAvailable, AppVersionInfo _appVersionInfo);
        public event delegate_updateCheckDone updateCheckDone;

        public AppUpdater()
        {
        }

        public void setAppVersionInfoFile(String _url)
        {
            appVersionInfoFile = _url;
        }

        public void setPollTime(Int64 _seconds)
        {
            pollTimeInSeconds = _seconds;
        }

        public void checkForUpdate(String _appId, String _currentAppVersionString)
        {
            currentAppVersionString = _currentAppVersionString;
            appId = _appId;
            currentAppVersion = new Version(currentAppVersionString);

            Thread thread = new Thread(checkForUpdateThread);
            thread.Start();

            if (pollTimeInSeconds > 0)
            {
                if (pollTimer == null)
                {
                    pollTimer = new System.Timers.Timer(pollTimeInSeconds * 1000);
                    pollTimer.Elapsed += pollTimer_ElapsedSink;
                    pollTimer.AutoReset = true;
                    pollTimer.Start();
                }
            }
        }

        void pollTimer_ElapsedSink(object sender, ElapsedEventArgs e)
        {
            this.checkForUpdate(appId, currentAppVersionString);
        }

        public virtual void applyUpdate()
        {
            if (newVersionInfo != null && !String.IsNullOrEmpty(newVersionInfo.packageUrl))
                System.Diagnostics.Process.Start(newVersionInfo.packageUrl);
        }

        protected void checkForUpdateThread()
        {
           AppVersionInfo versionInfo = this.getAppVersionInfo(appVersionInfoFile, appId);
           Version newVersion = new Version();

           if (versionInfo == null || String.IsNullOrEmpty(versionInfo.version))
           {
               if (updateCheckDone != null) updateCheckDone(UpdateAvailable.Unknown, null);
               return;
           }

           newVersion = new Version(versionInfo.version);
           if (newVersion > currentAppVersion)
           {
               newVersionInfo = versionInfo;
               if (updateCheckDone != null) updateCheckDone(UpdateAvailable.Yes, versionInfo);
           }
           else
           {
               if (updateCheckDone != null) updateCheckDone(UpdateAvailable.No, versionInfo);
           }
        }

        protected AppVersionInfo getAppVersionInfo(String _appVersionXMLUrl, String _appId)
        {
            XmlDocument document;
            XmlNode appNode, appVersionNode, appPackageUrlNode, appVersionNameNode;
            AppVersionInfo versionInfo;

            try
            {               
                document = new XmlDocument();
                document.Load(_appVersionXMLUrl);

                appNode = document.SelectSingleNode(_appId);
                if (appNode == null)
                    return null;

                appVersionNode = appNode.SelectSingleNode("version");
                appVersionNameNode = appNode.SelectSingleNode("name");
                appPackageUrlNode = appNode.SelectSingleNode("url");

                versionInfo = new AppVersionInfo();

                if (appVersionNode != null)
                    versionInfo.version = appVersionNode.InnerText;
                if (appPackageUrlNode != null)
                    versionInfo.packageUrl = appPackageUrlNode.InnerText;
                if (appVersionNameNode != null)
                    versionInfo.name = appVersionNameNode.InnerText;
     
                return versionInfo; 
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
