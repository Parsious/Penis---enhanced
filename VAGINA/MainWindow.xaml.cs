using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace VAGINA
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _log_path = "" ;
        private string _xml_data_path ="";
        private const string LOG = @"\Documents\EVE\logs\Chatlogs";
        private const string DATA_PATH = @"\Desktop\systems\xml";
        private List<FileInfo> interesting_logs = new List<FileInfo>();
        private Data_Object data_1 = new Data_Object();

        private bool system_set = false;
        private bool source_set = false;
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            var log_info = new DirectoryInfo(_log_path);
            var logs = log_info.GetFiles();

            InitializeComponent();
            _log_path = build_path(LOG, Environment.UserName);
            _xml_data_path = build_path(DATA_PATH, Environment.UserName);

            output_text.AppendText(_log_path + "\n");
            output_text.AppendText(_xml_data_path + "\n");
    

            
            
            //get_logfile(interesting_logs, data_1, logs, source);
            //List<string> systems = new List<string>();
            var interesting_logs = new List<FileInfo>();

            const bool TEST = true;
            
        
        
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private string build_path(string path, string user)
        {
            return @"C:\Users\" + user + path;
        }

        private void source_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (source_combo.SelectedIndex != -1)
            {
                source_set = true;
                output_text.AppendText("change source setting | ");
                string source = get_from_combo(source_combo.SelectedItem.ToString().Split(' '));
                output_text.AppendText("\'" + source_set + "\'\n");
            }

            
            
        }

        private string get_from_combo(string[] combo_strings )
        {
            string output = "";
            for (int index = 1; index < combo_strings.Length; index++)
            {
                output += combo_strings[index] + " ";
            }
            return output.Trim();
        }

        private void get_logfile(List<FileInfo> interesting_logs, Data_Object data_1, FileInfo[] logs, string region)
        {
            foreach (var log in (from log in logs orderby log.CreationTime descending select log).ToArray())
            {
                if (log.Name.Contains(region))
                {
                    interesting_logs.Add(log);
                }
            }
            output_text.AppendText(string.Format("intelfile is {0} ", interesting_logs[0]));

            foreach (string s in data_1.systems)
            {
                output_text.AppendText(s + " | ");
            }
            output_text.AppendText("\n");
        }

        private void get_location(Data_Object data_1)
        {
            bool system_found = false;

            do
            {

                output_text.AppendText("what system are you ratting in ? ");
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
                    output_text.AppendText(string.Format("\nyou are ratting in : {0} the xmal file for that is : {1} \n", ratting_system,
                        ratting_system_file));

                    open_xml(_xml_data_path, ratting_system_file, data_1);


                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\ncould not find xml data for : \"{0}\" are you sure your in \"Branch\"", ratting_system);
                }
            } while (!system_found);
        }

        private void open_xml(string XML_DATA_PATH, string ratting_system_file, Data_Object data)
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

        private void system_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (system_combo.SelectedIndex != -1)
            {
                system_set = true;
                output_text.AppendText("change system setting | ");
                string system = get_from_combo(system_combo.SelectedItem.ToString().Split(' '));
                output_text.AppendText("\'" + system + "\'\n");
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
