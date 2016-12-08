using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace worker
{
    class Program
    {
        private static string mainDir = @"\\mtdb001\VA_TRANSFER\TDS\";
        private static string subDir = @"\TrajectoryLog\Treatment\";
        private static string logFileEnding = ".bin";
        private static string txtFileEnding = ".txt";

        static void Main(string[] args)
        {
            // declare variables
            string[] machine = new string[] { };
            List<string> logged = new List<string>();
            List<machine> machines = new List<machine>();
            List<LoggedTreat> loggedTreats = new List<LoggedTreat>();

            // get subdirs
            machine = getMachines(mainDir);
            // create machines
            foreach (string mach in machine)
                machines.Add(new machine(mach));

            // loop over machines to get logged treatments (excluding those with corresponing txt)
            foreach (string mach in machine)
            { 
                List<string> logs = getLogged(mach + subDir);
                //logs = getLogged(logs, mach, subDir);
                logged.AddRange(logs);
            }

            // Connect
            SqlInterface.Connect();
            foreach (string log in logged)
            {
                try
                { 
                    loggedTreats.Add(new LoggedTreat(log));
                }
                catch (Exception e)
                {

                }
            }

            foreach (LoggedTreat log in loggedTreats)
            {
                log.getRecords();
                log.writeToFile(txtFileEnding);
            }
            // Disconnect
            SqlInterface.Disconnect();

        }

        static public string[] getMachines(string mainDir)
        {
            return Directory.GetDirectories(mainDir).Where(val => val != mainDir + "Backup").ToArray();

        }

        static public List<string> getLogged(string directory)
        {
            //string logFileEnding = ".bin";
            //string txtFileEnding = ".txt";
            List<string> bins = Directory.GetFiles(directory).Where(filename => Path.GetExtension(filename) == logFileEnding).Select(filename => Path.GetFileNameWithoutExtension(filename)).ToList();
            List<string> txts = Directory.GetFiles(directory).Where(filename => Path.GetExtension(filename) == txtFileEnding).Select(filename => Path.GetFileNameWithoutExtension(filename)).ToList();
            return bins.Except(txts).Select(entry => string.Concat(directory, entry)).ToList();
        }
    }
}
