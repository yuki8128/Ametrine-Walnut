using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

public class StackKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>
{
    private readonly Func<TItem, TKey> _keySelector;

    public StackKeyedCollection(Func<TItem, TKey> keySelector)
    {
        _keySelector = keySelector;
    }

    protected override TKey GetKeyForItem(TItem item)
    {
        return _keySelector(item);
    }

    public TItem GetItemForKey(TKey key)
    {
        if (this.Contains(key))
        {
            return this[key];
        }

        throw new KeyNotFoundException($"No item found with the key '{key}'.");
    }


    public void Push(TItem item)
    {
        if (this.Contains(item))
        {
            this.Remove(item);
        }
        this.Add(item);
    }

    public TItem Pop()
    {
        if (this.Count == 0)
        {
            throw new KeyNotFoundException("The stack is empty.");
        }

        var lastItem = this[this.Count - 1];
        this.RemoveAt(this.Count - 1);
        return lastItem;
    }

    public TItem PopForTargetItem(TKey key)
    {
        if (!this.Contains(key))
        {
            throw new KeyNotFoundException($"No item found with the key '{key}'.");
        }

        TItem targetItem = this[key];
        TItem poppedItem = default;

        // 指定されたキーの要素が見つかるまでPopを繰り返す
        while (this.Count > 0)
        {
            poppedItem = Pop();
            if (EqualityComparer<TItem>.Default.Equals(poppedItem, targetItem))
            {
                return poppedItem;
            }
        }

        // ここに到達することはないはずだが、念のため例外をスロー
        throw new InvalidOperationException("Unexpected state in PopForTargetItem.");
    }

    public TItem Peek()
    {
        if (this.Count == 0)
        {
            throw new KeyNotFoundException("The stack is empty.");
        }

        return this[this.Count - 1];
    }
}
