using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebProject.Entites;

namespace WebProject.Models
{
    public static class TreeViewModel
    {
        public static List<Category> GetCategoryChierarchicalTree(IEnumerable<Category> allCats, string parentId = null)
        {
            return allCats.Where(c => c.ParentCategoryID == parentId)
                            .Select(c => new Category()
                            {
                                ID = c.ID,
                                Name = c.Name,
                                ParentCategory = c.ParentCategory,
                                Slug = c.Slug,
                                ParentCategoryID = c.ParentCategoryID,
                                Level = c.Level,
                                DisplayOrder = c.DisplayOrder,
                                IsFeatured = c.IsFeatured,
                                CategoryChildrens = GetCategoryChierarchicalTree(allCats.ToList(), c.ID),

                            })
                            .ToList();
        }

        public static void CreateTreeViewCategorySeleteItems(IEnumerable<Category> commodityGroups
                                             , List<Category> des,
                                              int leve)
        {

            foreach (var category in commodityGroups)
            {
                string perfix = string.Concat(Enumerable.Repeat("-", leve));

                des.Add(new Category()
                {
                    ID = category.ID,
                    Name = perfix + category.Name
                });

                if (category.CategoryChildrens?.Count > 0)
                {

                    CreateTreeViewCategorySeleteItems(category.CategoryChildrens, des, leve + 1);

                }
            }

        }


        
    }
}
