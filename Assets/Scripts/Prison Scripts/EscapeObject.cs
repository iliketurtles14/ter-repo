using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EscapeObject
{
    public List<int> items;
    public List<string> messages;
    public List<string> headers;
    public EscapeObject(List<int> items, List<string> messages, List<string> headers)
    {
        this.items = items;
        this.messages = messages;
        this.headers = headers;
    }
}
