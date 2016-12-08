﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace worker
{
    public static class Settings
    {
        private const string settings_file = @"I:\Skånes Universitetssjukhus\CB-Strålningsfysik-Gemensam\Strålbehandling\Ansvarsområden\ScriptingAPI\GitHub\genIdFromTrajLog\worker\Settings.xml";
        
        public static string ARIA_SERVER = getFromXml(settings_file, "ARIA_SERVER");
        public static string ARIA_USERNAME = getFromXml(settings_file, "ARIA_USERNAME");
        public static string ARIA_PASSWORD = getFromXml(settings_file, "ARIA_PASSWORD");
        public static string ARIA_DATABASE = getFromXml(settings_file, "ARIA_DATABASE");



        private static string getFromXml(string file, string varible)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            return doc.SelectSingleNode("Settings/" + varible).InnerText;
        }
    }
}
