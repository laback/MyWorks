using KindergartenWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KindergartenWebApp.Models;
using System.Data.Entity;

namespace KindergartenWebApp
{
    public partial class CodeStaffs : System.Web.UI.Page
    {
        private KindergartenContext _db = new KindergartenContext();
        private string strFindStaff = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                strFindStaff = TextBoxFindStaff.Text;
                ShowData(strFindStaff);
            }

        }

        protected void GridViewStaff_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewStaff.EditIndex = e.NewEditIndex;
            ShowData(strFindStaff);

        }


        protected void GridViewStaff_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            GridViewRow row = GridViewStaff.Rows[e.RowIndex];
            int id = Convert.ToInt32(((TextBox)(row.Cells[1].Controls[0])).Text);
            Staff staff = _db.Staffs.Where(f => f.Id == id).FirstOrDefault();
            staff.FullName = e.NewValues["FullName"].ToString();
            staff.Adress = e.NewValues["Adress"].ToString();
            staff.Phone = Convert.ToInt32(e.NewValues["Phone"].ToString());
            staff.PositionId = int.Parse(e.NewValues["PositionId"].ToString());
            staff.Info = e.NewValues["Info"].ToString();
            staff.Reward = e.NewValues["Reward"].ToString();
            _db.SaveChanges();
            GridViewStaff.EditIndex = -1;

            ShowData(strFindStaff);

        }

        protected void GridViewStaff_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridViewStaff.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.Cells[1].Text);
            Staff staff = _db.Staffs.Where(f => f.Id == id).FirstOrDefault();
            _db.Staffs.Remove(staff);

            _db.SaveChanges();
            GridViewStaff.EditIndex = -1;

            ShowData(strFindStaff);

        }


        protected void GridViewStaff_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewStaff.EditIndex = 1;
            ShowData(strFindStaff);
        }


        protected void ButtonFindStaff_Click(object sender, EventArgs e)
        {
            strFindStaff = TextBoxFindStaff.Text;
            ShowData(strFindStaff);
        }

        protected void ButtonAddStaff_Click(object sender, EventArgs e)
        {
            string fullName = TextBoxFullName.Text;
            string adress = TextBoxAdress.Text;
            int phone = int.Parse(TextBoxPhone.Text);
            int positionId = int.Parse(PositionDropDownList.SelectedValue);
            string info = TextBoxInfo.Text;
            string reward = TextBoxReward.Text;
            Staff staff = new Staff
            {
                FullName = fullName,
                Adress = adress,
                Phone = phone,
                PositionId = positionId,
                Info = info,
                Reward = reward
            };

            _db.Staffs.Add(staff);
            _db.SaveChanges();
            TextBoxFullName.Text = "";
            TextBoxAdress.Text = "";
            TextBoxPhone.Text = "";
            TextBoxInfo.Text = "";
            TextBoxReward.Text = "";
            ShowData(strFindStaff);
        }
        protected void GridViewStaff_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewStaff.PageIndex = e.NewPageIndex;
            ShowData(strFindStaff);

        }
        protected void ShowData(string strFindStaff = "")
        {

            List<Staff> staffs = _db.Staffs.Include(s => s.Position).Where(s => s.FullName.Contains(strFindStaff)).ToList();
            GridViewStaff.DataSource = staffs;
            GridViewStaff.DataBind();
        }
    }
}