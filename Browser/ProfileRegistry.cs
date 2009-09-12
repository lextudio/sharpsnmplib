using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml;

namespace Lextm.SharpSnmpLib.Browser
{
	internal class ProfileRegistry : IProfileRegistry
	{
		private const string settingFile = "Agents2.xml";
		private AgentProfile _defaultProfile;

		public event EventHandler<EventArgs> OnChanged;

		internal IEnumerable<IPEndPoint> Names
		{
			get { return profiles.Keys; }
		}

		public IEnumerable<AgentProfile> Profiles
		{
			get { return profiles.Values; }
		}

		public AgentProfile DefaultProfile
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

		public void AddProfile(AgentProfile profile)
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
		
		private readonly IDictionary<IPEndPoint, AgentProfile> profiles = new Dictionary<IPEndPoint, AgentProfile>();

		public void DeleteProfile(IPEndPoint profile)
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

			if(profiles.ContainsKey(profile))
			{
				profiles.Remove(profile);
			}
		}

		public void ReplaceProfile(AgentProfile agentProfile)
		{
			DeleteInternal(agentProfile.Agent, true);
			AddInternal(agentProfile);
			if (OnChanged != null)
			{
				OnChanged(null, EventArgs.Empty);
			}
		}

		public void LoadProfiles()
		{
			if (LoadProfilesFromFile() == 0)
			{
				LoadDefaultProfile();
			}
		}

		public void SaveProfiles()
		{
			ICollection<IPEndPoint> myKeys = profiles.Keys;
			using (XmlTextWriter writer = new XmlTextWriter(settingFile, null))
			{
				writer.Formatting = Formatting.Indented;
				writer.WriteStartDocument();
				writer.WriteStartElement("agents");
				writer.WriteStartAttribute("defaultAgent");
				writer.WriteString(_defaultProfile.Id.ToString());
				writer.WriteEndAttribute();

				foreach (IPEndPoint k in myKeys)
				{
					writer.WriteStartElement("add");
					
					writer.WriteStartAttribute("id");
					writer.WriteString(profiles[k].Id.ToString());
					writer.WriteEndAttribute();
					
					writer.WriteStartAttribute("name");
					writer.WriteString(profiles[k].Name);
					writer.WriteEndAttribute();
					
					writer.WriteStartAttribute("binding");
					writer.WriteString(profiles[k].Agent.Address.ToString() + ":" + profiles[k].Agent.Port);
					writer.WriteEndAttribute();
					
					writer.WriteStartAttribute("version");
					writer.WriteValue((int)(profiles[k]).VersionCode);
					writer.WriteEndAttribute();
					
					writer.WriteStartAttribute("getCommunity");
					writer.WriteString(profiles[k].GetCommunity);
					writer.WriteEndAttribute();
					
					writer.WriteStartAttribute("setCommunity");
					writer.WriteString(profiles[k].SetCommunity);
					writer.WriteEndAttribute();

					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteEndDocument();
				writer.Flush();
				writer.Close();
			}
		}

		internal void LoadDefaultProfile()
		{
			AgentProfile first = new AgentProfile(Guid.Empty, VersionCode.V1, new IPEndPoint(IPAddress.Loopback, 161), "public", "public", "Localhost");
			AddProfile(first);
			DefaultProfile = first;
		}

		internal int LoadProfilesFromFile()
		{
			if (!File.Exists(settingFile))
			{
				return 0;
			}
			
			VersionCode vc = VersionCode.V1;
			String name = string.Empty;
			IPAddress def = IPAddress.Loopback;
			int port = 161;
			String get = "public";
			String set = "public";
			Guid id = Guid.Empty;
			Guid defaultId = Guid.Empty;
			using (XmlTextReader reader = new XmlTextReader(settingFile))
			{
				try
				{
					while (reader.Read())
					{
						switch (reader.NodeType)
						{
							case XmlNodeType.Element:
								{
									if (reader.Name == "agents")
									{
										defaultId = new Guid(reader.GetAttribute("defaultAgent"));
									}
									else if (reader.Name == "add")
									{
										name = reader.GetAttribute("name");
										id = new Guid(reader.GetAttribute("id"));
										string[] parts = reader.GetAttribute("binding").Split(new char[] {':'});
										def = IPAddress.Parse(parts[0]);
										port = int.Parse(parts[1]);
										vc = (VersionCode)int.Parse(reader.GetAttribute("version"));
										get = reader.GetAttribute("getCommunity");
										set = reader.GetAttribute("setCommunity");
										AgentProfile profile = new AgentProfile(id, vc, new IPEndPoint(def, port), get, set, name);
										if (id == defaultId)
										{
											DefaultProfile = profile;
										}
										
										AddProfile(profile);
									}
									
									break;
								}
								
							default:
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
					reader.Close();
				}
			}
			
			foreach (AgentProfile profile in profiles.Values)
			{
				if (profile.Id == defaultId)
				{
					
				}
			}

			return profiles.Count;
		}
	}
}
