using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RaumfeldNET.Base;
using RaumfeldNET.Log;
using RaumfeldNET.UPNP;


namespace RaumfeldNET
{
    public class ConfigManager : BaseManager
    {
        // we can only handle one config service (and this has to be a Raumfeld-ConfigService)
        CpConfigService configService;


        const String raumfeldConfigServiceDeviceName = "Raumfeld ConfigDevice";

        public delegate void delegate_OnConfigServiceFound();
        public event delegate_OnConfigServiceFound configServiceFound;

        public delegate void delegate_OnConfigServiceRemoved();
        public event delegate_OnConfigServiceRemoved configServiceRemoved;

        public delegate void delegate_OnZoneSetupChanged();
        public event delegate_OnZoneSetupChanged zoneSetupChanged;
        

        public ConfigManager(UPNP.UNPN _upnpStack)
            : base(_upnpStack)
        {
        }

        public void findConfigService()
        {
            upnpStack.onConfigServiceFound += configServiceFoundSink;
            upnpStack.onConfigServiceRemoved += configServiceRemovedSink;
            upnpStack.findConfigService();
        }

        protected void configServiceFoundSink(CpConfigService _configService)
        {
            this.writeLog(LogType.Info, String.Format("ConfigService'{0}' gefunden", _configService.FriendlyName));

            if (_configService.FriendlyName != raumfeldConfigServiceDeviceName)
                return;
            
            configService = _configService;
            configService.onZoneSetupChanged += configService_onZoneSetupChanged;
            if (configServiceFound != null) configServiceFound();
        }

        protected void configServiceRemovedSink(CpConfigService _configService)
        {
            this.writeLog(LogType.Info, String.Format("ConfigService'{0}' verloren", _configService.FriendlyName));

            if (_configService.FriendlyName != raumfeldConfigServiceDeviceName)
                return;
            
            configService = null;
            if (configServiceRemoved != null) configServiceRemoved();
        }

        protected void configService_onZoneSetupChanged(CpConfigService _configService)
        {
            if (zoneSetupChanged != null) zoneSetupChanged();
        }
    }
}
