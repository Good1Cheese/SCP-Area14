﻿using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
public class WeaponRecoilAnimation : MonoBehaviour
{
    [Inject] private readonly WeaponSlot _weaponSlot;
    [Inject] private readonly Animator _weaponAnimator;
    [Inject] private readonly WeaponAim _weaponAim;

    private void Awake()
    {
        _weaponSlot.Changed += SetWeaponAnimator;
        _weaponAim.FiredWithAim += ActivateRecoilInAim;
        _weaponAim.FiredWithoutAim += ActivateRecoilWithoutAim;
    }

    private void ActivateRecoilInAim()
    {
        //_weaponAnimator.SetTrigger("OnPlayerFiredWithAim");
    }

    private void ActivateRecoilWithoutAim()
    {
        //_weaponAnimator.SetTrigger("OnPlayerFiredWithoutAim");
    }

    private void SetWeaponAnimator(WeaponHandler weaponHandler)
    {
        _weaponAnimator.runtimeAnimatorController = weaponHandler.Weapon_SO.weaponAnimationContoller;
    }

    private void OnDestroy()
    {
        _weaponSlot.Changed -= SetWeaponAnimator;
        _weaponAim.FiredWithAim -= ActivateRecoilInAim;
        _weaponAim.FiredWithoutAim -= ActivateRecoilWithoutAim;
    }
}
