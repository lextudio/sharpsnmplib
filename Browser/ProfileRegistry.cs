using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Mib;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace Lextm.SharpSnmpLib.Browser
{
    internal class ProfileRegistry
    {
        //private IPEndPoint _default;
        private AgentProfile _defaultProfile;
        //private string _defaultString;
        
//        internal AgentProfile GetProfile(IPEndPoint endpoint)
//        {
//            if (profiles.ContainsKey(endpoint))
//            {
//                return profiles[endpoint];
//            }
//
//            return null;
//        }

        internal event EventHandler OnChanged;

        internal IEnumerable<IPEndPoint> Names
        {
            get { return profiles.Keys; }
        }

        internal IEnumerable<AgentProfile> Profiles
        {
            get { return profiles.Values; }
        }

        internal AgentProfile DefaultProfile
        {
            get { return _defaultProfile; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _defaultProfile = value;
            }
        }
        
//	    internal string DefaultString
//	    {
//	        get { return _defaultString; }
//	    }
//
//	    internal IPEndPoint Default
//	    {
//	        get { return _default; }
//	        set
//	        {
//	            if (value == null)
//	            {
//	                throw new ArgumentNullException("value");
//	            }
//
//	            _defaultProfile = GetProfile(value);
//	            _default = value;
//	            _defaultString = value.ToString();
//	        }
//	    }
        
        internal void AddProfile(AgentProfile profile)
        {
            AddInternal(profile);
            if (OnChanged != null)
            {
                OnChanged(null, EventArgs.Empty);
            }
        }

        private void AddInternal(AgentProfile profile)
        {
            if (profiles.ContainsKey(profile.Agent))
            {
                throw new BrowserException("This endpoint is already registered");
            }

            profiles.Add(profile.Agent, profile);

            if (DefaultProfile == null)
            {
                DefaultProfile = profile;
            }
        }
        
        private IDictionary<IPEndPoint, AgentProfile> profiles = new Dictionary<IPEndPoint, AgentProfile>();

        internal void DeleteProfile(IPEndPoint profile)
        {
            DeleteInternal(profile);
            if (OnChanged != null)
            {
                OnChanged(null, EventArgs.Empty);
            }
        }

        private void DeleteInternal(IPEndPoint profile)
        {
            DeleteInternal(profile, false);
        }

        private void DeleteInternal(IPEndPoint profile, bool replace)
        {
            if (profile.Equals(DefaultProfile.Agent) && !replace)
            {
                throw new BrowserException("Cannot delete the default endpoint!");
            }

            else if(profiles.ContainsKey(profile))
            {
                profiles.Remove(profile);
            }
        }

        internal void ReplaceProfile(AgentProfile agentProfile)
        {
            DeleteInternal(agentProfile.Agent, true);
            AddInternal(agentProfile);
            if (OnChanged != null)
            {
                OnChanged(null, EventArgs.Empty);
            }
        }

        internal void LoadProfiles()
        {
            if (LoadProfilesFromFile() == 0)
            {
                LoadDefaultProfile();
            }
        }

        internal void SaveProfiles()
        {
            ICollection<IPEndPoint> myKeys = profiles.Keys;
            XmlTextWriter objXmlTextWriter = new XmlTextWriter("Agents.xml", null);
            
            objXmlTextWriter.Formatting = Formatting.Indented;
            objXmlTextWriter.WriteStartDocument();
            objXmlTextWriter.WriteStartElement("Agents");

            foreach (IPEndPoint k in myKeys)
            {
                objXmlTextWriter.WriteStartElement("Agent");

                objXmlTextWriter.WriteStartElement("Name");
                objXmlTextWriter.WriteString(profiles[k].Name);
                objXmlTextWriter.WriteEndElement();

                objXmlTextWriter.WriteStartElement("IP");
                objXmlTextWriter.WriteString(profiles[k].Agent.Address.ToString());
                objXmlTextWriter.WriteEndElement();

                objXmlTextWriter.WriteStartElement("Port");
                objXmlTextWriter.WriteValue(profiles[k].Agent.Port);
                objXmlTextWriter.WriteEndElement();

                objXmlTextWriter.WriteStartElement("SNMP");
                objXmlTextWriter.WriteValue((int)(profiles[k]).VersionCode);
                objXmlTextWriter.WriteEndElement();

                objXmlTextWriter.WriteStartElement("Get");
                objXmlTextWriter.WriteString(profiles[k].GetCommunity);
                objXmlTextWriter.WriteEndElement();

                objXmlTextWriter.WriteStartElement("Set");
                objXmlTextWriter.WriteString(profiles[k].SetCommunity);
                objXmlTextWriter.WriteEndElement();

                objXmlTextWriter.WriteStartElement("Default");
                if (DefaultProfile.Agent == k)
                {
                    objXmlTextWriter.WriteValue(true);
                }
                else
                {
                    objXmlTextWriter.WriteValue(false);
                }
                objXmlTextWriter.WriteEndElement();

                objXmlTextWriter.WriteEndElement();
            }

            objXmlTextWriter.WriteEndElement();
            objXmlTextWriter.WriteEndDocument();
            objXmlTextWriter.Flush();
            objXmlTextWriter.Close();
        }

        internal void LoadDefaultProfile()
        {
            AgentProfile first = new AgentProfile(VersionCode.V1, new IPEndPoint(IPAddress.Loopback, 161), "public", "public", "Localhost");
            AddProfile(first);
            DefaultProfile = first;
        }

        internal int LoadProfilesFromFile()
        {
            if (!File.Exists("Agents.xml"))
            {
                return 0;
            }
            VersionCode vc = VersionCode.V1;
            String name = string.Empty;
            IPAddress def = IPAddress.Loopback;
            int port = 161;
            String get = "public";
            String set = "public";
            XmlTextReader objXmlTextReader = new XmlTextReader("Agents.xml");
            bool bDefault = false;

            try
            {
                string sName = "";

                while (objXmlTextReader.Read())
                {
                    switch (objXmlTextReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            sName = objXmlTextReader.Name;
                            break;
                        case XmlNodeType.Text:
                            switch (sName)
                            {
                                case "Name":
                                    name = objXmlTextReader.Value;
                                    break;
                                case "IP":
                                    def = IPAddress.Parse(objXmlTextReader.Value);
                                    break;
                                case "Port":
                                    port = objXmlTextReader.ReadContentAsInt();
                                    break;
                                case "SNMP":
                                    vc = (VersionCode)objXmlTextReader.ReadContentAsInt();
                                    break;
                                case "Get":
                                    get = objXmlTextReader.Value;
                                    break;
                                case "Set":
                                    set = objXmlTextReader.Value;
                                    break;
                                case "Default":
                                    AgentProfile prof = new AgentProfile(vc, new IPEndPoint(def, port), get, set, name);
                                    
                                    AddProfile(prof);

                                    bDefault = objXmlTextReader.ReadContentAsBoolean();
                                    if (bDefault)
                                    {
                                        DefaultProfile = prof;
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceSource source = new TraceSource("Browser");
                source.TraceInformation(ex.ToString());
                source.Flush();
                source.Close();
            }
            finally
            {
                objXmlTextReader.Close();
            }

            return profiles.Count;
        }
    }
}
