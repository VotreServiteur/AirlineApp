using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SortingService
{
    public class SortingListView
    {
        private static GridViewColumnHeader? listViewSortCol;
        private static SortAdorner? listViewSortAdorner;

        public static void Sorting(GridViewColumnHeader column, ListView lvPlanes)
        {
            var sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol)?.Remove(listViewSortAdorner);
                lvPlanes.Items.SortDescriptions.Clear();
            }

            var newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol)?.Add(listViewSortAdorner);
            lvPlanes.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
    }
}