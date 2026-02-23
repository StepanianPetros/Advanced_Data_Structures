using System;
using System.Collections.Generic;

namespace ConsoleApp1;

public sealed class Trie
{
    private sealed class Node
    {
        public readonly Dictionary<char, Node> Children = new();
        public bool IsEnd;
        public int EndCount;
    }

    private readonly Node _root = new();

    // O(L)
    public void Insert(string word)
    {
        Node cur = _root;

        for (int i = 0; i < word.Length; i++)
        {
            char ch = word[i];

            if (!cur.Children.TryGetValue(ch, out Node next))
            {
                next = new Node();
                cur.Children[ch] = next;
            }

            cur = next;
        }

        cur.IsEnd = true;
        cur.EndCount++;
    }

    public List<(string Word, int Count)> Suggest(string prefix, int limit = 10)
    {
        Node cur = _root;

        for (int i = 0; i < prefix.Length; i++)
        {
            char ch = prefix[i];

            if (!cur.Children.TryGetValue(ch, out Node next))
                return new List<(string Word, int Count)>();

            cur = next;
        }

        var results = new List<(string Word, int Count)>();
        var buffer = new List<char>(prefix.Length + 16);

        for (int i = 0; i < prefix.Length; i++)
            buffer.Add(prefix[i]);

        DfsCollect(cur, buffer, results);
        Sort(results);

        if (limit < results.Count)
            results.RemoveRange(limit, results.Count - limit);

        return results;
    }

    // O(P)
    public void PrintPrefix(string prefix, int limit = 10)
    {
        var list = Suggest(prefix, limit);

        for (int i = 0; i < list.Count; i++)
            Console.WriteLine(list[i].Word + " (" + list[i].Count + ")");
    }

    
    // O(N)
    private static void DfsCollect(Node node, List<char> buf, List<(string Word, int Count)> results)
    {
        if (node.IsEnd)
            results.Add((ToString(buf), node.EndCount));

        foreach (var kv in node.Children)
        {
            buf.Add(kv.Key);
            DfsCollect(kv.Value, buf, results);
            buf.RemoveAt(buf.Count - 1);
        }
    }

    private static string ToString(List<char> chars)
    {
        char[] arr = new char[chars.Count];
        for (int i = 0; i < chars.Count; i++)
            arr[i] = chars[i];

        return new string(arr);
    }

    // O(N^2)
    private static void Sort(List<(string Word, int Count)> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            var key = list[i];
            int j = i - 1;

            while (j >= 0 && Compare(key, list[j]) < 0)
            {
                list[j + 1] = list[j];
                j--;
            }

            list[j + 1] = key;
        }
    }

    private static int Compare((string Word, int Count) x, (string Word, int Count) y)
    {
        if (x.Count != y.Count)
            return y.Count - x.Count;

        return string.CompareOrdinal(x.Word, y.Word);
    }
}