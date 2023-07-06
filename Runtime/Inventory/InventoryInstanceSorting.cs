using System;
using System.Collections.Generic;

namespace CleverCrow.Fluid.ElasticInventory {
    public enum ItemOrder {
        Ascending,
        Descending,
    }

    public enum ItemSort {
        // Skip sorting
        None,

        // Uses the entry's date created field
        CreatedAt,

        // Uses the entry's date updated field
        UpdatedAt,

        // Sorts by display name
        Alphabetical,

        // Sort by category name, override with the customCategory field to assign categories to specific sort orders
        Category,
    }

    public class CategorySort {
        public string Category { get; }
        public int Order { get; }

        public CategorySort (string category, int order) {
            Category = category;
            Order = order;
        }
    }

    public partial class InventoryInstance {
        /// <summary>
        /// Optimally sorts the passed list of items in place. Defaults to sorting by date created then alphabetical in ascending order.
        /// This is so matching date timestamps never get out of order.
        /// </summary>
        /// <param name="items">What should be sorted</param>
        /// <param name="sort">Primary sorting order</param>
        /// <param name="order">Ascending or descending</param>
        /// <param name="sortSecondary">Perform extra sorting on matching items with a matching sort key</param>
        /// <param name="orderSecondary">Ascending or descending</param>
        /// <param name="customCategory">Allows you to provide a custom configuration for sorting categories. Providing this will override all usage of the ItemSort.Category usage</param>
        public void Sort (
            List<IItemEntryReadOnly> items,
            ItemSort sort = ItemSort.CreatedAt,
            ItemOrder order = ItemOrder.Ascending,
            ItemSort sortSecondary = ItemSort.Alphabetical,
            ItemOrder orderSecondary = ItemOrder.Ascending,
            List<CategorySort> customCategory = null
        ) {
            items.Sort((a, b) => {
                var compare = QuickCompare(a, b, sort, order, customCategory);
                if (compare == 0) {
                    return QuickCompare(a, b, sortSecondary, orderSecondary, customCategory);
                }

                return compare;
            });
        }

        private int QuickCompare (
            IItemEntryReadOnly a,
            IItemEntryReadOnly b,
            ItemSort sort,
            ItemOrder order,
            List<CategorySort> customCategory
        ) {
            int result;
            switch (sort) {
                case ItemSort.CreatedAt:
                    result = a.CreatedAt.CompareTo(b.CreatedAt);
                    break;
                case ItemSort.UpdatedAt:
                    result = a.UpdatedAt.CompareTo(b.UpdatedAt);
                    break;
                case ItemSort.Alphabetical:
                    result = string.Compare(a.Definition.DisplayName, b.Definition.DisplayName,
                        StringComparison.Ordinal);
                    break;
                case ItemSort.Category:
                    if (customCategory != null) {
                        var aCategory = customCategory.FindIndex(c => c.Category == a.Definition.Category);
                        var bCategory = customCategory.FindIndex(c => c.Category == b.Definition.Category);
                        result = aCategory.CompareTo(bCategory);
                        break;
                    }

                    result = string.Compare(a.Definition.Category, b.Definition.Category, StringComparison.Ordinal);
                    break;
                default:
                    result = 0;
                    break;
            }

            return order == ItemOrder.Ascending ? result : -result;
        }
    }
}
