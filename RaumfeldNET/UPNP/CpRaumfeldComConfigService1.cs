using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenHome.Net.Core;
using OpenHome.Net.ControlPoint;

namespace OpenHome.Net.ControlPoint.Proxies
{
    public interface ICpProxyRaumfeldComConfigService1 : ICpProxy, IDisposable
    {
        void SyncGetPublicKey(out String aKey);
        void BeginGetPublicKey(CpProxy.CallbackAsyncComplete aCallback);
        void EndGetPublicKey(IntPtr aAsyncHandle, out String aKey);
        void SyncGetRevision(out uint aRevision);
        void BeginGetRevision(CpProxy.CallbackAsyncComplete aCallback);
        void EndGetRevision(IntPtr aAsyncHandle, out uint aRevision);
        void SyncSetPreferences(String aPreferences, String aLeastCommonChangedNode, uint aExpectedRevision, String aOnConflict);
        void BeginSetPreferences(String aPreferences, String aLeastCommonChangedNode, uint aExpectedRevision, String aOnConflict, CpProxy.CallbackAsyncComplete aCallback);
        void EndSetPreferences(IntPtr aAsyncHandle);
        void SyncGetPreferences(String aPublicKey, out String aPreferences, out uint aRevision);
        void BeginGetPreferences(String aPublicKey, CpProxy.CallbackAsyncComplete aCallback);
        void EndGetPreferences(IntPtr aAsyncHandle, out String aPreferences, out uint aRevision);
        void SyncGetDevice(String aService, out String aUniqueDeviceName);
        void BeginGetDevice(String aService, CpProxy.CallbackAsyncComplete aCallback);
        void EndGetDevice(IntPtr aAsyncHandle, out String aUniqueDeviceName);
        void SetPropertyLastChangeChanged(System.Action aLastChangeChanged);
        String PropertyLastChange();
        void SetPropertyRevisionChanged(System.Action aRevisionChanged);
        uint PropertyRevision();
        void SetPropertyARG_TYPE_OnConflictChanged(System.Action aARG_TYPE_OnConflictChanged);
        String PropertyARG_TYPE_OnConflict();
    }

    internal class SyncGetPublicKeyRaumfeldComConfigService1 : SyncProxyAction
    {
        private CpProxyRaumfeldComConfigService1 iService;
        private String iKey;

        public SyncGetPublicKeyRaumfeldComConfigService1(CpProxyRaumfeldComConfigService1 aProxy)
        {
            iService = aProxy;
        }
        public String Key()
        {
            return iKey;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetPublicKey(aAsyncHandle, out iKey);
        }
    };

    internal class SyncGetRevisionRaumfeldComConfigService1 : SyncProxyAction
    {
        private CpProxyRaumfeldComConfigService1 iService;
        private uint iRevision;

        public SyncGetRevisionRaumfeldComConfigService1(CpProxyRaumfeldComConfigService1 aProxy)
        {
            iService = aProxy;
        }
        public uint Revision()
        {
            return iRevision;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetRevision(aAsyncHandle, out iRevision);
        }
    };

    internal class SyncSetPreferencesRaumfeldComConfigService1 : SyncProxyAction
    {
        private CpProxyRaumfeldComConfigService1 iService;

        public SyncSetPreferencesRaumfeldComConfigService1(CpProxyRaumfeldComConfigService1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndSetPreferences(aAsyncHandle);
        }
    };

    internal class SyncGetPreferencesRaumfeldComConfigService1 : SyncProxyAction
    {
        private CpProxyRaumfeldComConfigService1 iService;
        private String iPreferences;
        private uint iRevision;

        public SyncGetPreferencesRaumfeldComConfigService1(CpProxyRaumfeldComConfigService1 aProxy)
        {
            iService = aProxy;
        }
        public String Preferences()
        {
            return iPreferences;
        }
        public uint Revision()
        {
            return iRevision;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetPreferences(aAsyncHandle, out iPreferences, out iRevision);
        }
    };

    internal class SyncGetDeviceRaumfeldComConfigService1 : SyncProxyAction
    {
        private CpProxyRaumfeldComConfigService1 iService;
        private String iUniqueDeviceName;

        public SyncGetDeviceRaumfeldComConfigService1(CpProxyRaumfeldComConfigService1 aProxy)
        {
            iService = aProxy;
        }
        public String UniqueDeviceName()
        {
            return iUniqueDeviceName;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetDevice(aAsyncHandle, out iUniqueDeviceName);
        }
    };

    /// <summary>
    /// Proxy for the raumfeld.com:ConfigService:1 UPnP service
    /// </summary>
    public class CpProxyRaumfeldComConfigService1 : CpProxy, IDisposable, ICpProxyRaumfeldComConfigService1
    {
        private OpenHome.Net.Core.Action iActionGetPublicKey;
        private OpenHome.Net.Core.Action iActionGetRevision;
        private OpenHome.Net.Core.Action iActionSetPreferences;
        private OpenHome.Net.Core.Action iActionGetPreferences;
        private OpenHome.Net.Core.Action iActionGetDevice;
        private PropertyString iLastChange;
        private PropertyUint iRevision;
        private PropertyString iARG_TYPE_OnConflict;
        private System.Action iLastChangeChanged;
        private System.Action iRevisionChanged;
        private System.Action iARG_TYPE_OnConflictChanged;
        private Mutex iPropertyLock;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>Use CpProxy::[Un]Subscribe() to enable/disable querying of state variable and reporting of their changes.</remarks>
        /// <param name="aDevice">The device to use</param>
        public CpProxyRaumfeldComConfigService1(CpDevice aDevice)
            : base("schemas-raumfeld-com", "ConfigService", 1, aDevice)
        {
            OpenHome.Net.Core.Parameter param;
            List<String> allowedValues = new List<String>();

            iActionGetPublicKey = new OpenHome.Net.Core.Action("GetPublicKey");
            param = new ParameterString("Key", allowedValues);
            iActionGetPublicKey.AddOutputParameter(param);

            iActionGetRevision = new OpenHome.Net.Core.Action("GetRevision");
            param = new ParameterUint("Revision");
            iActionGetRevision.AddOutputParameter(param);

            iActionSetPreferences = new OpenHome.Net.Core.Action("SetPreferences");
            param = new ParameterString("Preferences", allowedValues);
            iActionSetPreferences.AddInputParameter(param);
            param = new ParameterString("LeastCommonChangedNode", allowedValues);
            iActionSetPreferences.AddInputParameter(param);
            param = new ParameterUint("ExpectedRevision");
            iActionSetPreferences.AddInputParameter(param);
            allowedValues.Add("ForceOverwrite");
            allowedValues.Add("Cancel");
            param = new ParameterString("OnConflict", allowedValues);
            iActionSetPreferences.AddInputParameter(param);
            allowedValues.Clear();

            iActionGetPreferences = new OpenHome.Net.Core.Action("GetPreferences");
            param = new ParameterString("PublicKey", allowedValues);
            iActionGetPreferences.AddInputParameter(param);
            param = new ParameterString("Preferences", allowedValues);
            iActionGetPreferences.AddOutputParameter(param);
            param = new ParameterUint("Revision");
            iActionGetPreferences.AddOutputParameter(param);

            iActionGetDevice = new OpenHome.Net.Core.Action("GetDevice");
            allowedValues.Add("meta-server");
            allowedValues.Add("renderer");
            param = new ParameterString("Service", allowedValues);
            iActionGetDevice.AddInputParameter(param);
            allowedValues.Clear();
            param = new ParameterString("UniqueDeviceName", allowedValues);
            iActionGetDevice.AddOutputParameter(param);

            iLastChange = new PropertyString("LastChange", LastChangePropertyChanged);
            AddProperty(iLastChange);
            iRevision = new PropertyUint("Revision", RevisionPropertyChanged);
            AddProperty(iRevision);
            iARG_TYPE_OnConflict = new PropertyString("ARG_TYPE_OnConflict", ARG_TYPE_OnConflictPropertyChanged);
            AddProperty(iARG_TYPE_OnConflict);
            
            iPropertyLock = new Mutex();
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aKey"></param>
        public void SyncGetPublicKey(out String aKey)
        {
            SyncGetPublicKeyRaumfeldComConfigService1 sync = new SyncGetPublicKeyRaumfeldComConfigService1(this);
            BeginGetPublicKey(sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aKey = sync.Key();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetPublicKey().</remarks>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetPublicKey(CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetPublicKey, aCallback);
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetPublicKey.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aKey"></param>
        public void EndGetPublicKey(IntPtr aAsyncHandle, out String aKey)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aKey = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aRevision"></param>
        public void SyncGetRevision(out uint aRevision)
        {
            SyncGetRevisionRaumfeldComConfigService1 sync = new SyncGetRevisionRaumfeldComConfigService1(this);
            BeginGetRevision(sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aRevision = sync.Revision();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetRevision().</remarks>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetRevision(CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetRevision, aCallback);
            int outIndex = 0;
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionGetRevision.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aRevision"></param>
        public void EndGetRevision(IntPtr aAsyncHandle, out uint aRevision)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aRevision = Invocation.OutputUint(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aPreferences"></param>
        /// <param name="aLeastCommonChangedNode"></param>
        /// <param name="aExpectedRevision"></param>
        /// <param name="aOnConflict"></param>
        public void SyncSetPreferences(String aPreferences, String aLeastCommonChangedNode, uint aExpectedRevision, String aOnConflict)
        {
            SyncSetPreferencesRaumfeldComConfigService1 sync = new SyncSetPreferencesRaumfeldComConfigService1(this);
            BeginSetPreferences(aPreferences, aLeastCommonChangedNode, aExpectedRevision, aOnConflict, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndSetPreferences().</remarks>
        /// <param name="aPreferences"></param>
        /// <param name="aLeastCommonChangedNode"></param>
        /// <param name="aExpectedRevision"></param>
        /// <param name="aOnConflict"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginSetPreferences(String aPreferences, String aLeastCommonChangedNode, uint aExpectedRevision, String aOnConflict, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionSetPreferences, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionSetPreferences.InputParameter(inIndex++), aPreferences));
            invocation.AddInput(new ArgumentString((ParameterString)iActionSetPreferences.InputParameter(inIndex++), aLeastCommonChangedNode));
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionSetPreferences.InputParameter(inIndex++), aExpectedRevision));
            invocation.AddInput(new ArgumentString((ParameterString)iActionSetPreferences.InputParameter(inIndex++), aOnConflict));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndSetPreferences(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aPublicKey"></param>
        /// <param name="aPreferences"></param>
        /// <param name="aRevision"></param>
        public void SyncGetPreferences(String aPublicKey, out String aPreferences, out uint aRevision)
        {
            SyncGetPreferencesRaumfeldComConfigService1 sync = new SyncGetPreferencesRaumfeldComConfigService1(this);
            BeginGetPreferences(aPublicKey, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aPreferences = sync.Preferences();
            aRevision = sync.Revision();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetPreferences().</remarks>
        /// <param name="aPublicKey"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetPreferences(String aPublicKey, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetPreferences, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionGetPreferences.InputParameter(inIndex++), aPublicKey));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetPreferences.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionGetPreferences.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aPreferences"></param>
        /// <param name="aRevision"></param>
        public void EndGetPreferences(IntPtr aAsyncHandle, out String aPreferences, out uint aRevision)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aPreferences = Invocation.OutputString(aAsyncHandle, index++);
            aRevision = Invocation.OutputUint(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aService"></param>
        /// <param name="aUniqueDeviceName"></param>
        public void SyncGetDevice(String aService, out String aUniqueDeviceName)
        {
            SyncGetDeviceRaumfeldComConfigService1 sync = new SyncGetDeviceRaumfeldComConfigService1(this);
            BeginGetDevice(aService, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aUniqueDeviceName = sync.UniqueDeviceName();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetDevice().</remarks>
        /// <param name="aService"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetDevice(String aService, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetDevice, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionGetDevice.InputParameter(inIndex++), aService));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetDevice.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aUniqueDeviceName"></param>
        public void EndGetDevice(IntPtr aAsyncHandle, out String aUniqueDeviceName)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aUniqueDeviceName = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Set a delegate to be run when the LastChange state variable changes.
        /// </summary>
        /// <remarks>Callbacks may be run in different threads but callbacks for a
        /// CpProxyRaumfeldComConfigService1 instance will not overlap.</remarks>
        /// <param name="aLastChangeChanged">The delegate to run when the state variable changes</param>
        public void SetPropertyLastChangeChanged(System.Action aLastChangeChanged)
        {
            lock (iPropertyLock)
            {
                iLastChangeChanged = aLastChangeChanged;
            }
        }

        private void LastChangePropertyChanged()
        {
            lock (iPropertyLock)
            {
                ReportEvent(iLastChangeChanged);
            }
        }

        /// <summary>
        /// Set a delegate to be run when the Revision state variable changes.
        /// </summary>
        /// <remarks>Callbacks may be run in different threads but callbacks for a
        /// CpProxyRaumfeldComConfigService1 instance will not overlap.</remarks>
        /// <param name="aRevisionChanged">The delegate to run when the state variable changes</param>
        public void SetPropertyRevisionChanged(System.Action aRevisionChanged)
        {
            lock (iPropertyLock)
            {
                iRevisionChanged = aRevisionChanged;
            }
        }

        private void RevisionPropertyChanged()
        {
            lock (iPropertyLock)
            {
                ReportEvent(iRevisionChanged);
            }
        }

        /// <summary>
        /// Set a delegate to be run when the ARG_TYPE_OnConflict state variable changes.
        /// </summary>
        /// <remarks>Callbacks may be run in different threads but callbacks for a
        /// CpProxyRaumfeldComConfigService1 instance will not overlap.</remarks>
        /// <param name="aARG_TYPE_OnConflictChanged">The delegate to run when the state variable changes</param>
        public void SetPropertyARG_TYPE_OnConflictChanged(System.Action aARG_TYPE_OnConflictChanged)
        {
            lock (iPropertyLock)
            {
                iARG_TYPE_OnConflictChanged = aARG_TYPE_OnConflictChanged;
            }
        }

        private void ARG_TYPE_OnConflictPropertyChanged()
        {
            lock (iPropertyLock)
            {
                ReportEvent(iARG_TYPE_OnConflictChanged);
            }
        }

        /// <summary>
        /// Query the value of the LastChange property.
        /// </summary>
        /// <remarks>This function is threadsafe and can only be called if Subscribe() has been
        /// called and a first eventing callback received more recently than any call
        /// to Unsubscribe().</remarks>
        /// <returns>Value of the LastChange property</returns>
        public String PropertyLastChange()
        {
         
            
                   PropertyReadLock();
                String val = iLastChange.Value();
                PropertyReadUnlock();
                return val;
        
   
            
        }

        /// <summary>
        /// Query the value of the Revision property.
        /// </summary>
        /// <remarks>This function is threadsafe and can only be called if Subscribe() has been
        /// called and a first eventing callback received more recently than any call
        /// to Unsubscribe().</remarks>
        /// <returns>Value of the Revision property</returns>
        public uint PropertyRevision()
        {
            PropertyReadLock();
            uint val = iRevision.Value();
            PropertyReadUnlock();
            return val;
        }

        /// <summary>
        /// Query the value of the ARG_TYPE_OnConflict property.
        /// </summary>
        /// <remarks>This function is threadsafe and can only be called if Subscribe() has been
        /// called and a first eventing callback received more recently than any call
        /// to Unsubscribe().</remarks>
        /// <returns>Value of the ARG_TYPE_OnConflict property</returns>
        public String PropertyARG_TYPE_OnConflict()
        {
            PropertyReadLock();
            String val = iARG_TYPE_OnConflict.Value();
            PropertyReadUnlock();
            return val;
        }

        /// <summary>
        /// Must be called for each class instance.  Must be called before Core.Library.Close().
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (iHandle == IntPtr.Zero)
                    return;
                DisposeProxy();
                iHandle = IntPtr.Zero;
            }
            iActionGetPublicKey.Dispose();
            iActionGetRevision.Dispose();
            iActionSetPreferences.Dispose();
            iActionGetPreferences.Dispose();
            iActionGetDevice.Dispose();
            iLastChange.Dispose();
            iRevision.Dispose();
            iARG_TYPE_OnConflict.Dispose();
        }
    }
}

