using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace Vision2.UI
{
    public class treeViewEX : TreeView
    {
        public treeViewEX()
        {
            this.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.MouseDown += TreeViewEX_MouseDown;
        }

        private struct imageXY
        {
            public Image Image;
            public int X;
            public int Y;
        }

        private List<imageXY> Images = new List<imageXY>();

        public void AddImage(Image image, int x, int y)
        {
            try
            {
                Images.Add(new imageXY() { Image = image, X = x, Y = y });
            }
            catch (Exception)
            { }
        }

        private void TreeViewEX_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                TreeNode treeNode = this.GetNodeAt(e.Location);
                if (treeNode == null)
                {
                    return;
                }
                if (e.Button == MouseButtons.Left)
                {
                    treeNode.TreeView.SelectedNode = treeNode;
                }
            }
            catch (Exception) { }
        }

        public ImageList ImageList1 = new ImageList();

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            try
            {
                //e.DrawBackground();
                //e.DrawFocusRectangle();
                if ((e.State & TreeNodeStates.Selected) != 0)
                {
                    e.Graphics.FillRectangle(Brushes.Gainsboro, e.Bounds);
                    Font nodeFont = e.Node.NodeFont;
                    if (nodeFont == null) nodeFont = ((TreeView)this).Font;
                    e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.Black,
                      e.Node.Bounds.X, e.Node.Bounds.Y);
                }
                else
                {
                    e.DrawDefault = true;
                }
                if (e.Node.ImageIndex == -1)
                {
                    e.Node.ImageIndex = 0;
                }
                for (int i = 0; i < Images.Count; i++)
                {
                    if (seleIamge == e.Node.Index)
                    {
                        e.Graphics.DrawImage(null,
                            e.Bounds.Width - Images[i].X, e.Node.Bounds.Y + Images[i].Y, e.Bounds.Height, e.Bounds.Height);
                    }
                    else
                    {
                        e.Graphics.DrawImage(Images[i].Image, e.Bounds.Width - Images[i].X, e.Node.Bounds.Y + Images[i].Y,
                            e.Bounds.Height, e.Bounds.Height);
                    }
                }
                if ((e.State & TreeNodeStates.Focused) != 0)
                {
                    using (Pen focusPen = new Pen(Color.Black))
                    {
                        focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        Rectangle focusBounds = e.Node.Bounds;
                        focusBounds.Size = new Size(focusBounds.Width - 1,
                        focusBounds.Height - 1);
                        e.Graphics.DrawRectangle(focusPen, focusBounds);
                    }
                }
            }
            catch (Exception) { }
            base.OnDrawNode(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            TreeNode treeNode = this.GetNodeAt(e.Location);
            for (int i = 0; i < Images.Count; i++)
            {
                if (e.Location.X > this.Width - Images[i].X && e.Location.X < this.Width - Images[i].X + treeNode.Bounds.Height)
                {
                    seleIamge = treeNode.Index;
                    Piamge_Click(this, e);
                    //       e.Graphics.DrawImage(Images[i].Image, e.Bounds.Width - Images[i].X, e.Node.Bounds.Y + Images[i].Y);
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            seleIamge = -1;
            base.OnMouseUp(e);
        }

        [Category("行为")]
        public event EventHandler SelectedValueBunClike;

        private int seleIamge = -1;

        private void Piamge_Click(object sender, MouseEventArgs e)
        {
            try
            {
                SelectedValueBunClike?.Invoke(this, e);

                //Piamge.Image = ListObj[this.IndexFromPoint(Piamge.Location)].ClickButContatin();
            }
            catch (Exception)
            {
            }
        }
    }
}