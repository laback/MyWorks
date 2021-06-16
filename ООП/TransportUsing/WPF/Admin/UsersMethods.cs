using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TransportUsing.Entities;

namespace WPF.Admin
{
    public partial class AdminWindow
    {
        private void UsersGridInitial()
        {
            UserList = (List<User>)_unitOfWork.Users.Get();
            UsersGrid.AutoGenerateColumns = false;
            UsersGrid.SelectionMode = DataGridSelectionMode.Single;
            UsersGrid.ItemsSource = UserList;

            userTypeCB.DisplayMemberPath = "TypeName";
            userTypeCB.SelectedValuePath = "TypeId";
            userTypeCB.ItemsSource = _unitOfWork.UserTypes.Get();
        }

        private void DeleteUser()
        {
            User duser = (User)UsersGrid.SelectedItem;
            if (duser.UserId == _user.UserId)
                MessageBox.Show("Невозможно удалить себя");
            else
                _unitOfWork.Users.Delete(duser.UserId);
        }

        private void AddUser()
        {
            int count = _unitOfWork.Users.Get().Count();
            string message = "Пользователи с логинами :";
            for (int i = count; i < UserList.Count; i++)
            {
                if (CheckLoginUnique(UserList[i]))
                    _unitOfWork.Users.Create(UserList[i]);
                else
                    message += " " + UserList[i].UserName;
            }
            if (!message.Equals("Пользователи с логинами :"))
                MessageBox.Show(message + " уже существуют.");
        }

        private void UpdateUser()
        {
            string message = "Пользователи с логинами :";
            foreach (User u in UserList)
            {
                if (u.TypeId != _user.TypeId && u.UserId == _user.UserId)
                    MessageBox.Show("Невозможно изменить роль себе");
                else if(CheckLoginUnique(u))
                {
                    var routes = _unitOfWork.Routes.Get().Where(x => x.UserId == u.UserId && u.TypeId != 3);
                    foreach(var route in routes)
                    {
                        _unitOfWork.Routes.Delete(route.RouteId);
                    }
                    _unitOfWork.Users.Update(u);
                }
                else 
                    message += " " + u.UserName;
            }
            if (!message.Equals("Пользователи с логинами :"))
                MessageBox.Show(message + " уже существуют.");
        }

        private bool CheckLoginUnique(User user)
        {
            bool isUnique = true;
            List<User> users = (List<User>)_unitOfWork.Users.Get();
            foreach(User u in users)
            {
                if(u.UserName.Equals(user.UserName) && u.UserId != user.UserId)
                {
                    isUnique = false;
                    break;
                }
            }
            return isUnique;
        }
    }
}
