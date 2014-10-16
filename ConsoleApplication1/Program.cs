using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ConsoleApplication1
{
    class Program
    {
        private const string LOG_PATH = @"C:\Users\radio\Documents\EVE\logs\Chatlogs" ;
        private const string DATA_PATH = @"C:\Users\radio\Desktop\systems";
        private List<string> _systems =new List<string>() ;


        static void Main(string[] args)
        {
            
            // first things first ... get the file from the directory 
            Console.WriteLine(LOG_PATH);
            var log_info = new DirectoryInfo(LOG_PATH);
            var logs = log_info.GetFiles();

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
            //FileInfo[] info = (from file in files orderby file.CreationTime select file ).ToArray();


            Console.WriteLine("\n==== branch intel to use for this  ====\n");
            Console.WriteLine("name : {0}  create date : {1}", interesting_logs[0], interesting_logs[0].CreationTime);
            
            Console.WriteLine("==== json files ====");

            Console.WriteLine(DATA_PATH);
            var data_fh = DATA_PATH + "\\O94U-A.json";
            List<string> systems_l = new List<string>();
            var data_info = new DirectoryInfo(DATA_PATH);
            //get json string 

            if (File.Exists(data_fh))
            {
                Console.WriteLine("we gave a file to read from ");
                StreamReader my_file = new StreamReader(data_fh);
                systems_l = my_file.ReadToEnd().Trim('[', ']').Trim('\"').Replace("\",\"", " ").Split(' ').ToList();
                my_file.Close();
            }

           

            foreach (string s in systems_l)
            {
                Console.WriteLine(s);
            }
            Console.ReadKey();
        }
    }

    
}
