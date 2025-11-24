using Content.Server.GameTicking;
using Content.Server.Light.EntitySystems;
using Content.Shared._starcup.Light;
using Content.Shared.Light.Components;
using Content.Shared.Light.EntitySystems;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server._starcup.Light;

/// <summary>
/// Simulates dusk-to-dawn light fixtures.
/// </summary>
public sealed class AmbientSensingLightSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly PoweredLightSystem _poweredLight = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly GameTicker _gameTicker = default!;

    private EntityQuery<LightCycleComponent> _lightCycleQuery;

    public override void Initialize()
    {
        base.Initialize();

        _lightCycleQuery = GetEntityQuery<LightCycleComponent>();
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<AmbientSensingLightComponent, PoweredLightComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var ambientSensingLight, out var poweredLight, out var transform))
        {
            if (!_lightCycleQuery.TryComp(transform.MapUid, out var lightCycle))
                continue;

            var time = _timing.CurTime.Add(lightCycle.Offset).Subtract(_gameTicker.RoundStartTimeSpan).TotalSeconds;
            var ambientLightLevel = SharedLightCycleSystem.CalculateLightLevel(lightCycle, (float) time);
            var shouldBeEnabled = ambientLightLevel < ambientSensingLight.LightLevelThreshold;
            var shouldToggle = poweredLight.On ^ shouldBeEnabled;

            if (!shouldToggle)
            {
                ambientSensingLight.StateChangeOffset = TimeSpan.Zero;
                continue;
            }

            if (ambientSensingLight.StateChangeOffset == TimeSpan.Zero)
            {
                ambientSensingLight.StateChangeOffset = _timing.CurTime + _robustRandom.Next(TimeSpan.FromSeconds(5));
            }
            else if (_timing.CurTime >= ambientSensingLight.StateChangeOffset)
            {
                _poweredLight.SetState(uid, shouldBeEnabled, poweredLight);
                ambientSensingLight.StateChangeOffset = TimeSpan.Zero;
            }
        }
    }
}
