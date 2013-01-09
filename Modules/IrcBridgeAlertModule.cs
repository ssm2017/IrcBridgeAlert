using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Nini.Config;
using OpenSim.Framework;
using OpenSim.Region.Framework.Interfaces;
using OpenSim.Region.Framework.Scenes;
using Mono.Addins;

[assembly: Addin("IrcBridgeAlertModule", "7.5")]
[assembly: AddinDependency("OpenSim", "0.5")]

namespace ModIrcBridgeAlertModule
{
	[Extension(Path = "/OpenSim/RegionModules", NodeName = "RegionModule")]
	public class AvatarServicesModule : ISharedRegionModule
	{
		#region Fields
		private static readonly ILog m_log = LogManager.GetLogger (MethodBase.GetCurrentMethod ().DeclaringType);
		private IDialogModule m_dialogModule;

		public string m_name = "IrcBridgeAlertModule";
		public string alert_message = "Warning IrcBridge active";
		private bool m_enabled = false;
		private Dictionary<string, Scene> m_scenel = new Dictionary<string, Scene> ();

		public IConfigSource m_config;
		public Scene m_world;
		public RegionInfo m_regionInfo;
		public Dictionary<string, Scene> mScene = new Dictionary<string, Scene> ();
		#endregion

		#region ISharedRegionModule implementation
		public void PostInitialise () {}
		#endregion

		#region IRegionModuleBase implementation
		public void Initialise (IConfigSource source)
		{
			m_config = source;
			IConfig cnf = source.Configs["IRC"];
			if (cnf != null)
			{
				if (cnf.GetBoolean("enabled", false))
				{
					m_enabled = true;
					alert_message = cnf.GetString("alert_msg_pre", "This region is linked to irc.")+ "\n"
						+ " \nServer "+ cnf.GetString("server", "")
						+ " \nPort "+ cnf.GetString("port", "")
						+ " \nChanel "+ cnf.GetString("channel", "")+ "\n"
						+ "\n"+ cnf.GetString("alert_msg_post", "\nEverything you say in public chat can be listened.\nSee http://opensimulator.org/wiki/IRCBridgeModule for more informations.");
				}
			}
		}

		public void Close () {}

		public void AddRegion (Scene scene)
		{
			if (m_enabled)
			{
				m_log.InfoFormat ("[IrcBridgeAlertModule]: Region {0} is using irc", scene.RegionInfo.RegionName);
				scene.EventManager.OnMakeRootAgent += OnMakeRootAgent;
			}
		}

		public void RemoveRegion (Scene scene)
		{
			scene.EventManager.OnMakeRootAgent -= OnMakeRootAgent;
			if (m_scenel.ContainsKey (scene.RegionInfo.RegionName))
			{
				lock (m_scenel)
				{
					m_scenel.Remove (scene.RegionInfo.RegionName);
				}
			}
		}

		public void RegionLoaded (Scene scene)
		{
			if (m_enabled == false)
				return;
			m_dialogModule = scene.RequestModuleInterface<IDialogModule>();
		}

		public string Name {
			get { return m_name; }
		}

		public Type ReplaceableInterface
		{
			get { return null; }
		}

		#endregion

		#region IrcBridgeAlertModule
		void OnMakeRootAgent (ScenePresence presence)
		{
			IClientAPI client = presence.ControllingClient;
			m_dialogModule.SendAlertToUser(client, alert_message, true);
		}
		#endregion
	}
}
