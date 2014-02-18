using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Web;
using System.Net;
using System.Xml;
using System.IO;

using RaumfeldNET;
using RaumfeldNET.Log;
using RaumfeldNET.UPNP;


namespace RaumfeldNET.Base
{
    public class Base
    {               
        protected void writeLog(LogType _logType, String _log, Exception _exception = null, Object _additionalObjectInfo = null)
        {
            Global.getLogWriter().writeLog(_logType, _log, _exception, _additionalObjectInfo);
        }        
    }

    public class BaseHelper : Base
    {
        protected BaseWebRequest webRequestHelper;
        protected BaseXmlDocument xmlDocumentHelper;

        public BaseHelper()
             :base()
        {
            webRequestHelper = new BaseWebRequest();
            xmlDocumentHelper = new BaseXmlDocument();
        }       
    }

    public class BaseXmlDocument : Base
    {
        public string getChildNodeValue(XmlNode _nodeItem, String _subNodeId, XmlNamespaceManager _manager = null)
        {
            XmlNode singleNode;
            singleNode = _nodeItem.SelectSingleNode(_subNodeId, _manager);
            if (singleNode != null)
                return singleNode.InnerText;
            return "";
        }

        public string getChildNodeAttributeValue(XmlNode _nodeItem, String _subNodeId, String _attributeId, XmlNamespaceManager _manager = null)
        {
            XmlNode singleNode;
            if (String.IsNullOrEmpty(_subNodeId))
                singleNode = _nodeItem;
            else
                singleNode = _nodeItem.SelectSingleNode(_subNodeId, _manager);
            if (singleNode != null && singleNode.Attributes[_attributeId] != null)
                return singleNode.Attributes[_attributeId].InnerText; ;
            return "";
        }

        public string getNodeAttributeValue(XmlNode _nodeItem, String _attributeId, XmlNamespaceManager _manager = null)
        {
            XmlNode singleNode = _nodeItem;
            if (singleNode != null && singleNode.Attributes[_attributeId] != null)
                return singleNode.Attributes[_attributeId].InnerText;
            return "";
        }
    }

    public class BaseWebRequest : Base
    {
        public Boolean denyRequestErrorLog;

        private void httpWebRequestAsyc(HttpWebRequest _request, Action<HttpWebResponse> _responseAction)
        {
            Action wrapperAction = () =>
            {
                _request.BeginGetResponse(new AsyncCallback((iar) =>
                {
                    try
                    {
                        var response = (HttpWebResponse)((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
                        if (_responseAction!=null)
                            _responseAction(response);
                    }
                    catch (Exception e)
                    {
                        if (!denyRequestErrorLog)
                            this.writeLog(LogType.Error, "Fehler bei WebRequest auf Url :" + _request.RequestUri, e);
                    }
                }), _request);
            };
            wrapperAction.BeginInvoke(new AsyncCallback((iar) =>
            {
                var action = (Action)iar.AsyncState;
                action.EndInvoke(iar);
            }), wrapperAction);
        }

        private HttpWebRequest createWebRequest(string _requestUrl, Dictionary<string, string> _postParameters = null, Dictionary<string, string> _headerParameters = null)
        {
            string postData = "";

            if (_postParameters != null)
            {
                foreach (string key in _postParameters.Keys)
                {
                    postData += HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(_postParameters[key]) + "&";
                }
            }

            String requestUrl = String.Format("{0}{1}", _requestUrl, String.IsNullOrWhiteSpace(postData) ? "" : "?" + postData);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(requestUrl);

            if (_headerParameters != null)
            {
                foreach (string key in _headerParameters.Keys)
                {
                    webRequest.Headers.Add(key, _headerParameters[key]);                    
                }
            }

            // disable searching for proxy. This will speed up the process
            webRequest.Proxy = null;

            return webRequest;
        }

        public string httpPostRequest(string _requestUrl, Dictionary<string, string> _postParameters, Dictionary<string, string> _headerParameters)
        {
            try
            {
                HttpWebRequest webRequest = this.createWebRequest(_requestUrl, _postParameters, _headerParameters);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                string pageContent = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                responseStream.Close();
                webResponse.Close();

                return pageContent;
            }
            catch (Exception _e)
            {
                this.writeLog(LogType.Error, "Fehler bei 'httpPostRequest' für url: " + _requestUrl, _e);
                return String.Empty;
            }
        }

        public void httpPostRequestAsync(string _requestUrl, Dictionary<string, string> _postParameters, Dictionary<string, string> _headerParameters, Action<HttpWebResponse> _responseAction)
        {
            try
            {
                HttpWebRequest webRequest = this.createWebRequest(_requestUrl, _postParameters, _headerParameters);
                this.httpWebRequestAsyc(webRequest, _responseAction);  
            }
            catch (Exception _e)
            {
                this.writeLog(LogType.Error, "Fehler bei 'httpPostRequestAsync' für url: " + _requestUrl, _e);
            }
        }
    }

    public class BaseUPNPAction : BaseHelper
    {
        protected RaumfeldNET.UPNP.UNPN upnpStack;

        public BaseUPNPAction(RaumfeldNET.UPNP.UNPN _upnpStack)
        {
            upnpStack = _upnpStack;
            if (upnpStack == null)
            {
                this.writeLog(LogType.Error, "UPNPStack ist nicht initialisiert!");
                // UPNP Stack not initialized is a fatal error and need programm to crash!
                throw new Exception(Global.getCrashInfo());
            }
        }
    }

    public class BaseManager : BaseUPNPAction
    {
        public BaseManager(RaumfeldNET.UPNP.UNPN _upnpStack) : base(_upnpStack) 
        {            
        }
    }
}

