using BepInEx.Configuration;

namespace Multiping;

public class MultipingConfig
{
    // Lifetimes
    public ConfigEntry<int> DefaultLifetime { get; }
    public ConfigEntry<int> EnemyLifetime { get; }
    public ConfigEntry<int> InteractableLifetime { get; }

    // Colors
    public ConfigEntry<string> DefaultColor { get; }
    public ConfigEntry<string> DefaultSpriteColor { get; }
        
    public ConfigEntry<string> EnemyColor { get; }
    public ConfigEntry<string> EnemySpriteColor { get; }
        
    public ConfigEntry<string> InteractableColor { get; }
    public ConfigEntry<string> InteractableSpriteColor { get; }
    public ConfigEntry<bool> ColorInteractablePerTier { get; }

    // Texts
    public ConfigEntry<bool> ShowShopText { get; }
    public ConfigEntry<bool> ShowChestText { get; }
    public ConfigEntry<bool> ShowPickupText { get; }
    public ConfigEntry<bool> ShowDroneText { get; }
    public ConfigEntry<bool> ShowShrineText { get; }
    public ConfigEntry<bool> ShowEnemyText { get; }
        
    public ConfigEntry<bool> ShowPingDistance { get; }
        
    public ConfigEntry<bool> HideOffscreenPingText { get; }
        
    // Notification
    public ConfigEntry<bool> ShowItemNotification { get; }
        
    public MultipingConfig(ConfigFile config)
    {
        DefaultLifetime = config.Bind(
            "Durations",
            "DefaultPingLifetime",
            6,
            "Time in seconds how long a regular 'walk to' ping indicator should be shown on the map, 0 means it never expires"
        );

        EnemyLifetime = config.Bind(
            "Durations",
            "EnemyPingLifetime",
            30,
            "Time in seconds how long a ping indicator for enemies should be shown on the map, 0 means it never expires"
        );

        InteractableLifetime = config.Bind(
            "Durations",
            "InteractablePingLifetime",
            0,
            "Time in seconds how long a ping indicator for interactables should be shown on the map, 0 means it never expires"
        );

        DefaultColor = config.Bind(
            "Colors",
            "DefaultPingColor",
            "#86F57C",
            "Color of the default ping"
        );

        DefaultSpriteColor = config.Bind(
            "SpriteColors",
            "DefaultPingSpriteColor",
            "#86F57C",
            "Color of the default ping sprite"
        );

        EnemyColor = config.Bind(
            "Colors",
            "EnemyPingColor",
            "#D11F1F",
            "Color of the enemy ping"
        );

        EnemySpriteColor = config.Bind(
            "SpriteColors",
            "EnemyPingSpriteColor",
            "#D11F1F",
            "Color of the enemy ping sprite"
        );

        InteractableColor = config.Bind(
            "Colors",
            "InteractablePingColor",
            "#E2DE2C",
            "Color of the interactable ping"
        );

        InteractableSpriteColor = config.Bind(
            "SpriteColors",
            "InteractablePingSpriteColor",
            "#E2DE2C",
            "Color of the interactable ping sprite"
        );

        ShowPickupText = config.Bind(
            "ShowPingText",
            "Pickups",
            true,
            "Shows item names on pickup pings"
        );

        ShowChestText = config.Bind(
            "ShowPingText",
            "Chests",
            false,
            "Shows item names and cost on chest pings"
        );

        ShowShopText = config.Bind(
            "ShowPingText",
            "ShopTerminals",
            false,
            "Shows item names and cost on shop terminal pings"
        );

        ShowDroneText = config.Bind(
            "ShowPingText",
            "Drones",
            true,
            "Shows drone type on broken drone pings"
        );

        ShowShrineText = config.Bind(
            "ShowPingText",
            "Shrines",
            false,
            "Shows shrine type on shrine pings"
        );
            
        ShowEnemyText = config.Bind(
            "ShowPingText",
            "Enemies",
            false,
            "Shows names on enemy pings"
        );

        ShowPingDistance = config.Bind(
            "ShowPingText",
            "Distance",
            false,
            "Show distance to ping in ping label"
        );

        HideOffscreenPingText = config.Bind(
            "ShowPingText",
            "HideOffscreenPingText",
            true,
            "Hide text of offscreen pings to prevent cluttering"
        );

        ShowItemNotification = config.Bind(
            "Notifications",
            "ShowItemNotification",
            true,
            "Show pickup-style notification with description on ping of an already discovered item"
        );

        ColorInteractablePerTier = config.Bind(
            "Colors",
            "TieredInteractablePingColor",
            true,
            "Color pings in their target tier color"
        );
    }
}