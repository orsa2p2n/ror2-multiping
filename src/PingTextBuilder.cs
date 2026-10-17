using System.Text;
using RoR2;
using UnityEngine;

namespace Multiping;

public class PingTextBuilder
{
    private static MultipingConfig _config;

    public PingTextBuilder(MultipingConfig config)
    {
        _config = config;
    }

    public void SetPingText(RoR2.UI.PingIndicator pingIndicator, RoR2.UI.PingIndicator.PingType pingType)
    {
        switch (pingType)
        {
            case RoR2.UI.PingIndicator.PingType.Default:
                break;
            
            case RoR2.UI.PingIndicator.PingType.Enemy:
                AddEnemyText(pingIndicator);
                break;
            
            case RoR2.UI.PingIndicator.PingType.Interactable:
                AddLootText(pingIndicator);
                break;
        }
    }
    
    private void AddEnemyText(RoR2.UI.PingIndicator pingIndicator)
    {
        const string textStart = "<size=70%>\n";
        string name = Util.GetBestBodyName(pingIndicator.pingTarget);

        if (_config.ShowEnemyText.Value) pingIndicator.pingText.text += $"{textStart}{name}";
    }
    
    private void AddLootText(RoR2.UI.PingIndicator pingIndicator)
    {
        const string textStart = "<size=70%>\n";
        string price = GetPrice(pingIndicator.pingTarget);
        
        ShopTerminalBehavior shopTerminal = pingIndicator.pingTarget.GetComponent<ShopTerminalBehavior>();
        
        if (shopTerminal && _config.ShowShopText.Value)
        {
            string text = textStart;
            PickupIndex pickupIndex = shopTerminal.CurrentPickup().pickupIndex;
            PickupDef pickup = PickupCatalog.GetPickupDef(pickupIndex);
            text += shopTerminal.pickupIndexIsHidden
                ? "?"
                : $"{Language.GetString(pickup.nameToken)}";
            pingIndicator.pingText.text += $"{text} ({price})";
            return;
        }

        GenericPickupController pickupController = pingIndicator.pingTarget.GetComponent<GenericPickupController>();
        if (pickupController && _config.ShowPickupText.Value)
        {
            PickupDef pickup = PickupCatalog.GetPickupDef(pickupController.pickup.pickupIndex);
            pingIndicator.pingText.text += $"{textStart}{Language.GetString(pickup.nameToken)}";
        }

        ChestBehavior chest = pingIndicator.pingTarget.GetComponent<ChestBehavior>();
        if (chest && _config.ShowChestText.Value)
        {
            pingIndicator.pingText.text += $"{textStart}{Util.GetBestBodyName(pingIndicator.pingTarget)} ({price})";
            return;
        }

        string name = "";

        PurchaseInteraction purchaseInteraction = pingIndicator.pingTarget.GetComponent<PurchaseInteraction>();
        if (purchaseInteraction)
        {
            name = Language.GetString(purchaseInteraction.displayNameToken);
        }

        // Drones
        SummonMasterBehavior summonMaster = pingIndicator.pingTarget.GetComponent<SummonMasterBehavior>();
        if (summonMaster && _config.ShowDroneText.Value)
        {
            pingIndicator.pingText.text += $"{textStart}{name} ({price})";
            return;
        }

        if (_config.ShowShrineText.Value) pingIndicator.pingText.text += $"{textStart}{name}";
    }
    
    private string GetPrice(GameObject go)
    {
        PurchaseInteraction purchaseInteraction = go.GetComponent<PurchaseInteraction>();
        if (purchaseInteraction)
        {
            StringBuilder sb = new StringBuilder();
            CostTypeCatalog.GetCostTypeDef(purchaseInteraction.costType)
                .BuildCostStringStyled(purchaseInteraction.cost, sb, true);

            return sb.ToString();
        }

        return null;
    }
}