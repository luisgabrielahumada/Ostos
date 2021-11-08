using System.Collections.Generic;

namespace Common.Shared
{
    public class MenuOptions
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public int Id { get; set; }
        public List<Items> Items { get; set; }
    }

    public class Items
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Path { get; set; }
        public int Root { get; set; }
        public string Icon { get; set; }
        public int SourcePrime { get; set; }
    }


    public class RootLinks
    {
        public int Root { get; set; }
        public int Id { get; set; }
        public string Header { get; set; }
        public string Link { get; set; }
        public string IconName { get; set; }
        public string Index { get; set; }
        public bool Active { get; set; }
        public int SourcePrime { get; set; }
        public List<RootLinks> ChildrenLinks { get; set; }
    }
}
