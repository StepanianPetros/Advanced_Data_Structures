using ConsoleApp1;

var trie = new Trie();

trie.Insert("aac");
trie.Insert("aac");
trie.Insert("aac");
trie.Insert("aac");
trie.Insert("aac");
trie.Insert("aaa");
trie.Insert("aaa");
trie.Insert("aba");
trie.Insert("aba");
trie.Insert("aba");
trie.Insert("abaa");
trie.Insert("a");

trie.PrintPrefix("a");