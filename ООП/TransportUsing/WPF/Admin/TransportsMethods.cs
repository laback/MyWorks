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
        private void TransportsGridInitial()
        {
            TransportList = (List<Transport>)_unitOfWork.Transports.Get();
            TransportsGrid.AutoGenerateColumns = false;
            TransportsGrid.SelectionMode = DataGridSelectionMode.Single;
            TransportsGrid.ItemsSource = TransportList;

            transportTypeCB.DisplayMemberPath = "TransportTypeName";
            transportTypeCB.SelectedValuePath = "TransportTypeId";
            transportTypeCB.ItemsSource = _unitOfWork.TransportTypes.Get();

            transportMarkCB.DisplayMemberPath = "MarkName";
            transportMarkCB.SelectedValuePath = "MarkId";
            transportMarkCB.ItemsSource = _unitOfWork.Marks.Get();
        }

        private void DeleteTransport()
        {
            Transport transport = (Transport)TransportsGrid.SelectedItem;
            _unitOfWork.Transports.Delete(transport.TransportId);
        }

        private void AddTransport()
        {
            int count = _unitOfWork.Transports.Get().Count();
            for (int i = count; i < TransportList.Count; i++)
            {
                _unitOfWork.Transports.Create(TransportList[i]);
            }
        }

        private void UpdateTransport()
        {
            foreach (Transport transport in TransportList)
                _unitOfWork.Transports.Update(transport);
        }
    }
}
