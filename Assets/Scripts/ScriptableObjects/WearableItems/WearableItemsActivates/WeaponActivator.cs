using Zenject;

public class WeaponActivator : WearableItemActivator
{
    [Inject] private readonly WeaponSlot _weaponSlot;

    public override WearableSlot WearableItemSlot => _weaponSlot;
}