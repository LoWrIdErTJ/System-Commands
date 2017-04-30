using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using UBotPlugin;
using System.Linq;
using System.Windows;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Security.Cryptography;
using System.Configuration;
using System.Media;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Net;
using System.Management;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Reflection;
using System.Data.OleDb;

namespace CSVtoHTML
{

    // API KEY HERE
    public class PluginInfo
    {
        public static string HashCode { get { return "db04eda1fd4198233e4aeb4fb6cfe41e983056c1"; } }
    }


    // ---------------------------------------------------------------------------------------------------------- //
    //
    // ---------------------------------               COMMANDS               ----------------------------------- //
    //
    // ---------------------------------------------------------------------------------------------------------- //

    //
    //
    // LOG TO FILE DATE TIME
    //
    //
    public class LogToFileDate : IUBotCommand
    {

        private List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();

        public LogToFileDate()
        {
            _parameters.Add(new UBotParameterDefinition("Header text (optional)", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Text to log", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Save to file.txt", UBotType.String));
        }

        public string Category
        {
            get { return "System Commands"; }
        }

        public string CommandName
        {
            get { return "log to file DateTime"; }
        }


        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {
            string headerinfo = parameters["Header text (optional)"];
            string textToLog = parameters["Text to log"];
            string savetoPath = parameters["Save to file.txt"];
            string path = savetoPath;

            if (headerinfo != null)
            {
                if (!File.Exists(path))
                {
                    if (headerinfo != null)
                    {
                        // Create a file to write to. 
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine(headerinfo);
                        }
                    }
                }
            }

                    // This text is always added, making the file longer over time 
                    // if it is not deleted. 
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        DateTime now = DateTime.Now;

                        sw.WriteLine(now + " : " + textToLog);

                    }

                    // Open the file to read from. 
                    using (StreamReader sr = File.OpenText(path))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(s);
                        }
                    }

        }

        public bool IsContainer
        {
            get { return false; }
        }

        public IEnumerable<UBotParameterDefinition> ParameterDefinitions
        {
            get { return _parameters; }
        }

        public UBotVersion UBotVersion
        {
            get { return UBotVersion.Standard; }
        }
    }

    //
    //
    // LOG TO FILE NO DATE TIME
    //
    //
    public class LogToFile : IUBotCommand
    {

        private List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();

        public LogToFile()
        {
            _parameters.Add(new UBotParameterDefinition("Header text (optional)", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Text to log", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Save to file.txt", UBotType.String));
        }

        public string Category
        {
            get { return "System Commands"; }
        }

        public string CommandName
        {
            get { return "log to file"; }
        }


        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {
            string headerinfo = parameters["Header text (optional)"];
            string textToLog = parameters["Text to log"];
            string savetoPath = parameters["Save to file.txt"];
            string path = savetoPath;

            if (headerinfo != null)
            {
                if (!File.Exists(path))
                {
                    if (headerinfo != null)
                    {
                        // Create a file to write to. 
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine(headerinfo);
                        }
                    }
                }
            }

            // This text is always added, making the file longer over time 
            // if it is not deleted. 
            using (StreamWriter sw = File.AppendText(path))
            {
                DateTime now = DateTime.Now;

                sw.WriteLine(textToLog);

            }

            // Open the file to read from. 
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }

        }

        public bool IsContainer
        {
            get { return false; }
        }

        public IEnumerable<UBotParameterDefinition> ParameterDefinitions
        {
            get { return _parameters; }
        }

        public UBotVersion UBotVersion
        {
            get { return UBotVersion.Standard; }
        }
    }

    // ---------------------------------------------------------------------------------------------------------- //
    //
    // ---------------------------------               FUNCTIONS              ----------------------------------- //
    //
    // ---------------------------------------------------------------------------------------------------------- //




    //
    //
    // Detailed Prompt
    //
    //
    public class DetailedPrompt : IUBotFunction
    {

        private List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();
        private string _returnValue;

        public DetailedPrompt()
        {
            _parameters.Add(new UBotParameterDefinition("Prompt Height", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Prompt Width", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Text Label", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Label Height", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Caption", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Default Value", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Button Submit Text", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Button Cancel Text", UBotType.String));

        }

        public string Category
        {
            get { return "System Functions"; }
        }

        public string FunctionName
        {
            get { return "$detailed prompt"; }
        }


        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {

            string FormHeight = parameters["Prompt Height"];
            int fheight = Convert.ToInt32(FormHeight);
            string FormWidth = parameters["Prompt Width"];
            int fwidth = Convert.ToInt32(FormWidth); 
            string TextLabel = parameters["Text Label"];
            string LabelHeight = parameters["Label Height"];
            int    LabelHeightInt = Convert.ToInt32(LabelHeight);
            string Caption = parameters["Caption"];
            string DefaultValue = parameters["Default Value"];
            string ButtonText = parameters["Button Submit Text"];
            string ButtonCancelText = parameters["Button Cancel Text"];

            string promptValue = Prompt.ShowDialog(TextLabel, Caption, DefaultValue, ButtonText, LabelHeightInt, fheight, fwidth, ButtonCancelText);

            _returnValue = promptValue;
        }

        public object ReturnValue
        {
            // We return our variable _returnValue as the result of the function.
            get { return _returnValue; }
        }

        public UBotType ReturnValueType
        {
            // Our result is text, so the return value type is String.
            get { return UBotType.String; }
        }


        public bool IsContainer
        {
            get { return false; }
        }

        public IEnumerable<UBotParameterDefinition> ParameterDefinitions
        {
            get { return _parameters; }
        }

        public IEnumerable<string> Options
        {
            get;
            set;
        }

        public UBotVersion UBotVersion
        {
            get { return UBotVersion.Standard; }
        }
    }



    //
    //
    // GET PC INFO
    //
    //
    public class PCInfo : IUBotFunction
    {

        private List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();
        private string _returnValue;

        public PCInfo()
        {
            var xParameter = new UBotParameterDefinition("PC Info for?", UBotType.String);
            xParameter.Options = new[] { "Username", "User DomainName", "System Directory", "OS Version", "Machine Name", "Current Directory", "OS Platform", "Processor Count", "Processor ID", "HD Model", "HD Manufacturer", "HD Signature", "HD TotalHeads", "HDD Serial Num", "BIOS Maker", "Physical Memory", "CPU Speed", "CPU Maker", "Program Files Path 32/64 Bit" };
            _parameters.Add(xParameter);

        }

        public string Category
        {
            get { return "System Functions"; }
        }

        public string FunctionName
        {
            get { return "$get system info"; }
        }


        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {

            string ckpcinfo = parameters["PC Info for?"];
            string sData = string.Empty;
            
            if(ckpcinfo == "Username"){
                
                //Get Current User Name of Windows Operating System
                sData = Environment.UserName;
                _returnValue = sData;

            }
            else if(ckpcinfo == "User DomainName"){
                
                //Get Domain Name of the System
                sData = Environment.UserDomainName;
                _returnValue = sData;

            }
            else if(ckpcinfo == "System Directory"){
                
                //Get System Directory
                sData = Environment.SystemDirectory;
                _returnValue = sData;

            }
            else if(ckpcinfo == "OS Version"){

                //Get Version of Windows Operating System
                sData = Environment.OSVersion.VersionString;
                _returnValue = sData;

            }
            else if(ckpcinfo == "Machine Name"){
                
                //Get NetBIOS Name of Local System
                sData = Environment.MachineName;
                _returnValue = sData;

            }
            else if(ckpcinfo == "Current Directory"){
                
                //Get Current Working Directory(Folder)
                sData = Environment.CurrentDirectory;
                _returnValue = sData;

            }
            else if(ckpcinfo == "OS Platform"){
                
                //Get Current Platform of Windows Operating System
                sData = Environment.OSVersion.Platform.ToString();
                _returnValue = sData;

            }
            else if (ckpcinfo == "Processor Count")
            {

                //Get No.of Processor of Current Machine
                int n = Environment.ProcessorCount;
                _returnValue = n.ToString();

            }
            else if (ckpcinfo == "Processor ID")
            {

                string cpuInfo = string.Empty;
                ManagementClass mc = new ManagementClass("win32_processor");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    if (cpuInfo == "")
                    {
                        //Get only the first CPU's ID
                        cpuInfo = mo.Properties["processorID"].Value.ToString();
                        break;
                    }
                }

                _returnValue = cpuInfo.ToString();

            }
            else if (ckpcinfo == "HD Model")
            {

                string modelNo = identifier("Win32_DiskDrive", "Model");

                _returnValue = modelNo.ToString();

            }
            else if (ckpcinfo == "HD Manufacturer")
            {

                string manufatureID = identifier("Win32_DiskDrive", "Manufacturer");

                _returnValue = manufatureID.ToString();

            }
            else if (ckpcinfo == "HD Signature")
            {

                string signature = identifier("Win32_DiskDrive", "Signature");

                _returnValue = signature.ToString();

            }
            else if (ckpcinfo == "HD TotalHeads")
            {

                string totalHeads = identifier("Win32_DiskDrive", "TotalHeads");

                _returnValue = totalHeads.ToString();

            }
            else if (ckpcinfo == "HDD Serial Num") 
            {

                string hdserial = GetHDDSerialNo();

                _returnValue = hdserial.ToString();

            }
            else if (ckpcinfo == "BIOS Maker") 
            {

                string hdserial = GetBIOSmaker();

                _returnValue = hdserial.ToString();

            }
            else if (ckpcinfo == "Physical Memory") 
            {

                string hdserial = GetPhysicalMemory();

                _returnValue = hdserial.ToString();

            }
            else if (ckpcinfo == "CPU Speed") 
            {

                string hdserial = GetCpuSpeedInGHz().ToString();

                _returnValue = hdserial.ToString();

            }
            else if (ckpcinfo == "CPU Maker")
            {

                string hdserial = GetCPUManufacturer();

                _returnValue = hdserial.ToString();

            }
            else if (ckpcinfo == "Program Files Path 32/64 Bit")
            {

                string get_programs_folder = Environment.GetEnvironmentVariable("PROGRAMFILES(X86)") ?? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

                _returnValue = get_programs_folder.ToString();

            }
                
            else { }
            
        }

        public string GetHDDSerial()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                // get the hardware serial no.
                if (wmi_HD["SerialNumber"] != null)
                    return wmi_HD["SerialNumber"].ToString();
            }

            return string.Empty;
        }

        private string identifier(string wmiClass, string wmiProperty)
        //Return a hardware identifier
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                //Only get the first one
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            return result;
        }

        public static String GetProcessorId()
        {

            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            String Id = String.Empty;
            foreach (ManagementObject mo in moc)
            {

                Id = mo.Properties["processorID"].Value.ToString();
                break;
            }
            return Id;

        }
        /// <summary>
        /// Retrieving HDD Serial No.
        /// </summary>
        /// <returns></returns>
        public static String GetHDDSerialNo()
        {
            ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection mcol = mangnmt.GetInstances();
            string result = "";
            foreach (ManagementObject strt in mcol)
            {
                result += Convert.ToString(strt["VolumeSerialNumber"]);
            }
            return result;
        }
        /// <summary>
        /// Retrieving System MAC Address.
        /// </summary>
        /// <returns></returns>
        public static string GetMACAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string MACAddress = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                if (MACAddress == String.Empty)
                {
                    if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
                }
                mo.Dispose();
            }

            MACAddress = MACAddress.Replace(":", "");
            return MACAddress;
        }
        /// <summary>
        /// Retrieving Motherboard Product Id.
        /// </summary>
        /// <returns></returns>
        public static string GetBoardProductId()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Product").ToString();

                }

                catch { }

            }

            return "Product: Unknown";

        }
        /// <summary>
        /// Retrieving CD-DVD Drive Path.
        /// </summary>
        /// <returns></returns>
        public static string GetCdRomDrive()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Drive").ToString();

                }

                catch { }

            }

            return "CD ROM Drive Letter: Unknown";

        }
        /// <summary>
        /// Retrieving BIOS Maker.
        /// </summary>
        /// <returns></returns>
        public static string GetBIOSmaker()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Manufacturer").ToString();

                }

                catch { }

            }

            return "BIOS Maker: Unknown";

        }
        /// <summary>
        /// Retrieving BIOS Serial No.
        /// </summary>
        /// <returns></returns>
        public static string GetBIOSserNo()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("SerialNumber").ToString();

                }

                catch { }

            }

            return "BIOS Serial Number: Unknown";

        }
        /// <summary>
        /// Retrieving BIOS Caption.
        /// </summary>
        /// <returns></returns>
        public static string GetBIOScaption()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Caption").ToString();

                }
                catch { }
            }
            return "BIOS Caption: Unknown";
        }
        /// <summary>
        /// Retrieving System Account Name.
        /// </summary>
        /// <returns></returns>
        public static string GetAccountName()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {

                    return wmi.GetPropertyValue("Name").ToString();
                }
                catch { }
            }
            return "User Account Name: Unknown";

        }
        /// <summary>
        /// Retrieving Physical Ram Memory.
        /// </summary>
        /// <returns></returns>
        public static string GetPhysicalMemory()
        {
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
            ManagementObjectCollection oCollection = oSearcher.Get();

            long MemSize = 0;
            long mCap = 0;

            // In case more than one Memory sticks are installed
            foreach (ManagementObject obj in oCollection)
            {
                mCap = Convert.ToInt64(obj["Capacity"]);
                MemSize += mCap;
            }
            MemSize = (MemSize / 1024) / 1024;
            return MemSize.ToString() + "MB";
        }
        /// <summary>
        /// Retrieving No of Ram Slot on Motherboard.
        /// </summary>
        /// <returns></returns>
        public static string GetNoRamSlots()
        {

            int MemSlots = 0;
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
            ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(oMs, oQuery2);
            ManagementObjectCollection oCollection2 = oSearcher2.Get();
            foreach (ManagementObject obj in oCollection2)
            {
                MemSlots = Convert.ToInt32(obj["MemoryDevices"]);

            }
            return MemSlots.ToString();
        }
        //Get CPU Temprature.
        /// <summary>
        /// method for retrieving the CPU Manufacturer
        /// using the WMI class
        /// </summary>
        /// <returns>CPU Manufacturer</returns>
        public static string GetCPUManufacturer()
        {
            string cpuMan = String.Empty;
            //create an instance of the Managemnet class with the
            //Win32_Processor class
            ManagementClass mgmt = new ManagementClass("Win32_Processor");
            //create a ManagementObjectCollection to loop through
            ManagementObjectCollection objCol = mgmt.GetInstances();
            //start our loop for all processors found
            foreach (ManagementObject obj in objCol)
            {
                if (cpuMan == String.Empty)
                {
                    // only return manufacturer from first CPU
                    cpuMan = obj.Properties["Manufacturer"].Value.ToString();
                }
            }
            return cpuMan;
        }

        /// <summary>
        /// method to retrieve the CPU's current
        /// clock speed using the WMI class
        /// </summary>
        /// <returns>Clock speed</returns>
        public static int GetCPUCurrentClockSpeed()
        {
            int cpuClockSpeed = 0;
            //create an instance of the Managemnet class with the
            //Win32_Processor class
            ManagementClass mgmt = new ManagementClass("Win32_Processor");
            //create a ManagementObjectCollection to loop through
            ManagementObjectCollection objCol = mgmt.GetInstances();
            //start our loop for all processors found
            foreach (ManagementObject obj in objCol)
            {
                if (cpuClockSpeed == 0)
                {
                    // only return cpuStatus from first CPU
                    cpuClockSpeed = Convert.ToInt32(obj.Properties["CurrentClockSpeed"].Value.ToString());
                }
            }
            //return the status
            return cpuClockSpeed;
        }
        /// <summary>
        /// method to retrieve the network adapters
        /// default IP gateway using WMI
        /// </summary>
        /// <returns>adapters default IP gateway</returns>
        public static string GetDefaultIPGateway()
        {
            //create out management class object using the
            //Win32_NetworkAdapterConfiguration class to get the attributes
            //of the network adapter
            ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");
            //create our ManagementObjectCollection to get the attributes with
            ManagementObjectCollection objCol = mgmt.GetInstances();
            string gateway = String.Empty;
            //loop through all the objects we find
            foreach (ManagementObject obj in objCol)
            {
                if (gateway == String.Empty)  // only return MAC Address from first card
                {
                    //grab the value from the first network adapter we find
                    //you can change the string to an array and get all
                    //network adapters found as well
                    //check to see if the adapter's IPEnabled
                    //equals true
                    if ((bool)obj["IPEnabled"] == true)
                    {
                        gateway = obj["DefaultIPGateway"].ToString();
                    }
                }
                //dispose of our object
                obj.Dispose();
            }
            //replace the ":" with an empty space, this could also
            //be removed if you wish
            gateway = gateway.Replace(":", "");
            //return the mac address
            return gateway;
        }
        /// <summary>
        /// Retrieve CPU Speed.
        /// </summary>
        /// <returns></returns>

        public static double? GetCpuSpeedInGHz()
        {
            double? GHz = null;
            using (ManagementClass mc = new ManagementClass("Win32_Processor"))
            {
                foreach (ManagementObject mo in mc.GetInstances())
                {
                    GHz = 0.001 * (UInt32)mo.Properties["CurrentClockSpeed"].Value;
                    break;
                }
            }
            return GHz;
        }

        public object ReturnValue
        {
            // We return our variable _returnValue as the result of the function.
            get { return _returnValue; }
        }

        public UBotType ReturnValueType
        {
            // Our result is text, so the return value type is String.
            get { return UBotType.String; }
        }


        public bool IsContainer
        {
            get { return false; }
        }

        public IEnumerable<UBotParameterDefinition> ParameterDefinitions
        {
            get { return _parameters; }
        }

        public IEnumerable<string> Options
        {
            get;
            set;
        }

        public UBotVersion UBotVersion
        {
            get { return UBotVersion.Standard; }
        }
    }


    //
    //
    // GET FILE INFO
    //
    //
    public class FileInfoFunct : IUBotFunction
    {

        private List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();
        private string _returnValue;

        public FileInfoFunct()
        {
            _parameters.Add(new UBotParameterDefinition("Path to file", UBotType.String)); 
            var xParameter = new UBotParameterDefinition("File info wanted?", UBotType.String);
            xParameter.Options = new[] { "", "File Name", "Date Time Created", "Extension", "Total Size", "File Path", "Full File Name", "Created Date/Time", "Last Accessed Date/Time", "Last Modified Date/Time" };
            _parameters.Add(xParameter);

        }

        public string Category
        {
            get { return "System Functions"; }
        }

        public string FunctionName
        {
            get { return "$get file info"; }
        }


        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {

            string ckpcinfo = parameters["File info wanted?"];
            string file = parameters["Path to file"]; ;

            FileInfo nFileInfo = new FileInfo(file);
            string fData = string.Empty;

            if (nFileInfo != null)
            {

                if (ckpcinfo == "File Name")
                {

                    fData = nFileInfo.Name;
                    _returnValue = fData;

                }
                else if (ckpcinfo == "Date Time Created")
                {

                    DateTime dtCreationTime = nFileInfo.CreationTime;
                    fData = dtCreationTime.ToString();
                    _returnValue = fData;

                }
                else if (ckpcinfo == "Extension")
                {

                    fData = nFileInfo.Extension;
                    _returnValue = fData;

                }
                else if (ckpcinfo == "Total Size")
                {

                    fData = nFileInfo.Length.ToString();
                    _returnValue = fData;

                }
                else if (ckpcinfo == "File Path")
                {

                    fData = nFileInfo.DirectoryName;
                    _returnValue = fData;

                }
                else if (ckpcinfo == "Full File Name")
                {

                    fData = nFileInfo.FullName;
                    _returnValue = fData;

                }
                else if (ckpcinfo == "Created Date/Time")
                {
                    // local times
                    DateTime creationTime = nFileInfo.CreationTime;

                    fData = creationTime.ToString();
                    _returnValue = fData;

                }
                else if (ckpcinfo == "Last Accessed Date/Time")
                {
                    // local times
                    DateTime lastAccessTime = nFileInfo.LastAccessTime;

                    fData = lastAccessTime.ToString();
                    _returnValue = fData;

                }
                else if (ckpcinfo == "Last Modified Date/Time")
                {
                    // local times
                    // local times
                    DateTime lastWriteTime = nFileInfo.LastWriteTime;
                    DateTime lastAccessTime = nFileInfo.LastAccessTime;

                    fData = lastWriteTime.ToString();
                    _returnValue = fData;

                }

                else { }
            }
        }

        public object ReturnValue
        {
            // We return our variable _returnValue as the result of the function.
            get { return _returnValue; }
        }

        public UBotType ReturnValueType
        {
            // Our result is text, so the return value type is String.
            get { return UBotType.String; }
        }


        public bool IsContainer
        {
            get { return false; }
        }

        public IEnumerable<UBotParameterDefinition> ParameterDefinitions
        {
            get { return _parameters; }
        }

        public IEnumerable<string> Options
        {
            get;
            set;
        }

        public UBotVersion UBotVersion
        {
            get { return UBotVersion.Standard; }
        }
    }


    //
    //
    // GET PC FONTS
    //
    //
    public class GetPCFonts : IUBotFunction
    {

        private List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();
        private string _returnValue;

        public GetPCFonts()
        {
            // no parameters
        }

        public string Category
        {
            get { return "System Functions"; }
        }

        public string FunctionName
        {
            get { return "$pc font list"; }
        }


        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {


            string familyName;
            string familyList = "";
            FontFamily[] fontFamilies;

            InstalledFontCollection installedFontCollection = new InstalledFontCollection();

            // Get the array of FontFamily objects.
            fontFamilies = installedFontCollection.Families;

            // The loop below creates a large string that is a comma-separated 
            // list of all font family names. 

            int count = fontFamilies.Length;
            for (int j = 0; j < count; ++j)
            {
                familyName = fontFamilies[j].Name;
                familyList = familyList + familyName;
                familyList = familyList + ",";
            }

            //string[] pieces = familyList.Split(new string[] { "," }, StringSplitOptions.None);
            _returnValue = familyList.ToString();

        }


        public object ReturnValue
        {
            // We return our variable _returnValue as the result of the function.
            get { return _returnValue; }
        }

        public UBotType ReturnValueType
        {
            // Our result is text, so the return value type is String.
            get { return UBotType.String; }
        }

        public bool IsContainer
        {
            get { return false; }
        }

        public IEnumerable<UBotParameterDefinition> ParameterDefinitions
        {
            get { return _parameters; }
        }

        public UBotVersion UBotVersion
        {
            get { return UBotVersion.Standard; }
        }
    }

    //
    //
    // GET PC_IP
    //
    //
    public class PC_IP : IUBotFunction
    {

        private List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();
        private string _returnValue;

        public PC_IP()
        {
            // no inputs
        }

        public string Category
        {
            get { return "System Functions"; }
        }

        public string FunctionName
        {
            get { return "$pc ip address"; }
        }


        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {
            
            string Handle = GetPublicIP();

            _returnValue = Handle.ToString();


        }

        public string GetPublicIP()
        {
            String direction = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                direction = stream.ReadToEnd();
            }

            //Search for the ip in the html
            int first = direction.IndexOf("Address: ") + 9;
            int last = direction.LastIndexOf("</body>");
            direction = direction.Substring(first, last - first);

            return direction;
        }

        public object ReturnValue
        {
            // We return our variable _returnValue as the result of the function.
            get { return _returnValue; }
        }

        public UBotType ReturnValueType
        {
            // Our result is text, so the return value type is String.
            get { return UBotType.String; }
        }

        public bool IsContainer
        {
            get { return false; }
        }

        public IEnumerable<UBotParameterDefinition> ParameterDefinitions
        {
            get { return _parameters; }
        }

        public UBotVersion UBotVersion
        {
            get { return UBotVersion.Standard; }
        }
    }



    //
    //
    // MONITOR RUNNING PROCESSES
    //
    //
    public class RunningProcesses : IUBotFunction
    {

        private List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();
        private string _returnValue;

        public RunningProcesses()
        {
            _parameters.Add(new UBotParameterDefinition("Path to save to file.txt", UBotType.String));
            var MonitorProcesses = new UBotParameterDefinition("Monitor Process Name?", UBotType.String);
            MonitorProcesses.Options = new[] { "", "Browser", "Bot", "UBot Studio" };
            _parameters.Add(MonitorProcesses);
        }

        public string Category
        {
            get { return "System Functions"; }
        }

        public string FunctionName
        {
            get { return "$get running processes"; }
        }


        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {
            string savetoPath = parameters["Path to save to file.txt"];
            string MonitorProcessName = parameters["Monitor Process Name?"];
            string path = savetoPath;

            int index = 0;
            Process[] processList = Process.GetProcesses();
            foreach (Process p in processList)
            {
                index++;
                // This text is added only once to the file. 
                if (p.ProcessName == MonitorProcessName)
                {

                    if (!File.Exists(path))
                    {
                        // Create a file to write to. 
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine("------------------------");
                            sw.WriteLine("----  PROCESS DATA  ----");
                            sw.WriteLine("------------------------");
                        }
                    }

                    // This text is always added, making the file longer over time 
                    // if it is not deleted. 
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        DateTime now = DateTime.Now;

                        sw.WriteLine("Process Number: " + p);
                        sw.WriteLine("Process Id: " + index + p.Id.ToString());
                        sw.WriteLine("Process Machine Name: " + p.MachineName);
                        sw.WriteLine("Process Process Name: " + p.ProcessName);
                        sw.WriteLine("Process Paged Memory Size: " + (p.PagedMemorySize64 / 1048576) + " MB");
                        sw.WriteLine("Process Physical Memory Size: " + (p.PeakWorkingSet64 / 1048576) + " MB");
                        sw.WriteLine("Process start time: " + p.StartTime);
                        sw.WriteLine("Process Total Processor Time: " + p.TotalProcessorTime);
                        sw.WriteLine("Process Main Window Handle: " + p.MainWindowHandle);
                        sw.WriteLine("Process Main Window Title: " + p.MainWindowTitle);
                        sw.WriteLine("... " + now + " ...");
                        
                        sw.WriteLine("--------------------");
                        sw.WriteLine("--------------------");

                    }

                    // Open the file to read from. 
                    using (StreamReader sr = File.OpenText(path))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(s);
                        }
                    }

                    string text = "Wrote to file";
                    _returnValue = text.ToString();
                }
                
            }

            

        }

        public object ReturnValue
        {
            // We return our variable _returnValue as the result of the function.
            get { return _returnValue; }
        }

        public UBotType ReturnValueType
        {
            // Our result is text, so the return value type is String.
            get { return UBotType.String; }
        }

        public bool IsContainer
        {
            get { return false; }
        }

        public IEnumerable<UBotParameterDefinition> ParameterDefinitions
        {
            get { return _parameters; }
        }

        public UBotVersion UBotVersion
        {
            get { return UBotVersion.Standard; }
        }
    }

    //
    //
    // CHECK IF PROCESS IS RUNNING
    //
    //
    public class CheckForProcess : IUBotFunction
    {

        private List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();
        private string _returnValue;

        public CheckForProcess()
        {
            _parameters.Add(new UBotParameterDefinition("Process Name", UBotType.String));
        }

        public string Category
        {
            get { return "System Functions"; }
        }

        public string FunctionName
        {
            get { return "$is process running"; }
        }


        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {
            string processname = parameters["Process Name"];

            Process[] pname = Process.GetProcessesByName(processname);
            if (pname.Length == 0)
                _returnValue = "false";
            else
                _returnValue = "true";
               
        }

        public object ReturnValue
        {
            // We return our variable _returnValue as the result of the function.
            get { return _returnValue; }
        }

        public UBotType ReturnValueType
        {
            // Our result is text, so the return value type is String.
            get { return UBotType.String; }
        }

        public bool IsContainer
        {
            get { return false; }
        }

        public IEnumerable<UBotParameterDefinition> ParameterDefinitions
        {
            get { return _parameters; }
        }

        public UBotVersion UBotVersion
        {
            get { return UBotVersion.Standard; }
        }
    }

}

public static class Prompt
{
    public static string ShowDialog(string text, string caption, string valueText, string buttonText, int labelHeight, int formHeight, int formWidth, string buttonCancelText)
    {
        Form prompt = new Form();
        prompt.Width = formWidth+25;
        prompt.Height = formHeight+25;
        prompt.Text = caption;
        prompt.Icon = Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
        Label textLabel = new Label() { Left = 25, Top = 20, Width = formWidth - 25, Text = text, Height = labelHeight };
        TextBox textBox = new TextBox() { Left = 25, Top = labelHeight + 30, Width = formWidth - 25 };
        Button confirmation = new Button() { Text = buttonText, Left = 25, Width = (formWidth / 2) - 50, Top = labelHeight + 50 };
        Button cancelation = new Button() { Text = buttonCancelText, Left = (formWidth / 2)+25, Width = (formWidth / 2) - 25, Top = labelHeight + 50 };
        confirmation.Click += (sender, e) => { prompt.Close(); };
        cancelation.Click += (sender, e) => { textBox.Text = "false"; prompt.Close(); };
        prompt.Controls.Add(confirmation);
        prompt.Controls.Add(cancelation);
        prompt.Controls.Add(textLabel);
        prompt.Controls.Add(textBox);
        if (textBox.Text == "")
        {
            textBox.Text = valueText;
        }
        prompt.ShowDialog();
        return textBox.Text;
    }
}