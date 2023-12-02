using System.Collections;
using System.Collections.Generic;

public class BookNameHashSet
{
    private HashSet<string> _values = new HashSet<string>();

    public bool AddValue(string value)
    {
        return _values.Add(value);
    }

    public bool RemoveValue(string value)
    {
        return _values.Remove(value);
    }

    public bool Contains(string value)
    {
        return _values.Contains(value);
    }
    

}
