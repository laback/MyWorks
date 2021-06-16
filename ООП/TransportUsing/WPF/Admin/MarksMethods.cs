using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TransportUsing.Entities;

namespace WPF.Admin
{
    public partial class AdminWindow
    {
        private void MarksGridInitial()
        {
            MarkList = (List<Mark>)_unitOfWork.Marks.Get();
            MarksGrid.AutoGenerateColumns = false;
            MarksGrid.SelectionMode = DataGridSelectionMode.Single;
            MarksGrid.ItemsSource = MarkList;
        }

        private void DeleteMark()
        {
            Mark mark = (Mark)MarksGrid.SelectedItem;
            _unitOfWork.Marks.Delete(mark.MarkId);
        }

        private void AddMark()
        {
            int count = _unitOfWork.Marks.Get().Count();
            for(int i = count; i < MarkList.Count; i++)
            {
                _unitOfWork.Marks.Create(MarkList[i]);
            }
        }

        private void UpdateMark()
        {
            foreach(Mark mark in MarkList)
                _unitOfWork.Marks.Update(mark);
        }
    }
}
