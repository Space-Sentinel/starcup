using Robust.Shared.GameStates;

namespace Content.Shared._starcup.Light;

/// <summary>
/// Causes a PoweredLight to toggle based on the map's ambient light level. Used for dusk-to-dawn light fixtures.
/// </summary>
[RegisterComponent]
public sealed partial class AmbientSensingLightComponent : Component
{
    /// <summary>
    /// Below this ambient (map) light level, the entity's PointLight will become enabled. It will be disabled above it.
    /// </summary>
    /// <remarks>
    /// Compare this against LightCycleComponent's MinLightLevel and MaxLightLevel.
    /// </remarks>
    [DataField]
    public float LightLevelThreshold = 0.8f;

    /// <summary>
    /// The game time after which this entity will toggle state. This is non-zero when the fixture is pending a state change.
    /// </summary>
    /// <remarks>
    /// Used to stagger light fixture state changes, simulates realism.
    /// </remarks>
    [ViewVariables]
    public TimeSpan StateChangeOffset;
}
