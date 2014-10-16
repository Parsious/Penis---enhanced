using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            
            // first things first ... get the file from the directory 
            Console.WriteLine(LOG_PATH);
            var log_info = new DirectoryInfo(LOG_PATH);
            var logs = log_info.GetFiles();
            XmlSerializer ser = new XmlSerializer(typeof(List<string>));

            List<string> systems = new List<string>();
            
            Console.WriteLine("==== log files ====");
            Console.WriteLine("==== initial number of files ====");
            Console.WriteLine("number of files is  : " + logs.Length);
            var interesting_logs = new List<FileInfo>();

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

            Console.WriteLine("\n==== branch intel file s in order (most recent first) ====\n");
            foreach (var log in (from log in logs orderby log.CreationTime descending select log).ToArray())
            {

                if (log.Name.Contains("BRN.CFC"))
                {
                    interesting_logs.Add(log);
                }
            }

            foreach (var interesting_file in interesting_logs)
            {
                Console.WriteLine("name : {0}  create date : {1}", interesting_file, interesting_file.CreationTime);
            }
            Console.WriteLine("\n==== branch intel to use for this  ====\n");
            Console.WriteLine("name : {0}  create date : {1}", interesting_logs[0], interesting_logs[0].CreationTime);
            Console.WriteLine("==== xml file in ====");

            Console.WriteLine(XML_DATA_PATH);

            DirectoryInfo xml_dir_info = new DirectoryInfo(XML_DATA_PATH);
            FileInfo[] xml_file_info = xml_dir_info.GetFiles();

            var data_fh = XML_DATA_PATH + "\\" + xml_file_info[3];
            
            Console.WriteLine(data_fh);
            //var data_info = new DirectoryInfo(XML_DATA_PATH);
            //get json string 

            if (File.Exists(data_fh))
            {
                using (Stream s = File.OpenRead(data_fh))
                {
                    systems = (List<string>) ser.Deserialize(s);
                    s.Close();
                }

            }



            foreach (string s in systems)
            {
                Console.Write(s + " | ");
            }
            Console.ReadKey();
        }
    }

    
}
