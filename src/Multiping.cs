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

        private PingIndicator _pingIndicator;
        private PingerController _pingerController;

        public void Awake()
        {
            _pingIndicator = new PingIndicator(this);
            _pingerController = new PingerController(this);

            On.RoR2.PingerController.SetCurrentPing += _pingerController.SetCurrentPing;
            On.RoR2.PingerController.AttemptPing += _pingerController.AttemptPing;

            On.RoR2.UI.PingIndicator.Update += _pingIndicator.Update;

            SceneManager.sceneUnloaded += _pingerController.OnSceneUnloaded;
        }

        public void OnDestroy()
        {
            On.RoR2.PingerController.SetCurrentPing -= _pingerController.SetCurrentPing;
            On.RoR2.PingerController.AttemptPing -= _pingerController.AttemptPing;

            On.RoR2.UI.PingIndicator.Update -= _pingIndicator.Update;

            SceneManager.sceneUnloaded -= _pingerController.OnSceneUnloaded;
        }
    }
}