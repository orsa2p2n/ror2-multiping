using BepInEx;
using UnityEngine.SceneManagement;

namespace Multiping;

[BepInPlugin("com.orsa.multiping", "Multiping", "1.0.0")]
public class Multiping : BaseUnityPlugin
{
    private readonly MultipingConfig _config;
    public MultipingConfig GetConfig() => _config;
        
    private readonly PingIndicatorManager _pingIndicatorManager;
    private readonly PingerControllerManager _pingerControllerManager;
        
    public Multiping()
    {
        _config = new MultipingConfig(Config);
        _pingIndicatorManager = new PingIndicatorManager(this);
        _pingerControllerManager = new PingerControllerManager(this);
    }

    public void Awake()
    {
        On.RoR2.PingerController.SetCurrentPing += _pingerControllerManager.On_PingerController_SetCurrentPing;
        On.RoR2.PingerController.AttemptPing += _pingerControllerManager.On_PingerController_AttemptPing;

        On.RoR2.UI.PingIndicator.Update += _pingIndicatorManager.On_PingIndicator_Update;

        SceneManager.sceneUnloaded += _pingerControllerManager.OnSceneUnloaded;
    }

    public void OnDestroy()
    {
        On.RoR2.PingerController.SetCurrentPing -= _pingerControllerManager.On_PingerController_SetCurrentPing;
        On.RoR2.PingerController.AttemptPing -= _pingerControllerManager.On_PingerController_AttemptPing;

        On.RoR2.UI.PingIndicator.Update -= _pingIndicatorManager.On_PingIndicator_Update;

        SceneManager.sceneUnloaded -= _pingerControllerManager.OnSceneUnloaded;
    }
}