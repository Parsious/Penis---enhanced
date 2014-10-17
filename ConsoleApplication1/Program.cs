using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace ConsoleApplication1
{
    class Program
    {
        private  static string _log_path = "" ;
        private static string _xml_data_path ="";
        private const string LOG = @"\Documents\EVE\logs\Chatlogs";
        private const string DATA_PATH = @"\Desktop\systems\xml";
        //private List<string> _systems =new List<string>() ;


        static void Main(string[] args)
        {
            //string user = Environment.UserName;

            _log_path = build_path(LOG, Environment.UserName);
            _xml_data_path = build_path(DATA_PATH, Environment.UserName);
            
            var log_info = new DirectoryInfo(_log_path);
            var logs = log_info.GetFiles();
            //List<string> systems = new List<string>();
            var interesting_logs = new List<FileInfo>();

            const int TEST = 3;
            Data_Object data_1 = new Data_Object();
            Data_Object data_2 = new Data_Object();
            
            
            
            //get_location(data_1);
           

            #region old test code
            //Console.WriteLine("\n==== BRN.CFC files ====\n");
            //Console.WriteLine("name : {0} \t type : {1} ",files[0] , files[0].Extension);
            
            //foreach (var file in files)
            //{
            //    if (file.Name.Contains("BRN.CFC"))
            //    {
            //        Console.WriteLine("name : {0}  create date : {1}", file , file.CreationTime);
            //    }
            //}

            //Console.WriteLine("\n==== all files in no order files ====\n");
            //foreach (var file in files)
            //{

            //    Console.WriteLine("name : {0}  create date : {1}", file, file.CreationTime);
                
            //}
            ////files = (from file in files orderby file.CreationTime select file).ToArray();
            //Console.WriteLine("\n==== all files in date order files ====\n");
            //foreach (var file in (from file in files orderby file.CreationTime descending select file).ToArray()) 
            //{

            //    Console.WriteLine("name : {0}  create date : {1}", file, file.CreationTime);
            //}
            #endregion

            string region = "";
            if (TEST == 1)
            {
                region = "BRN.CFC";
                get_location(data_1, null);
                get_logfile(interesting_logs, data_1, logs, "BRN.CFC");
                bool eof_b = intel_file(interesting_logs, data_1, null);

            }
            else if (TEST == 2)
            {
                region = "par chan";
                get_location(data_1, null);
                get_logfile(interesting_logs, data_1, logs, "par chan");
                bool eof_b = intel_file(interesting_logs, data_1, null);
            }
            else
            {
                region = "both";
                get_location(data_1, data_2);
                get_logfile(interesting_logs, data_1, logs, "BRN.CFC");
                get_logfile(interesting_logs, data_2, logs, "par chan");
                bool eof_b = intel_file(interesting_logs, data_1, data_2);
            }
            //Console.WriteLine("\n==== branch intel file s in order (most recent first) ====\n");
            


            

           
             //now open the branch intel file 

            
            
            Console.ReadKey();
        }

        private static void get_logfile(List<FileInfo> interesting_logs, Data_Object data_1, FileInfo[] logs, string region)
        {
            foreach (var log in (from log in logs orderby log.CreationTime descending select log).ToArray())
            {
                if (log.Name.Contains(region))
                {
                    interesting_logs.Add(log);
                }
            }
            Console.WriteLine("intelfile is {0} ", interesting_logs[0]);

            foreach (string s in data_1.systems)
            {
                Console.Write(s + " | ");
            }
        }

        private static string build_path(string path, string user)
        {
            return @"C:\Users\" + user + path;
        }

        private static void get_location(Data_Object data_1 ,Data_Object data_2)
        {
            bool system_found = false;
            bool multi = false;
            if (data_2 != null)
            {
                multi = true;
            }
            do
            {
                
                Console.Write("what system are you ratting in ? ");
                string ratting_system = Console.ReadLine();
                string ratting_system_file = string.Empty;
                FileInfo[] xml_file_info = new DirectoryInfo(_xml_data_path).GetFiles();
                foreach (var xml_file in xml_file_info)
                {
                    if (ratting_system != null && xml_file.Name.Split('.')[0] == ratting_system.ToUpper())
                    {

                        ratting_system_file = xml_file.Name;
                        system_found = true;
                    }
                }
                if (system_found)
                {
                    Console.WriteLine("\nyou are ratting in : {0} the xmal file for that is : {1} \n", ratting_system,
                        ratting_system_file);

                    open_xml(_xml_data_path, ratting_system_file, data_1);
                    if (multi)
                    {
                        open_xml(_xml_data_path, ratting_system_file, data_2);
                    }

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\ncould not find xml data for : \"{0}\" are you sure your in \"Branch\"", ratting_system);
                }
            } while (!system_found);
        }

        private static bool intel_file(List<FileInfo> interesting_logs, Data_Object data, Data_Object data_2)
        {
            bool match = false;
            bool multi = false;
            if (data_2 != null)
                {
                    multi = true;
                }
            using (var fs = new FileStream(_log_path + "\\" + interesting_logs[0], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))   
            using (StreamReader intel_stream = new StreamReader(fs))
                
            {
                while (true)
                {
                    Thread.Sleep(500);
                    string line = intel_stream.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        
                        check_words(line, data, 1);
                        if (multi)
                        {
                            check_words(line, data_2, 2); 
                        }
                    }
                    if (intel_stream.Peek() == -1)
                    {
                        
                    }
                    
                }
            }
            return true;
        }

        private static void check_words(string line, Data_Object data, int number)
        {
            bool hit = false;
            Console.WriteLine(number +" === " + line);
            foreach (var system in data.systems)
            {
                if (line.Contains(system.ToLower())&& !hit)
                {
                    hit = true;
                    Console.WriteLine(number +" ===hit===");
                    SystemSounds.Beep.Play();
                    SystemSounds.Beep.Play();
                    SystemSounds.Beep.Play();
                    //return true;
                }
            }
            //return false;
        }

        private static void open_xml(string XML_DATA_PATH, string ratting_system_file, Data_Object data)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<string>));
            var data_fh = XML_DATA_PATH + "\\" + ratting_system_file;
            Console.WriteLine(data_fh + "");
            if (File.Exists(data_fh))
            {
                using (Stream s = File.OpenRead(data_fh))
                {
                    data.systems = (List<string>)ser.Deserialize(s);
                    s.Close();
                }
                
            }
        }   
    }

    internal class Data_Object
    {
        public List<String> systems { get; set; }


        //public void systems_add(List<string> to_add)
        //{
        //    systems = to_add;
        //}
    }
}
