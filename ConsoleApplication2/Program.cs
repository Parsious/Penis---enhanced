using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApplication2
{
    class Program
    {

        private const string DATA_PATH = @"C:\Users\radio\Desktop\systems";
        private const string XML_DATA_PATH = @"C:\Users\radio\Desktop\systems\xml";

        static void Main(string[] args)
        {
            var systems = new List<string>();
            if (!Directory.Exists(XML_DATA_PATH))
            {
                Directory.CreateDirectory(XML_DATA_PATH);
            }

            var data_info = new DirectoryInfo(DATA_PATH);
            //var file_names = data_info.GetFiles();
            foreach (var file in data_info.GetFiles())
            {
                var data_json_fh = file.Name;
                var data_xml_fh = data_json_fh.Split('.')[0] + ".xml";

                //Console.WriteLine("data_fh      : {0}", data_json_fh);
                //Console.WriteLine("data_fh_head : {0} ", data_xml_fh);

                //get json string 
                if (File.Exists(DATA_PATH + "\\" + data_json_fh))
                {
                //    Console.WriteLine("we gave a file to read from ");
                    var my_file = new StreamReader(DATA_PATH + "\\" + data_json_fh);
                    systems = my_file.ReadToEnd().Trim('[', ']').Trim('\"').Replace("\",\"", " ").Split(' ').ToList();
                    my_file.Close();
                }

                // write as XML 
                try
                {
                    var ser = new XmlSerializer(typeof(List<string>));
                    TextWriter writer = new StreamWriter(XML_DATA_PATH + "\\" + data_xml_fh);
                    ser.Serialize(writer, systems);
                    writer.Close();
                }
                catch (Exception)
                {
                    throw;
                } 
            }
            
        }
    }
}
