using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace ConsoleApplication1
{
    class Program
    {
        private const string LOG_PATH = @"C:\Users\radio\Documents\EVE\logs\Chatlogs" ;
        private const string XML_DATA_PATH = @"C:\Users\radio\Desktop\systems\xml";
        private const string DATA_PATH = @"C:\Users\radio\Desktop\systems";
        private List<string> _systems =new List<string>() ;


        static void Main(string[] args)
        {
            var log_info = new DirectoryInfo(LOG_PATH);
            var logs = log_info.GetFiles();
            List<string> systems = new List<string>();
            var interesting_logs = new List<FileInfo>();
            const bool TEST = true;
            Data_Object data_1 = new Data_Object();

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
            if (!TEST)
            {
                region = "BRN.CFC";

            }
            else
            {
                region = "par chan";
            }
            //Console.WriteLine("\n==== branch intel file s in order (most recent first) ====\n");
            foreach (var log in (from log in logs orderby log.CreationTime descending select log).ToArray())
            {

                if (log.Name.Contains(region))
                {
                    interesting_logs.Add(log);
                }
            }
            Console.WriteLine("intelfile is {0} ", interesting_logs[0]);
            bool system_found = false;

            do
            {
                Console.Write("what system are you ratting in ? ");
                string ratting_system = Console.ReadLine();
                string ratting_system_file = string.Empty;
                FileInfo[] xml_file_info = new DirectoryInfo(XML_DATA_PATH).GetFiles();
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

                    open_xml(XML_DATA_PATH, ratting_system_file, data_1);

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\ncould not find xml data for : \"{0}\" are you sure your in \"Branch\"", ratting_system);
                }
            } while (!system_found);

            foreach (string s in data_1.systems)
            {
                Console.Write(s + " | ");
            }
             //now open the branch intel file 

            bool eof_b = intel_file(interesting_logs, data_1);
            
            Console.ReadKey();
        }

        private static bool intel_file(List<FileInfo> interesting_logs, Data_Object data)
        {
            bool match = false;
            using (var fs = new FileStream(LOG_PATH + "\\" + interesting_logs[0], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))   
            using (StreamReader intel_stream = new StreamReader(fs))
            {
                while (true)
                {
                    Thread.Sleep(500);
                    string line = intel_stream.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        check_words(line, data); 
                    }
                    if (intel_stream.Peek() == -1)
                    {
                        
                    }
                    
                }
            }
            return true;
        }

        private static void check_words(string line, Data_Object data)
        {
            bool hit = false;
            Console.WriteLine("="+line);
            foreach (var system in data.systems)
            {
                if (line.Contains(system.ToLower())&& !hit)
                {
                    hit = true;
                    Console.WriteLine("===hit===");
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
