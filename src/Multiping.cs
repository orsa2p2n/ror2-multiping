using BepInEx;
using UnityEngine.SceneManagement;

namespace Multiping
{
    [BepInPlugin("com.orsa.multiping", "Multiping", "1.0.0")]
    public class Multiping : BaseUnityPlugin
    {
        #region Private Fields

        /// <summary>
        /// Configuration instance
        /// </summary>
        private static MultipingConfig _config;

        #endregion

        public MultipingConfig GetConfig() => _config;

        #region Configuration and Startup

        public Multiping()
        {
            _config = new MultipingConfig(Config);
        }

        public void Awake()
        {
            PingIndicator pingIndicator = new PingIndicator(this);
            PingerController pingerController = new PingerController(this);

            On.RoR2.PingerController.SetCurrentPing += pingerController.SetCurrentPing;
            On.RoR2.PingerController.AttemptPing += pingerController.AttemptPing;

            On.RoR2.UI.PingIndicator.Update += pingIndicator.Update;

            SceneManager.sceneUnloaded += pingerController.OnSceneUnloaded;
        }

        #endregion
    }
}