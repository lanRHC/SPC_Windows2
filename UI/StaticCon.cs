using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Vision2.UI.DataGridViewF
{
    public static class StCon
    {
        /// <summary>
        /// 为控件DataGridView添加行号
        /// </summary>
        /// <param name="dataGrid"></param>
        public static void AddCount(DataGridView dataGrid, int cont = 1)
        {
            dataGrid.RowHeadersVisible = true;
            dataGrid.RowPostPaint += new DataGridViewRowPostPaintEventHandler(DataGridViewEx_RowPostPaint);
            void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
            {
                if (dataGrid.CurrentCell != null && dataGrid.CurrentCell.OwningColumn is DataGridViewComboBoxColumnEx)
                {
                    DataGridViewComboBoxColumnEx col = dataGrid.CurrentCell.OwningColumn as DataGridViewComboBoxColumnEx;
                    //修改组合框的样式
                    if (col.DropDownStyle != ComboBoxStyle.DropDownList)
                    {
                        ComboBox combo = e.Control as ComboBox;
                        combo.DropDownStyle = col.DropDownStyle;
                        combo.Leave += new EventHandler(combo_Leave);
                    }
                }
                OnEditingControlShowing(e);
            }
            //dataGrid.Leave += combo_Leave;
            /// <summary>
            /// 当焦点离开时，需要将新输入的值加入到组合框的 Items 列表中
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            void combo_Leave(object sender, EventArgs e)
            {
                ComboBox combo = sender as ComboBox;
                if (combo != null)
                {
                    combo.Leave -= new EventHandler(combo_Leave);
                    if (dataGrid.CurrentCell != null && dataGrid.CurrentCell.OwningColumn is DataGridViewComboBoxColumnEx)
                    {
                        DataGridViewComboBoxColumnEx col = dataGrid.CurrentCell.OwningColumn as DataGridViewComboBoxColumnEx;
                        //一定要将新输入的值加入到组合框的值列表中
                        //否则下一步给单元格赋值的时候会报错（因为值不在组合框的值列表中）
                        col.Items.Add(combo.Text);
                        dataGrid.CurrentCell.Value = combo.Text;
                    }
                }
            }
            void DataGridViewEx_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
            {
                string title = (e.RowIndex + cont).ToString();
                Brush bru = Brushes.Black;
                e.Graphics.DrawString(title, dataGrid.DefaultCellStyle.Font,
                    bru, e.RowBounds.Location.X /*+ dataGrid.RowHeadersWidth / 2*/ , e.RowBounds.Location.Y /*+ dataGrid.Rows[e.RowIndex].Height / 2*/ /*- dataGrid.DefaultCellStyle.Font.Height*/);
            }

        }

            /// <summary>
            /// 为控件DataGridView添加行号
            /// </summary>
            /// <param name="dataGrid"></param>
            public static void AddCoun(this DataGridView dataGrid, int cont = 1)
         {
            dataGrid.RowHeadersVisible = true;
            dataGrid.RowPostPaint += new DataGridViewRowPostPaintEventHandler(DataGridViewEx_RowPostPaint);
            void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
            {
                if (dataGrid.CurrentCell != null && dataGrid.CurrentCell.OwningColumn is DataGridViewComboBoxColumnEx)
                {
                    DataGridViewComboBoxColumnEx col = dataGrid.CurrentCell.OwningColumn as DataGridViewComboBoxColumnEx;
                    //修改组合框的样式
                    if (col.DropDownStyle != ComboBoxStyle.DropDownList)
                    {
                        ComboBox combo = e.Control as ComboBox;
                        combo.DropDownStyle = col.DropDownStyle;
                        combo.Leave += new EventHandler(combo_Leave);
                    }
                }
                OnEditingControlShowing(e);
            }
            //dataGrid.Leave += combo_Leave;
            /// <summary>
            /// 当焦点离开时，需要将新输入的值加入到组合框的 Items 列表中
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            void combo_Leave(object sender, EventArgs e)
            {
                ComboBox combo = sender as ComboBox;
                if (combo != null)
                {
                    combo.Leave -= new EventHandler(combo_Leave);
                    if (dataGrid.CurrentCell != null && dataGrid.CurrentCell.OwningColumn is DataGridViewComboBoxColumnEx)
                    {
                        DataGridViewComboBoxColumnEx col = dataGrid.CurrentCell.OwningColumn as DataGridViewComboBoxColumnEx;
                        //一定要将新输入的值加入到组合框的值列表中
                        //否则下一步给单元格赋值的时候会报错（因为值不在组合框的值列表中）
                        col.Items.Add(combo.Text);
                        dataGrid.CurrentCell.Value = combo.Text;
                    }
                }
            }
            void DataGridViewEx_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
            {
                string title = (e.RowIndex + cont).ToString();
                Brush bru = Brushes.Black;
                e.Graphics.DrawString(title, dataGrid.DefaultCellStyle.Font,
                    bru, e.RowBounds.Location.X /*+ dataGrid.RowHeadersWidth / 2*/ , e.RowBounds.Location.Y /*+ dataGrid.Rows[e.RowIndex].Height / 2*/ /*- dataGrid.DefaultCellStyle.Font.Height*/);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="items"></param>
        /// <param name="addComBoxColumnIndex"></param>
        /// <returns></returns>
        public static ComboBox InitComboBox(this DataGridView dataGrid,List<string>  items,int addComBoxColumnIndex,Func<List<string>> action=null)
        {
            System.Windows.Forms.ComboBox comboBox = new System.Windows.Forms.ComboBox();
            comboBox.Items.Clear();
            comboBox.Items.AddRange(items.ToArray());
            comboBox.Leave += new EventHandler(ComboBox_Leave);
            comboBox.SelectedIndexChanged += new EventHandler(ComboBox_TextChanged);
            comboBox.Visible = false;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            dataGrid.Controls.Add(comboBox);
            dataGrid.CurrentCellChanged += DataGridView1_CurrentCellChanged;
            void ComboBox_Leave(object sender, EventArgs e)
            {
                comboBox.Visible = false;
            }
            void ComboBox_TextChanged(object sender, EventArgs e)
            {
                dataGrid.CurrentCell.Value = ((System.Windows.Forms.ComboBox)sender).Text;
                comboBox.Visible = false;
            }
            void DataGridView1_CurrentCellChanged(object sender, EventArgs e)
            {
                try
                {
                    if (dataGrid.CurrentCell==null)
                    {
                        return;
                    }
                    if (dataGrid.CurrentCell.ColumnIndex == addComBoxColumnIndex)
                    {
                        Rectangle rectangle = dataGrid.GetCellDisplayRectangle(dataGrid.CurrentCell.ColumnIndex, dataGrid.CurrentCell.RowIndex, false);
                        if (action!=null)
                        {
                           List<string> vs=      action.Invoke();
                            comboBox.Items.Clear();
                            comboBox.Items.AddRange(vs.ToArray());
                        }
                        if (dataGrid.CurrentCell.Value != null)
                        {
                            string value = dataGrid.CurrentCell.Value.ToString();
                            comboBox.Text = value;
                        }
                        comboBox.Left = rectangle.Left;
                        comboBox.Top = rectangle.Top;
                        comboBox.Width = rectangle.Width;
                        comboBox.Height = rectangle.Height;
                        comboBox.Visible = true;
                    }
                    else
                    {
                        comboBox.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    return;
                }

            }
            return comboBox;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="items"></param>
        /// <param name="addComBoxColumnIndex"></param>
        /// <returns></returns>
        public static ComboBox InitComboBox(this DataGridView dataGrid, List<string> items, DataGridViewColumn addComBoxColumn, Func<List<string>> action = null)
        {
            System.Windows.Forms.ComboBox comboBox = new System.Windows.Forms.ComboBox();
            comboBox.Items.Clear();
            comboBox.Items.AddRange(items.ToArray());
            comboBox.Leave += new EventHandler(ComboBox_Leave);
            comboBox.SelectedIndexChanged += new EventHandler(ComboBox_TextChanged);
            comboBox.Visible = false;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            dataGrid.Controls.Add(comboBox);
            dataGrid.CurrentCellChanged += DataGridView1_CurrentCellChanged;
            void ComboBox_Leave(object sender, EventArgs e)
            {
                comboBox.Visible = false;
            }
            void ComboBox_TextChanged(object sender, EventArgs e)
            {
                dataGrid.CurrentCell.Value = ((System.Windows.Forms.ComboBox)sender).Text;
                comboBox.Visible = false;
            }
            void DataGridView1_CurrentCellChanged(object sender, EventArgs e)
            {
                try
                {
                    if (dataGrid.CurrentCell == null)
                    {
                        return;
                    }
                    if (dataGrid.CurrentCell.OwningColumn == addComBoxColumn)
                    {
                        Rectangle rectangle = dataGrid.GetCellDisplayRectangle(dataGrid.CurrentCell.ColumnIndex, dataGrid.CurrentCell.RowIndex, false);
                        if (action != null)
                        {
                            List<string> vs = action.Invoke();
                            comboBox.Items.Clear();
                            comboBox.Items.AddRange(vs.ToArray());
                        }
                        if (dataGrid.CurrentCell.Value != null)
                        {
                            string value = dataGrid.CurrentCell.Value.ToString();
                            comboBox.Text = value;
                        }
                        comboBox.Left = rectangle.Left;
                        comboBox.Top = rectangle.Top;
                        comboBox.Width = rectangle.Width;
                        comboBox.Height = rectangle.Height;
                        comboBox.Visible = true;
                    }
                    else
                    {
                        comboBox.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    return;
                }

            }
            return comboBox;
        }
        /// <summary>
        /// 双缓冲，解决闪烁问题
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="flag">双缓存</param>
        public static void DoubleBufferedDataGirdView(DataGridView dgv, bool flag)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, flag, null);
        }
    
            /// <summary>
            /// 为控件提供Tooltip
            /// </summary>
            /// <param name="control">控件</param>
            /// <param name="tip">ToolTip</param>
            /// <param name="message">提示消息</param>
            public static void ShowTooltip(this Control control, ToolTip tip, string message)
            {
                Point _mousePoint = Control.MousePosition;
                int _x = control.PointToClient(_mousePoint).X;
                int _y = control.PointToClient(_mousePoint).Y;
                tip.Show(message, control, _x, _y);
                tip.Active = true;
            }
            /// <summary>
            /// 为控件提供Tooltip
            /// </summary>
            /// <param name="control">控件</param>
            /// <param name="tip">ToolTip</param>
            /// <param name="message">提示消息</param>
            /// <param name="durationTime">保持提示的持续时间</param>
            public static void ShowTooltip(this Control control, ToolTip tip, string message, int durationTime)
            {
                Point _mousePoint = Control.MousePosition;
                int _x = control.PointToClient(_mousePoint).X;
                int _y = control.PointToClient(_mousePoint).Y;
                tip.Show(message, control, _x, _y, durationTime);
                tip.Active = true;
            }
            /// <summary>
            /// 为控件提供Tooltip
            /// </summary>
            /// <param name="control">控件</param>
            /// <param name="tip">ToolTip</param>
            /// <param name="message">提示消息</param>
            /// <param name="xoffset">水平偏移量</param>
            /// <param name="yoffset">垂直偏移量</param>
            public static void ShowTooltip(this Control control, ToolTip tip, string message, int xoffset, int yoffset)
            {
                Point _mousePoint = Control.MousePosition;
                int _x = control.PointToClient(_mousePoint).X;
                int _y = control.PointToClient(_mousePoint).Y;
                tip.Show(message, control, _x + xoffset, _y + yoffset);
                tip.Active = true;
            }
            /// <summary>
            /// 为控件提供Tooltip
            /// </summary>
            /// <param name="control">控件</param>
            /// <param name="tip">ToolTip</param>
            /// <param name="message">提示消息</param>
            /// <param name="xoffset">水平偏移量</param>
            /// <param name="yoffset">垂直偏移量</param>
            /// <param name="durationTime">保持提示的持续时间</param>
            public static void ShowTooltip(this Control control, ToolTip tip, string message, int xoffset, int yoffset, int durationTime)
            {
                Point _mousePoint = Control.MousePosition;
                int _x = control.PointToClient(_mousePoint).X;
                int _y = control.PointToClient(_mousePoint).Y;
                tip.Show(message, control, _x + xoffset, _y + yoffset, durationTime);
                tip.Active = true;
            }
        

    }
}