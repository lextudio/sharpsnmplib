using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;

namespace Lextm.SharpSnmpLib.Browser
{
	internal class ProfileRegistry : IProfileRegistry
	{
		private const string SettingFile = "Agents3.xml";
		private AgentProfile _defaultProfile;
		// private readonly IDictionary<IPEndPoint, AgentProfile> _profiles = new Dictionary<IPEndPoint, AgentProfile>();
		private readonly IDictionary<Guid, AgentProfile> _profiles = new Dictionary<Guid, AgentProfile>();
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger("Lextm.SharpSnmpLib.Browser");

		public event EventHandler<EventArgs> OnChanged;

		public IEnumerable<AgentProfile> Profiles
		{
			get { return _profiles.Values; }
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
			if (_profiles.ContainsKey(profile.Id))
			{
				throw new BrowserException("This endpoint is already registered");
			}

			_profiles.Add(profile.Id, profile);

			if (DefaultProfile == null)
			{
				DefaultProfile = profile;
			}
		}
		
		public void DeleteProfile(AgentProfile profile)
		{
			DeleteInternal(profile.Id);
			if (OnChanged != null)
			{
				OnChanged(null, EventArgs.Empty);
			}
		}

		private void DeleteInternal(Guid id)
		{
			DeleteInternal(id, false);
		}

		private void DeleteInternal(Guid id, bool replace)
		{
			if (id.Equals(DefaultProfile.Id) && !replace)
			{
				throw new BrowserException("Cannot delete the default endpoint!");
			}

			if(_profiles.ContainsKey(id))
			{
				_profiles.Remove(id);
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
			ICollection<Guid> myKeys = _profiles.Keys;
			using (XmlTextWriter writer = new XmlTextWriter(SettingFile, null))
			{
				writer.Formatting = Formatting.Indented;
				writer.WriteStartDocument();
				writer.WriteStartElement("agents");
				writer.WriteStartAttribute("defaultAgent");
				writer.WriteString(_defaultProfile.Id.ToString());
				writer.WriteEndAttribute();

				foreach (Guid k in myKeys)
				{
					writer.WriteStartElement("add");
					
					writer.WriteStartAttribute("id");
					writer.WriteString(_profiles[k].Id.ToString());
					writer.WriteEndAttribute();
					
					writer.WriteStartAttribute("name");
					writer.WriteString(_profiles[k].Name);
					writer.WriteEndAttribute();
					
					writer.WriteStartAttribute("binding");
					writer.WriteString(string.Format("{0}:{1}", _profiles[k].Agent.Address, _profiles[k].Agent.Port));
					writer.WriteEndAttribute();
					
					writer.WriteStartAttribute("version");
					writer.WriteValue((int)(_profiles[k]).VersionCode);
					writer.WriteEndAttribute();

				    var normal = _profiles[k] as NormalAgentProfile;
					writer.WriteStartAttribute("getCommunity");
					writer.WriteString(normal == null ? string.Empty : normal.GetCommunity);
					writer.WriteEndAttribute();
					
					writer.WriteStartAttribute("setCommunity");
                    writer.WriteString(normal == null ? string.Empty : normal.SetCommunity);
					writer.WriteEndAttribute();

				    var secure = _profiles[k] as SecureAgentProfile;
                    writer.WriteStartAttribute("authenticationPassphrase");
                    writer.WriteString(secure == null ? string.Empty : secure.AuthenticationPassphrase);
                    writer.WriteEndAttribute();

                    writer.WriteStartAttribute("privacyPassphrase");
                    writer.WriteString(secure == null ? string.Empty : secure.PrivacyPassphrase);
                    writer.WriteEndAttribute();

                    writer.WriteStartAttribute("autheticationMethod");
                    writer.WriteString(secure == null ? string.Empty : secure.AuthenticationMethod.ToString());
                    writer.WriteEndAttribute();

                    writer.WriteStartAttribute("privacyMethod");
                    writer.WriteString(secure == null ? string.Empty : secure.PrivacyMethod.ToString());
                    writer.WriteEndAttribute();

                    writer.WriteStartAttribute("userName");
                    writer.WriteString(_profiles[k].UserName);
                    writer.WriteEndAttribute();

					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteEndDocument();
				writer.Flush();
				writer.Close();
			}
		}

	    private void LoadDefaultProfile()
		{
			AgentProfile first = AgentProfileFactory.Create(Guid.Empty, VersionCode.V1, new IPEndPoint(IPAddress.Loopback, 161), "public", "private", "localhost", string.Empty, string.Empty, 0, 0, string.Empty);
			AddProfile(first);
			DefaultProfile = first;
		}

	    private int LoadProfilesFromFile()
		{
			if (!File.Exists(SettingFile))
			{
				return 0;
			}

		    Guid defaultId = Guid.Empty;
			using (XmlTextReader reader = new XmlTextReader(SettingFile))
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
										string name = reader.GetAttribute("name");
										Guid id = new Guid(reader.GetAttribute("id"));
									    string[] parts = reader.GetAttribute("binding").Split(new[] {':'});
										IPAddress def = IPAddress.Parse(parts[0]);
										int port = int.Parse(parts[1]);
										VersionCode vc = (VersionCode)int.Parse(reader.GetAttribute("version"));
										string get = reader.GetAttribute("getCommunity");
										string set = reader.GetAttribute("setCommunity");
									    string authenticationPassphrase = reader.GetAttribute("authenticationPassphrase");
									    string privacyPassphrase = reader.GetAttribute("privacyPassphrase");
									    string authenticationMethod = reader.GetAttribute("authenticationMethod");
									    string privacyMethod = reader.GetAttribute("privacyMethod");
									    string userName = reader.GetAttribute("userName");

                                        if (def == null)
                                        {
                                            break;
                                        }

									    AgentProfile profile = AgentProfileFactory.Create(id, vc, new IPEndPoint(def, port), get, set, name,
									                                            authenticationPassphrase, privacyPassphrase, 
                                                                                string.IsNullOrEmpty(authenticationMethod) ? 0 : int.Parse(authenticationMethod),
									                                            string.IsNullOrEmpty(privacyMethod) ? 0 : int.Parse(privacyMethod),
                                                                                userName);
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
					Logger.Info(ex.ToString());
				}
				finally
				{
					reader.Close();
				}
			}

	        return _profiles.Count;
		}
	}
}
