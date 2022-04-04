using DevExpress.Data.Filtering;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using System.Collections;
using System.Linq;

namespace RetailTradeServer.Components
{
    public class CustomGridControl : GridControl
    {
        private ObservableCollectionCore<object> _seletedItems;

        public IList MySelectedItems => _seletedItems;

        public CustomGridControl()
        {
            SelectionChanged += CustomGridControl_SelectionChanged;
            _seletedItems = new();
        }

        Hashtable selection = new();
        IEnumerable OrderedSelection => selection.Keys.Cast<int>().OrderBy(x => x);
        protected override void OnItemsSourceChanged(object oldValue, object newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            selection.Clear();
            if (newValue is not IEnumerable itemsSource)
                return;
            int i = 0;
            foreach (object item in itemsSource)
                selection[i++] = false;
        }

        Locker updateLocker = new();
        private void CustomGridControl_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (updateLocker.IsLocked) return;
            for (int i = 0; i < VisibleRowCount; i++)
            {
                int rowHandle = GetRowHandleByVisibleIndex(i);
                selection[GetListIndexByRowHandle(rowHandle)] = View.IsRowSelected(rowHandle);
            }
            _seletedItems.BeginUpdate();
            _seletedItems.Clear();
            foreach (int index in OrderedSelection)
            {
                if ((bool)selection[index])
                    _seletedItems.Add(GetRowByListIndex(index));
            }
            _seletedItems.EndUpdate();
        }

        protected override void ApplyFilter(CriteriaOperator op, FilterGroupSortChangingEventArgs filterSortArgs, bool skipIfFilterEquals)
        {
            updateLocker.DoLockedAction(() => {
                base.ApplyFilter(op, filterSortArgs, skipIfFilterEquals);
                BeginSelection();
                foreach (int index in OrderedSelection)
                {
                    if ((bool)selection[index])
                        SelectItem(GetRowHandleByListIndex(index));
                }
                EndSelection();
            });
        }
    }
}
