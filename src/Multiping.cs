using BepInEx;
using UnityEngine.SceneManagement;

namespace Multiping
{
    [BepInPlugin("com.orsa.multiping", "Multiping", "1.0.0")]
    public class Multiping : BaseUnityPlugin
    {
        private static MultipingConfig _config;
        public static MultipingConfig GetConfig() => _config;

        public string test;
        
        public Multiping()
        {
            _config = new MultipingConfig(Config);
            test = "";
            print(test);
        }

        public void Awake()
        {
            var pingIndicator = new PingIndicator(this);
            var pingerController = new PingerController(this);

            On.RoR2.PingerController.SetCurrentPing += pingerController.SetCurrentPing;
            On.RoR2.PingerController.AttemptPing += pingerController.AttemptPing;

            On.RoR2.UI.PingIndicator.Update += pingIndicator.Update;

            SceneManager.sceneUnloaded += pingerController.OnSceneUnloaded;
        }
    }
}