﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(WeaponReloadCoroutineUser))]
public class WeaponReload : WeaponAction
{
    [Inject] private readonly PickableItemsInventory _pickableItemsInventory;
    [Inject] private readonly WeaponReloadCoroutineUser _weaponReloadCoroutineUser;

    private IEnumerable<AmmoHandler> _inventoryAmmoEnumarable;
    private int _calculatedClipAmmo;

    public Action OnReloadStarted { get; set; }

    public void ActivateReload()
    {
        if (_weaponHandler.ClipAmmo == _weaponHandler.Weapon_SO.clipMaxAmmo
            || _weaponHandler.Ammo == 0
            || _weaponSlot.ItemActionMaker.IsItemActionGoing) { return; }

        _weaponReloadCoroutineUser.StartAction();
    }

    public IEnumerator Reload()
    {
        _weaponSlot.ItemActionMaker.StartInterruptingItemAction(_weaponReloadCoroutineUser, _weaponHandler.Weapon_SO.reloadSound);

        AddClipAmmoToInventoryAmmo();
        _weaponHandler.ClipAmmo = 0;

        OnReloadStarted?.Invoke();

        yield return _weaponHandler.Weapon_SO.reloadTimeout;

        CalculateCurrentClipAmmo();

        _weaponHandler.ClipAmmo = _calculatedClipAmmo;
        _weaponReloadCoroutineUser.IsActionGoing = false;
    }

    private void CalculateCurrentClipAmmo()
    {
        AmmoHandler[] ammos = GetInventoryAmmo();
        int clipAmmo = _weaponHandler.Weapon_SO.clipMaxAmmo;

        for (int i = 0; i < ammos.Length; i++)
        {
            var currentAmmo = ammos[i].Ammo;
            ammos[i].Ammo -= clipAmmo;

            if (currentAmmo > clipAmmo) { clipAmmo = 0; break; }

            clipAmmo -= currentAmmo;

            if (clipAmmo < 0) { break; }
        }
        _calculatedClipAmmo = _weaponHandler.Weapon_SO.clipMaxAmmo - clipAmmo;
    }

    private AmmoHandler[] GetInventoryAmmo()
    {
        return _inventoryAmmoEnumarable.ToArray();
    }

    private void AddClipAmmoToInventoryAmmo()
    {
        _inventoryAmmoEnumarable = _pickableItemsInventory.Inventory.TakeWhile(item => item != null).Where(item =>
        {
            var ammo = item as AmmoHandler;
            return ammo != null && ammo.Ammo != 0;
        }).Select(item => (AmmoHandler)item);

        var ammo = _inventoryAmmoEnumarable.FirstOrDefault(ammo => ammo.Ammo + _weaponHandler.ClipAmmo <= AmmoHandler.MAX_SLOT_AMMO);
        ammo.Ammo += _weaponHandler.ClipAmmo;
    }
}