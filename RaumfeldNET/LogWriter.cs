using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using RaumfeldNET;

namespace RaumfeldNET.Log
{

    public enum LogType
    {
        Info = 0,
        Warning = 1,
        Error = 2,
        Always = 99
    }
    
    public class LogWriter
    {
        private String logFilePath;
        private String logFileName;
        private String LogFileNameException;
        private String LogFileNameAdditionalObject;
        private LogType logTypeLogLevel;
        private StreamWriter logFileWriter;
        private uint exceptionCounter;
        private uint logCounter;
        
        public LogWriter()
        {
            logFileName = "raumfeldNET.log";
            LogFileNameException = "exception.log";
            LogFileNameAdditionalObject = "additional.log";
        }

        ~LogWriter()
        {                        
        }
       
        public void setLogFilePath(String _logFilePath)
        {
            logFilePath = _logFilePath;
        }

        public void setLogLevel(LogType _logTypeLevel)
        {
            logTypeLogLevel = _logTypeLevel;
        }

        protected Boolean isLogTypeLogged(LogType _logType)
        {
            if (_logType >= logTypeLogLevel)
                return true;
            return false;
        }

        protected String buildLogFilePathName()
        {
            if (!String.IsNullOrWhiteSpace(logFilePath) && !logFilePath.EndsWith(@"\"))
                logFilePath += @"\";
            return logFilePath + logFileName;
        }

        protected String buildExceptionLogFilePathName()
        {
            if (!String.IsNullOrWhiteSpace(logFilePath) && !logFilePath.EndsWith(@"\"))
                logFilePath += @"\";
            return String.Format("{0}{1}{2}{3}", logFilePath, Path.GetFileNameWithoutExtension(LogFileNameException), logCounter, Path.GetExtension(LogFileNameException));
        }

        protected String buildAdditionalObjectLogFilePathName()
        {
            if (!String.IsNullOrWhiteSpace(logFilePath) && !logFilePath.EndsWith(@"\"))
                logFilePath += @"\";
            return String.Format("{0}{1}{2}{3}", logFilePath, Path.GetFileNameWithoutExtension(LogFileNameAdditionalObject), logCounter, Path.GetExtension(LogFileNameException));
        }

        protected void writeExceptionLog(Exception _e)
        {
            StreamWriter exceptionLogWriter;

            if (_e == null)
                return;

            exceptionCounter++;

            exceptionLogWriter = new StreamWriter(this.buildExceptionLogFilePathName());
           
            exceptionLogWriter.WriteLine("#Source >");
            exceptionLogWriter.WriteLine(_e.Source);
            exceptionLogWriter.WriteLine("#Message >");
            exceptionLogWriter.WriteLine(_e.Message);
            exceptionLogWriter.WriteLine("#StackTrace >");
            exceptionLogWriter.WriteLine(_e.StackTrace);
            exceptionLogWriter.WriteLine("#InnerException >");
            exceptionLogWriter.WriteLine(_e.ToString());

            exceptionLogWriter.Close();
        }

        protected void writeAdditionalObjectLog(Object _additionalObject)
        {
            StreamWriter exceptionLogWriter;

            if (_additionalObject == null)
                return;

            exceptionCounter++;

            exceptionLogWriter = new StreamWriter(this.buildAdditionalObjectLogFilePathName());

            exceptionLogWriter.WriteLine(_additionalObject.ToString());

            exceptionLogWriter.Close();
        }

        protected void writeSystemInformation(StreamWriter _streamWriter)
        {
            _streamWriter.WriteLine(String.Format("OS: {0}", System.Environment.OSVersion));
            _streamWriter.WriteLine(String.Format("NET Version: {0}", System.Environment.Version));
            _streamWriter.WriteLine(String.Format("Anzahl Prozessoren: {0}", System.Environment.ProcessorCount));  
            _streamWriter.WriteLine(String.Format("64Bit System: {0}", System.Environment.Is64BitOperatingSystem));
            _streamWriter.WriteLine(String.Format("64Bit Prozess: {0}", System.Environment.Is64BitProcess));            
            _streamWriter.WriteLine("");
        }

        public void writeLog(LogType _logType, String _log, Exception _exception = null, Object _additionalInfoObject = null)
        {
            try
            {

                if (!isLogTypeLogged(_logType))
                    return;

                if (logFileWriter == null)
                {
                    logFileWriter = new StreamWriter(this.buildLogFilePathName());
                    if (logFileWriter == null)
                        return;
                    lock (logFileWriter)
                    {
                        this.writeSystemInformation(logFileWriter);
                    }
                }

                lock (logFileWriter)
                {


                    logCounter++;

                    logFileWriter.WriteLine(String.Format("{3,-5} {0:d} {0:t}:{0:ss}.{0:ffffff}   {1,-12} {2}",
                                            System.DateTime.Now,
                                            String.Format("{0:G}", _logType),
                                            _log,
                                            logCounter
                                            ));
                    if (_exception != null)
                    {
                        this.writeExceptionLog(_exception);
                        logFileWriter.WriteLine(String.Format("{1,105} '{0}'",
                                                this.buildExceptionLogFilePathName(),
                                                "Weitere Informationen zu diesem Fehler finden sie im File"));
                    }

                    if (_additionalInfoObject != null)
                    {
                        this.writeAdditionalObjectLog(_additionalInfoObject);
                        logFileWriter.WriteLine(String.Format("{1,86} '{0}'",
                                                this.buildAdditionalObjectLogFilePathName(),
                                                "Zusatzinformationen finden sie im File"));
                    }

                    logFileWriter.Flush();
                }

            }
            catch (Exception logWriteException)
            {
                // do not throw any exception...
                //throw new Exception("Fehler beim Schreiben des LogFiles!", logWriteException);
            }
           
        }
    }
}
