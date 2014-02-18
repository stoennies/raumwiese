using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RaumfeldNET;

namespace RaumfeldNET_TestProject
{
    public partial class Form1 : Form
    {

        RaumfeldNET.Controller rfController;

        public Form1()
        {
            InitializeComponent();
            this.FormClosed += Form1_FormClosed;
            rfController = new Controller();
        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            rfController = null;
        }

        private void LogWriterTestButton_Click(object sender, EventArgs e)
        {

            RaumfeldNET.Global.getLogWriter().setLogLevel(RaumfeldNET.Log.LogType.Info);
            RaumfeldNET.Global.getLogWriter().writeLog(RaumfeldNET.Log.LogType.Error, "Fehler", new Exception("Exception"));
            RaumfeldNET.Global.getLogWriter().writeLog(RaumfeldNET.Log.LogType.Warning, "Warnung");
            RaumfeldNET.Global.getLogWriter().writeLog(RaumfeldNET.Log.LogType.Warning, "Warnung", null, "Additional Info");
            RaumfeldNET.Global.getLogWriter().writeLog(RaumfeldNET.Log.LogType.Info, "Info");            

        }

        private void RetrieveZones_Click(object sender, EventArgs e)
        {
            rfController.zoneManager.retrieveZones();
        }

        private void InitController_Click(object sender, EventArgs e)
        {            
            rfController.init();
        }
    }
}
