using System.Collections.Generic;
using RoR2;
using UnityEngine;

namespace Multiping;

enum ColorType
{
    DefaultPingColor,
    DefaultPingSpriteColor,
    EnemyPingColor,
    EnemyPingSpriteColor,
    InteractablePingColor,
    InteractablePingSpriteColor
}

public class PingPainter
{
    private readonly Dictionary<ColorType, Color> _colors = new();
    private readonly bool _tieredInteractablePingColor;

    public PingPainter(MultipingConfig config)
    {
        _colors.Add(ColorType.DefaultPingColor, config.DefaultColor.Value.ToColor());
        _colors.Add(ColorType.DefaultPingSpriteColor, config.DefaultSpriteColor.Value.ToColor());
        _colors.Add(ColorType.EnemyPingColor, config.EnemyColor.Value.ToColor());
        _colors.Add(ColorType.EnemyPingSpriteColor, config.EnemySpriteColor.Value.ToColor());
        _colors.Add(ColorType.InteractablePingColor, config.InteractableColor.Value.ToColor());
        _colors.Add(ColorType.InteractablePingSpriteColor, config.InteractableSpriteColor.Value.ToColor());
        
        _tieredInteractablePingColor = config.ColorInteractablePerTier.Value;
    }
    
    public void SetPingIndicatorColor(RoR2.UI.PingIndicator pingIndicator, RoR2.UI.PingIndicator.PingType pingType)
    {
        SpriteRenderer sprRenderer = new SpriteRenderer();
        Color textColor = Color.black;
        Color spriteColor = Color.black;

        switch (pingType)
        {
            case RoR2.UI.PingIndicator.PingType.Default:
                sprRenderer = pingIndicator.defaultPingGameObjects[0].GetComponent<SpriteRenderer>();
                textColor = _colors[ColorType.DefaultPingColor];
                spriteColor = _colors[ColorType.DefaultPingSpriteColor];
                break;
            
            case RoR2.UI.PingIndicator.PingType.Enemy:
                sprRenderer = pingIndicator.enemyPingGameObjects[0].GetComponent<SpriteRenderer>();
                textColor = _colors[ColorType.EnemyPingColor];
                spriteColor = _colors[ColorType.EnemyPingSpriteColor];
                break;
            
            case RoR2.UI.PingIndicator.PingType.Interactable:
                sprRenderer = pingIndicator.interactablePingGameObjects[0].GetComponent<SpriteRenderer>();
                if (_tieredInteractablePingColor)
                {
                    Color pickupColor = GetTargetTierColor(pingIndicator.pingTarget);
                    textColor = pickupColor;
                    spriteColor = pickupColor;    
                }
                else
                {
                    textColor = _colors[ColorType.InteractablePingColor];
                    spriteColor = _colors[ColorType.InteractablePingSpriteColor];
                }
                break;
        }

        pingIndicator.pingText.color = textColor;
        sprRenderer.color = spriteColor;
    }
    
    private Color GetTargetTierColor(GameObject gameObject)
    {
        Color color = Color.black;
            
        ShopTerminalBehavior shopTerminal = gameObject.GetComponent<ShopTerminalBehavior>();
        if (shopTerminal)
        {
            PickupIndex pickupIndex = shopTerminal.CurrentPickup().pickupIndex;
            PickupDef pickup = PickupCatalog.GetPickupDef(pickupIndex);

            if (pickup != null)
            {
                color = pickup.baseColor;    
            }
        }
            
        GenericPickupController pickupController = gameObject.GetComponent<GenericPickupController>();
        if (pickupController)
        {
            PickupDef pickup = PickupCatalog.GetPickupDef(pickupController.pickup.pickupIndex);
                
            if (pickup != null)
            {
                color = pickup.baseColor;    
            }
        }
            
        PurchaseInteraction purchaseInteraction = gameObject.GetComponent<PurchaseInteraction>();
        if (purchaseInteraction)
        {
            CostTypeDef costType = CostTypeCatalog.GetCostTypeDef(purchaseInteraction.costType);
            color = ColorCatalog.GetColor(costType.colorIndex);
        }

        PickupIndexNetworker pickupIndexNetworker = gameObject.GetComponent<PickupIndexNetworker>();
        if (pickupIndexNetworker)
        {
            PickupDef pickup = PickupCatalog.GetPickupDef(pickupIndexNetworker.NetworkpickupState.pickupIndex);
                
            if (pickup != null)
            {
                color = pickup.baseColor;    
            }
        }

        if (color == Color.black)
        {
            color = _colors[ColorType.InteractablePingColor];
        }

        return color;
    }
}