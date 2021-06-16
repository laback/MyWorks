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
    public partial class CodeGroups : System.Web.UI.Page
    {
        private KindergartenContext _db = new KindergartenContext();
        private string strFindGroup = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                strFindGroup = TextBoxFindGroup.Text;
                ShowData(strFindGroup);
            }

        }

        protected void GridViewGroup_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewGroup.EditIndex = e.NewEditIndex;
            ShowData(strFindGroup);

        }


        protected void GridViewGroup_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            GridViewRow row = GridViewGroup.Rows[e.RowIndex];
            int id = Convert.ToInt32(((TextBox)(row.Cells[1].Controls[0])).Text);
            Group group = _db.Groups.Where(f => f.Id == id).FirstOrDefault();
            group.GroupName = e.NewValues["GroupName"].ToString();
            group.StaffId = int.Parse(e.NewValues["StaffId"].ToString());
            group.CountOfChildren = int.Parse(e.NewValues["CountOfChildren"].ToString());
            group.YearOfCreation = int.Parse(e.NewValues["YearOfCreating"].ToString());
            group.GroupType_Id = int.Parse(e.NewValues["TypeId"].ToString());
            _db.SaveChanges();
            GridViewGroup.EditIndex = -1;

            ShowData(strFindGroup);

        }

        protected void GridViewGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridViewGroup.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.Cells[1].Text);
            Group Group = _db.Groups.Where(f => f.Id == id).FirstOrDefault();
            _db.Groups.Remove(Group);

            _db.SaveChanges();
            GridViewGroup.EditIndex = -1;

            ShowData(strFindGroup);

        }


        protected void GridViewGroup_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewGroup.EditIndex = 1;
            ShowData(strFindGroup);
        }


        protected void ButtonFindGroup_Click(object sender, EventArgs e)
        {
            strFindGroup = TextBoxFindGroup.Text;
            ShowData(strFindGroup);
        }

        protected void ButtonAddGroup_Click(object sender, EventArgs e)
        {
            string groupName = TextBoxNameOfGroup.Text;
            int staffId = int.Parse(StaffDropDownList.SelectedValue);
            int count = int.Parse(TextBoxCount.Text);
            int year = int.Parse(TextBoxYear.Text);
            int typeId = int.Parse(TypeDropDownList.SelectedValue);
            Group group = new Group
            {
                GroupName = groupName,
                StaffId = staffId,
                CountOfChildren = count,
                YearOfCreation = year,
                GroupType_Id = typeId
            };

            _db.Groups.Add(group);
            _db.SaveChanges();
            TextBoxNameOfGroup.Text = "";
            TextBoxCount.Text = "";
            TextBoxYear.Text = "";
            ShowData(strFindGroup);
        }
        protected void GridViewGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewGroup.PageIndex = e.NewPageIndex;
            ShowData(strFindGroup);

        }
        protected void ShowData(string strFindGroup = "")
        {

            List<Group> groups = _db.Groups.Include(g => g.GroupType).Include(g => g.Staff).Where(s => s.GroupName.Contains(strFindGroup)).ToList();
            GridViewGroup.DataSource = groups;
            GridViewGroup.DataBind();
        }
    }
}