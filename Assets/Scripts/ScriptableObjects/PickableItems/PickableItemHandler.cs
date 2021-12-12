﻿using UnityEngine;
using Zenject;

public abstract class PickableItemHandler : ItemHandler, IClickable
{
    [SerializeField] protected PickableIte_SO _pickableItem_SO;

    [Inject] protected readonly PickableItemsInventory _pickableItemsInventory;

    public virtual bool ShouldItemNotBeUsed => false;
    public override Item_SO Item => _pickableItem_SO;

    public virtual void Use() { }

    public override void Equip()
    {
        _pickableItemsInventory.Add(this);
    }

    public virtual void Clicked(int slotIndex)
    {
        if (!ShouldItemNotBeUsed) { return; }

        Use();
        _pickableItemsInventory.Remove(slotIndex);
    }
}
