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

[assembly: Addin("IrcBridgeAlertModule", "0.1")]
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
        private bool m_enabled = false;
        private Dictionary<string, Scene> m_scenel = new Dictionary<string, Scene> ();

        public IConfigSource m_config;
        public Scene m_world;
        public RegionInfo m_regionInfo;
        public Dictionary<string, Scene> mScene = new Dictionary<string, Scene> ();
        #endregion

        #region ISharedRegionModule implementation
        public void PostInitialise ()
        {
        }

        #endregion

        #region IRegionModuleBase implementation
        public void Initialise (IConfigSource source)
        {
			m_config = source;
			IConfig cnf = source.Configs["IRC"];
			if (cnf != null) {
				if (cnf.GetBoolean("enabled", false)) {
					m_enabled = true;
				}
			}
        }


        public void Close ()
        {

        }


        public void AddRegion (Scene scene)
        {
			if (m_enabled) {
				m_log.InfoFormat ("[IrcBridgeAlertModule]: Region {0} is using irc", scene.RegionInfo.RegionName);
				m_dialogModule = scene.RequestModuleInterface<IDialogModule>();
				scene.EventManager.OnMakeRootAgent += HandleSceneEventManagerOnMakeRootAgent;
			}
        }

        public void RemoveRegion (Scene scene)
        {
            if (m_scenel.ContainsKey (scene.RegionInfo.RegionName)) {
                lock (m_scenel) {
                    m_scenel.Remove (scene.RegionInfo.RegionName);
                }
            }
        }


        public void RegionLoaded (Scene scene)
        {
            if (m_enabled == false)
                return;

        }


        public string Name {
            get { return m_name; }
        }


        public Type ReplaceableInterface {
            get { return null; }
        }

        #endregion

        #region IrcBridgeAlertModule

		void HandleSceneEventManagerOnMakeRootAgent (ScenePresence presence) {
			IClientAPI client = presence.ControllingClient;
			m_dialogModule.SendAlertToUser(client, "Attention. Irc Bridge actif.");
		}

        #endregion
    }
}
