using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace XMLmanager
{
    class XMLmana
    {
        //variables
        XmlDocument doc = new XmlDocument();
        public XmlNode rootNode;

        //start func
        public XMLmana(List<string[]> sett)
        {
            //start
            if (File.Exists("settings.xml"))
            {
                doc.Load("settings.xml");
                rootNode = doc.DocumentElement;
            }
            else
            {
                rootNode = doc.CreateElement("settings");
                doc.AppendChild(rootNode);

                AddSettingM(sett);

                doc.Save("settings.xml");
            }
            /*error messages
                MessageBox.Show("Missing arguments for creating a new \"settings.xml\"\nError from class constructor", "XMLmanager Error");
                MessageBox.Show("Unable to find or load \"settings.xml\"\nError from class constructor", "XMLmanager Error");
            */
        }
        
        //Edits a Settings entry
        public void EditSetting(String term, String val)
        {
            XmlNode nn = rootNode.SelectSingleNode(term);
            if (nn != null)
            {
                nn.Attributes[1].Value = val;
                doc.Save("settings.xml");
            }
            else
                MessageBox.Show("Setting not found\nSearching for: " + term, "XMLmanager Error");
            
        }

        //Adds a new Settings entry
        public void AddSetting(String title, String val1, String val2=null)
        {
            //add node/entry
            XmlNode settingNode = doc.CreateElement(title);

            XmlAttribute settingAttribute = doc.CreateAttribute("default");
            settingAttribute.Value = @val1;
            settingNode.Attributes.Append(settingAttribute);

            settingAttribute = doc.CreateAttribute("user");
            settingAttribute.Value = @val2;
            settingNode.Attributes.Append(settingAttribute);
            
            rootNode.AppendChild(settingNode);
            //end adding
            doc.Save("settings.xml");
        }

        public void AddSettingM(List<string[]> items)
        {
            foreach (String[] it in items)
            {
                AddSetting(it[0], it[1]);
            }
        }
        
        public String[] SearchSetting(String term)
        {
            String def, user;
            def = rootNode.SelectSingleNode(term).Attributes[0].Value;
            user = rootNode.SelectSingleNode(term).Attributes[1].Value;

            var item = new[] { term, def, user };

            return item;
        }

        public List<String[]> SearchSettingM()
        {
            List<String[]> show = new List<String[]>();

            foreach (XmlNode nn in rootNode)
            {
                XmlAttributeCollection set = nn.Attributes;
                String[] ad = new[] { nn.Name, set[0].Value, set[1].Value};

                show.Add(ad);
            }

            return show;
        }

        public String ShowXML()
        {
            return doc.InnerXml;
        }
    }

}
