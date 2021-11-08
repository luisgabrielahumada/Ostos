using Core.Component.Library.Json;
using System.Collections.Generic;
using System.Linq;
namespace Common.Shared
{
    public static class ParseJson
    {
        public static List<dynamic> GetJson(this string data)
        {
            List<dynamic> items = new List<dynamic>();
            var temp = data.DeserializeJson<List<MenuOptions>>();
            var root = temp.Select(k=> new RootLinks
            {
                Id = k.Id,
                Root = k.Id,
                Header = k.Name,
                Link = $"/app/{k.Name}",
                IconName = "flaticon-network",
                Index = k.Alias,
                ChildrenLinks = k.Items.GetJsonItems()
            });
            items.AddRange(root);
            return items;
        }


        private static List<RootLinks> GetJsonItems(this List<Items> data)
        {

            if (data == null) return new List<RootLinks>();

            var childrenLinks = data
                    .Where(m => m.Root == 0)
                    .Select(m => new RootLinks
                    {
                        Id = m.Id,
                        Root = m.Root,
                        Header = m.Name,
                        Link = $"{m.Path}",
                        Index = $"{m.Name}-{m.Id.ToString()}",
                        Active = true,
                        IconName = "",
                        SourcePrime = m.SourcePrime,
                        ChildrenLinks = data.GetJsonChildren(m.Id)
                    }).ToList();

            return childrenLinks ?? new List<RootLinks>();
        }

        private static List<RootLinks> GetJsonChildren(this List<Items> data, int root)
        {
            var childrenLinks = data
                    .Where(m => m.Root == root && m.Root > 0)
                    .Select(m => new RootLinks
                    {
                        Id = m.Id,
                        Root = m.Id,
                        Header = m.Name,
                        Link = $"{m.Path}",
                        SourcePrime = m.SourcePrime,
                        Index = $"{m.Name}-{m.Id.ToString()}",
                    }).ToList();


            return childrenLinks ?? new List<RootLinks>();
        }

    }
}
