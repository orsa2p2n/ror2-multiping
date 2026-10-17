using RoR2;
using RoR2.UI;
using UnityEngine;

namespace Multiping;

public class PingIndicatorManager
{
    private readonly MultipingConfig _config;
        
    public PingIndicatorManager(Multiping plugin)
    {
        _config = plugin.GetConfig();
    }

    // --- HOOKS ---

    public void On_PingIndicator_Update(On.RoR2.UI.PingIndicator.orig_Update orig, PingIndicator pingIndicator)
    {
        UpdatePingIndicator(pingIndicator);
        orig(pingIndicator);
    }
    
    // --- LOGIC ---
    
    private void UpdatePingIndicator(PingIndicator pingIndicator)
    {
        UpdatePingText(pingIndicator);
        CheckPingDestruction(pingIndicator);
    }

    private void UpdatePingText(PingIndicator pingIndicator)
    {
        LocalUser localUser = LocalUserManager.GetFirstLocalUser();
        
        if (localUser == null)
        {
            return;
        }

        bool showPingDistance = _config.ShowPingDistance.Value;
        
        if (showPingDistance && localUser.cachedBody != null)
        {
            var origin = localUser.cachedBody.footPosition;
            var pingPos = pingIndicator.transform.position;
            var distance = Vector3.Distance(origin, pingPos);
            
            int index = pingIndicator.pingText.text.IndexOf((char)0x200B);
            string sub = index >= 0 ? pingIndicator.pingText.text.Substring(0, index) : pingIndicator.pingText.text;
            
            pingIndicator.pingText.text = sub + (char) 0x200B + $" ({distance:0.0}m)";
        }

        bool hideOffscreenText = _config.HideOffscreenPingText.Value;
        
        if (hideOffscreenText)
        {
            var isObjectVisible = localUser.cameraRigController.sceneCam.IsObjectVisible(pingIndicator.transform);
            var textAlpha = isObjectVisible ? 1 : 0;
            pingIndicator.pingText.alpha = textAlpha;
        }
    }

    private void CheckPingDestruction(PingIndicator pingIndicator)
    {
        var target = pingIndicator.pingTarget;
        if (target == null)
        {
            return;
        }
        
        BarrelInteraction barrelInteraction = pingIndicator.pingTarget.GetComponent<BarrelInteraction>();
        
        if (!barrelInteraction)
        {
            return;
        }
        
        if (barrelInteraction.Networkopened)
        {
            Object.Destroy(pingIndicator.gameObject);
        }
    }
}