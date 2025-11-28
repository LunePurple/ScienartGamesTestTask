#nullable enable

public class Inventory<T> where T : class
{
    private readonly T?[] _slots;

    public Inventory(int slotsNumber)
    {
        _slots = new T?[slotsNumber];
    }

    public bool TryGetFromSlot(int slotNumber, out T? item)
    {
        item = null;
        if (slotNumber < 0 || _slots.Length <= slotNumber) return false;

        item = _slots[slotNumber];
        return item is not null;
    }

    public bool TryRemoveFromSlot(int slotNumber)
    {
        if (slotNumber < 0 || _slots.Length <= slotNumber) return false;
        if (_slots[slotNumber] is null) return false;
        
        _slots[slotNumber] = null;
        return true;
    }
    
    public bool TryPickupItem(T item)
    {
        if (HasEmptySlot(out int emptySlotIndex))
        {
            _slots[emptySlotIndex] = item;
            return true;
        }

        return false;
    }

    public bool HasEmptySlot(out int emptySlotIndex)
    {
        emptySlotIndex = -1;
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] is not null) continue;

            emptySlotIndex = i;
            return true;
        }

        return false;
    }
}