using System.Collections.Generic;
using RoR2;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Text;
using RoR2.UI;

namespace Multiping;

class PingerControllerManager
{
    private readonly MultipingConfig _config;
        
    private readonly PingPainter _painter;
    private readonly PingTextBuilder _textBuilder;
        
    private readonly Dictionary<string, Color> _colors = new();
    private readonly List<PingIndicator> _pingIndicators = [];

    public PingerControllerManager(Multiping plugin)
    {
        _config = plugin.GetConfig();
            
        _painter = new PingPainter(_config);
        _textBuilder = new PingTextBuilder(_config);
    }

    // --- HOOKS ---
    
    public void On_PingerController_SetCurrentPing(On.RoR2.PingerController.orig_SetCurrentPing orig, PingerController pingerController, PingerController.PingInfo pingInfo)
    {
        ProcessPing(pingerController, pingInfo);
    }

    public void On_PingerController_AttemptPing(On.RoR2.PingerController.orig_AttemptPing orig, PingerController self, Ray aimRay, GameObject bodyObject) {
        // HACK: Empty ping so we can ping the same object multiple times in a row
        self.currentPing = PingerController.emptyPing;
            
        orig(self, aimRay, bodyObject);
    }
        
    public void OnSceneUnloaded(Scene scene)
    {
        _pingIndicators.Clear();
    }
    
    // --- LOGIC ---
    
    private void ProcessPing(PingerController pingerController, PingerController.PingInfo pingInfo)
    {
        pingerController.pingStock = 99;

        // Pinging a non-valid location places a ping at 0,0,0
        if (pingInfo.origin == Vector3.zero) return;
        
        var unpinged = TryUnping(pingerController, pingInfo);
        
        if (unpinged)
        {
            return;
        }

        pingerController.NetworkcurrentPing = pingInfo;
        
        // Create an instance of PingIndicator instead of using PingerController.RebuildPing()
        var gameObj = (GameObject) Object.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/PingIndicator"));
        var pingIndicator = gameObj.GetComponent<PingIndicator>();

        pingIndicator.pingOwner = pingerController.gameObject;
        pingIndicator.pingOrigin = pingInfo.origin;
        pingIndicator.pingNormal = pingInfo.normal;
        pingIndicator.pingTarget = pingInfo.targetGameObject;
        pingIndicator.weakPointTarget = pingInfo.weakPointObject;
        pingIndicator.forceUseHitOriginForPosition = pingInfo.forceUseHitOriginForPosition;

        pingIndicator.RebuildPing();

        var pingType = pingIndicator.pingType;

        _painter.SetPingIndicatorColor(pingIndicator, pingType);
        _textBuilder.SetPingText(pingIndicator, pingType);
        
        var duration = GetPingDuration(pingType);
        pingIndicator.fixedTimer = duration;

        // Add the ping indicator to our own list
        _pingIndicators.Add(pingIndicator);

        if (pingerController.hasAuthority)
        {
            pingerController.CallCmdPing(pingerController.currentPing);
        }
    }

    private bool TryUnping(PingerController pingerController, PingerController.PingInfo pingInfo)
    {
        var pingedGameObj = pingInfo.targetGameObject;
        
        if (pingedGameObj == null)
        {
            return false;
        }
        
        // Find if our current ping indicators have one targetting the game object
        var curPingIndicator = _pingIndicators.SingleOrDefault(indicator => indicator.pingTarget == pingedGameObj);
        
        if (curPingIndicator == null)
        {
            return false;
        }
        
        // Destroy it if so
        Object.Destroy(curPingIndicator.gameObject);

        if (pingerController.hasAuthority) {
            pingerController.CallCmdPing(pingInfo);
        }

        return true;
    }
    
    private float GetPingDuration(PingIndicator.PingType pingType)
    {
        float duration = 0f;

        switch (pingType)
        {
            case PingIndicator.PingType.Default:
                duration = _config.DefaultLifetime.Value;
                break;
            case PingIndicator.PingType.Enemy:
                duration = _config.EnemyLifetime.Value;
                break;
            case PingIndicator.PingType.Interactable:
                duration = _config.InteractableLifetime.Value;
                break;
        }

        if (duration <= 0f)
        {
            duration = float.PositiveInfinity;
        }
            
        return duration;
    }
}