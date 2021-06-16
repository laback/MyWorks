using KindergartenWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KindergartenWebApp.Models;

namespace KindergartenWebApp
{
    public partial class CodePositions : System.Web.UI.Page
    {
        private KindergartenContext _db = new KindergartenContext();
        private string strFindPosition = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                strFindPosition = TextBoxFindPosition.Text;
                ShowData(strFindPosition);
            }

        }

        protected void GridViewPosition_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewPosition.EditIndex = e.NewEditIndex;
            ShowData(strFindPosition);

        }


        protected void GridViewPosition_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridViewPosition.Rows[e.RowIndex];
            int id = Convert.ToInt32(((TextBox)(row.Cells[1].Controls[0])).Text);
            Position position = _db.Positions.Where(f => f.Id == id).FirstOrDefault();
            position.PositionName = ((TextBox)(row.Cells[2].Controls[0])).Text;
            _db.SaveChanges();
            GridViewPosition.EditIndex = -1;

            ShowData(strFindPosition);

        }

        protected void GridViewPosition_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridViewPosition.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.Cells[1].Text);
            Position Position = _db.Positions.Where(f => f.Id == id).FirstOrDefault();
            _db.Positions.Remove(Position);

            _db.SaveChanges();
            GridViewPosition.EditIndex = -1;

            ShowData(strFindPosition);

        }


        protected void GridViewPosition_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewPosition.EditIndex = 1;
            ShowData(strFindPosition);
        }


        protected void ButtonFindPosition_Click(object sender, EventArgs e)
        {
            strFindPosition = TextBoxFindPosition.Text;
            ShowData(strFindPosition);
        }

        protected void ButtonAddPosition_Click(object sender, EventArgs e)
        {
            string nameOfPosition = TextBoxPositionName.Text ?? "";
            if (nameOfPosition != "")
            {
                Position Position = new Position
                {
                    PositionName = nameOfPosition
                };

                _db.Positions.Add(Position);
                _db.SaveChanges();
                TextBoxPositionName.Text = "";
                ShowData(strFindPosition);

            }


        }
        protected void GridViewPosition_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPosition.PageIndex = e.NewPageIndex;
            ShowData(strFindPosition);

        }
        protected void ShowData(string strFindPosition = "")
        {

            List<Position> Positions = _db.Positions.Where(s => s.PositionName.Contains(strFindPosition)).ToList();
            GridViewPosition.DataSource = Positions;
            GridViewPosition.DataBind();
        }
    }
}