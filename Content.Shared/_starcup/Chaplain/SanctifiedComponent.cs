using Content.Shared.Damage;
using Robust.Shared.Audio;

namespace Content.Shared._starcup.Chaplain;

[RegisterComponent]
public sealed partial class SanctifiedComponent : Component
{
    [DataField]
    public EntityUid? OwnerUid;

    [DataField]
    public EntityUid? HealingActionUid;

    /// <summary>
    /// Damage that will be healed on a success
    /// </summary>
    [DataField(required: true)]
    public DamageSpecifier Damage = default!;

    [DataField]
    public SoundSpecifier HealSound = new SoundPathSpecifier("/Audio/Effects/holy.ogg");
}
