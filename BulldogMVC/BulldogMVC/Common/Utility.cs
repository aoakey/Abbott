using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using BulldogMVC.Models;

namespace BulldogMVC.Common
{
    public class Utility
    {
        private static XDocument xml = null;
        private static XDocument config = null;

        public static void LoadXml()
        {
            xml = XDocument.Load("D:\\Client Work\\Tamman\\abbot\\definition.xml");
        }

        public static void LoadConfigXml()
        {
            config = XDocument.Load("D:\\Client Work\\Tamman\\abbot\\config.xml");
        }

        public static void SaveConfigXml(XDocument config)
        {
            config.Save("D:\\Client Work\\Tamman\\abbot\\config.xml");
        }

        public static string GetValue(int id)
        {
            if (config == null)
            {
                LoadConfigXml();
            }

            try
            {

                XElement result = (from x in config.Descendants("control")
                                   where x.Attribute("id").Value == id.ToString()
                                   select x).First();


                //single value
                return result.Value;

            }
            catch (Exception ex)
            {
                //no value saved, return default
                if (xml == null)
                {
                    LoadXml();
                }

                XElement result = (from x in xml.Descendants("control")
                                   where x.Attribute("id").Value == id.ToString()
                                   select x).First();
                XElement def = (from d in result.Descendants("default")
                                select d).First();

                return (def == null ? string.Empty : def.Value);

            }
        }

        public static List<string> GetValues(int id)
        {
            if (config == null)
            {
                LoadConfigXml();
            }

            try
            {
                XElement result = (from x in config.Descendants("control")
                                   where x.Attribute("id").Value == id.ToString()
                                   select x).First();

                XElement valuesXElement = result.Element("values");
                List<string> s = new List<string>();

                if (valuesXElement != null)
                {
                    List<XElement> values = valuesXElement.Elements("value").ToList<XElement>();                    

                    foreach (XElement x in values)
                    {
                        s.Add(x.Value);
                    }                    
                }
                else
                {
                    s.Add(result.Value);
                }
                return s;
            }
            catch
            {
                //no value saved, return default
                if (xml == null)
                {
                    LoadXml();
                }

                XElement result = (from x in xml.Descendants("control")
                                   where x.Attribute("id").Value == id.ToString()
                                   select x).First();
                XElement def = result.Element("default");

                List<string> s = new List<string>();

                if (def != null)
                {
                    List<XElement> values = def.Elements("value").ToList<XElement>();
                    if (values.Count > 0)
                    {
                        foreach (XElement val in values)
                        {
                            s.Add(val.Value);
                        }
                    }

                }

                return s;
            }
        }

        public static Models.ConfigContol GetControlModel(int id)
        {
            if (xml == null)
            {
                LoadXml();
            }

            XElement result = (from x in xml.Descendants("control")
                               where x.Attribute("id").Value == id.ToString()
                               select x).First();

            Models.ConfigContol ctrl = new ConfigContol();

            ctrl.Id = id;
            ctrl.Type = GetXMLValue(result, "type", "");  //result.Elements("type").First().Value;
            ctrl.Mode = GetXMLValue(result, "mode", "");  //result.Elements("mode").First().Value;
            ctrl.Length = int.Parse(GetXMLValue(result, "length", "0"));  //int.Parse(result.Elements("length").First().Value);
            ctrl.Size = GetXMLValue(result, "size", "");  //result.Elements("size").First().Value;
            ctrl.Min = GetXMLValue(result, "min", "");  //result.Elements("min").First().Value;
            ctrl.Max = GetXMLValue(result, "max", "");  //result.Elements("max").First().Value;
            ctrl.Default = GetXMLValue(result, "default", "");  //result.Elements("default").First().Value;
            ctrl.Required = bool.Parse(GetXMLValue(result, "required", "false"));  //bool.Parse(result.Elements("required").First().Value);
            ctrl.RegEx = GetXMLValue(result, "regex", "");  //result.Elements("regex").First().Value;
            ctrl.Name = GetXMLValue(result, "name", "");  //result.Elements("name").First().Value;
            ctrl.Text = GetXMLValue(result, "text", "");  //result.Elements("text").First().Value;
            ctrl.Description = GetXMLValue(result, "Description", "");  //result.Elements("Description").First().Value;    

            List<XElement> options = (from o in result.Descendants("option")
                                      select o).ToList<XElement>();

            ctrl.Options = new Dictionary<string, string>();
            foreach (XElement o in options)
            {
                string value = o.Attribute("value").Value;
                string name = o.Value;
                KeyValuePair<string, string> option = new KeyValuePair<string, string>(name, value);
                ctrl.Options.Add(option);
            }

            return ctrl;
        }

        public static Models.Section GetSectionModel(int id)
        {
            if (xml == null)
            {
                LoadXml();
            }

            XElement result = (from x in xml.Element("BulldogDefinition").Element("sections").Elements("section")
                               where x.Attribute("id").Value == id.ToString()
                               select x).First();

            Models.Section sec = new Section();

            List<XElement> ctrls = (from e in result.Descendants("control")
                                    select e).ToList<XElement>();

            sec.Controls = new List<ConfigContol>();

            foreach (XElement c in ctrls)
            {
                int ctrlId = int.Parse(c.Attribute("id").Value);
                sec.Controls.Add(GetControlModel(ctrlId));
            }

            sec.Id = id;
            sec.Name = result.Elements("name").First().Value;
            sec.NewPage = bool.Parse(result.Elements("newPage").First().Value);
            sec.Description = result.Elements("description").First().Value;
            return sec;

        }

        public static Models.Definition GetDefinitionModel()
        {
            LoadXml();

            XElement result = xml.Descendants("BulldogDefinition").First(); //(XElement)from x in xml.Elements("BulldogDefinition")
            //select x;

            Definition def = new Definition();

            def.Id = int.Parse(GetXMLValue(result, "id", "0"));
            def.Name = GetXMLValue(result, "name", "");
            def.Description = GetXMLValue(result, "description", "");

            def.VersionName = GetXMLValue(result.Element("version"), "name", "");
            def.VersionNumber = GetXMLValue(result.Element("version"), "number", "");

            def.CreatedBy = result.Element("created").Element("createdBy").Value;
            def.CreationDate = DateTime.Parse(result.Element("created").Element("date").Value);

            def.ModifiedBy = result.Element("modified").Element("modifiedBy").Value;
            def.ModificationDate = DateTime.Parse(result.Element("modified").Element("date").Value);

            List<XElement> secs = (from s in result.Element("sections").Elements("section")
                                   select s).ToList<XElement>();

            def.Sections = new List<Section>();

            foreach (XElement s in secs)
            {
                int sectionId = int.Parse(s.Attribute("id").Value);
                def.Sections.Add(GetSectionModel(sectionId));
            }

            return def;

        }

        private static string GetXMLValue(XElement element, string name, string defaultValue)
        {
            try
            {
                return element.Element(name).Value;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static void SaveData(HttpRequestBase request)
        {
            Models.Definition def = GetDefinitionModel();

            if (config == null)
            {
                LoadConfigXml();
            }

            XElement modified = config.Descendants("modified").First();
            modified.Element("modifiedBy").Value = "test";
            modified.Element("date").Value = DateTime.Now.ToShortDateString();

            XElement sections = config.Descendants("sections").First();
            sections.Descendants().Remove();

            foreach (Models.Section section in def.Sections)
            {
                XElement sectionXElement = new XElement("section");                
                sectionXElement.SetAttributeValue("id", section.Id);

                foreach (Models.ConfigContol ctrl in section.Controls)
                {
                    string ctrlId = ctrl.Type + "_" + ctrl.Id.ToString();
                    string value = request.Form[ctrlId];

                    XElement controlXElement = new XElement("control");
                    controlXElement.SetAttributeValue("id", ctrl.Id);

                    switch (ctrl.Type.ToLower())
                    {
                        case "slider":
                            if (ctrl.Length == 1)
                            {
                                controlXElement.Value = value;
                            }
                            else
                            {
                                string[] values = value.Split(char.Parse(","));
                                XElement valuesXElement = new XElement("values");
                                controlXElement.Add(valuesXElement);
                                foreach (string v in values)
                                {
                                    XElement valueXElement = new XElement("value");
                                    valueXElement.Value = v;
                                    valuesXElement.Add(valueXElement);
                                }
                            }
                            break;
                        case "list":
                            if (ctrl.Mode == "SelectOne")
                            {
                                controlXElement.Value = value;
                            }
                            else
                            {
                                string[] values = value.Split(char.Parse(","));
                                XElement valuesXElement = new XElement("values");
                                controlXElement.Add(valuesXElement);
                                foreach (string v in values)
                                {
                                    XElement valueXElement = new XElement("value");
                                    valueXElement.Value = v;
                                    valuesXElement.Add(valueXElement);
                                }
                            }
                            break;
                        default:
                            controlXElement.Value = value;
                            break;
                    }

                    sectionXElement.Add(controlXElement);

                }
                sections.Add(sectionXElement);
            }
            SaveConfigXml(config);
        }
    }
}