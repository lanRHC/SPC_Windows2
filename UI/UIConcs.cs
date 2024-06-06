using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Vision2.UI.DataGridViewF;

namespace Vision2.UI
{
    public class UICon
    {
        /// <summary>
        /// 显示到最上方窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="fAltTab"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        /// <summary>
        /// 将窗口显示在前端
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static bool WindosFormerShow<T>(ref T form) where T : Control
        {
            try
            {
                if (form == null)
                {
                    return false;
                }
                if (form.IsDisposed)
                {
                    Assembly assembly = Assembly.GetAssembly(form.GetType()); // 获取当前程序集
                    dynamic obj = assembly.CreateInstance(form.GetType().ToString()); // 创建类的实例
                    form = (T)obj;
                    form.Show();
                }
                else
                {
                    form.Show();
                    SwitchToThisWindow(form.Handle, true);
                }
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 移动控件
        /// </summary>
        /// <param name="control">拖动控件</param>
        /// <param name="moveControl">被移动控件</param>
        public static void MoveControl(Control control, Control moveControl)
        {
            bool MoveFlag = false;
            int xPos = 0;
            int yPos = 0;

            control.MouseMove += Contlr_MouseMove;
            control.MouseUp += panel1_MouseUp;
            control.MouseDown += panel1_MouseDown;
            void Contlr_MouseMove(object sender, MouseEventArgs e)
            {
                try
                {
                    if (MoveFlag)
                    {
                        moveControl.Left += Convert.ToInt16(e.X - xPos);//设置x坐标.
                        moveControl.Top += Convert.ToInt16(e.Y - yPos);//设置y坐标.
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace);
                }
            }
            void panel1_MouseUp(object sender, MouseEventArgs e)
            {
                MoveFlag = false;
            }
            void panel1_MouseDown(object sender, MouseEventArgs e)
            {
                try
                {
                    MoveFlag = true;//已经按下.
                    xPos = e.X;//当前x坐标.
                    yPos = e.Y;//当前y坐标.
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 绑定数据到控件
        /// </summary>
        /// <param name="ControlName">绑定到控件的属性</param>
        /// <param name="control">控件</param>
        /// <param name="datas">数据</param>
        /// <param name="dataName">数据属性名称</param>
        public static void AddDataBindings(object datas, string dataName, Control control, string ControlName = null,bool clert=true)
        {
            try
            {

                if (ControlName == null)
                {

                    if (control is TextBox)
                    {
                        ControlName = "Text";
                    }
                    else if (control is NumericUpDown)
                    {
                        ControlName = "Value";
                    }
                    else if (control is CheckBox)
                    {
                        ControlName = "Checked";
                    }
                }
                if (clert)
                {
                    control.DataBindings.Clear();
                }
                //Binding bind1 = new Binding(ControlName, datas, dataName, false, DataSourceUpdateMode.OnPropertyChanged);
                control.DataBindings.Add(ControlName, datas, dataName, false, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception EX)
            {
            }
        }


        /// <summary>
        /// 为控件DataGridView添加行号
        /// </summary>
        /// <param name="dataGrid"></param>
        public static void AddCon(DataGridView dataGrid, int cont = 1)
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
                    bru, e.RowBounds.Location.X + /*dataGrid.RowHeadersWidth / 2 -*/ 4, e.RowBounds.Location.Y + dataGrid.Rows[e.RowIndex].Height / 2 - dataGrid.DefaultCellStyle.Font.Height);
            }
        }

        /// <summary>
        /// 双缓冲，解决闪烁问题
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="flag"></param>
        public static void DoubleBufferedDataGirdView(DataGridView dgv, bool flag)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, flag, null);
        }
  

        private Hashtable _HashUISizeKnob;
        private Control _Owner = new Control();

        private Point downPos = new Point(0, 0);
        private Graphics g = null;
        private bool isDown = false;
        private Point upPos = new Point(100, 100);

        private bool isShift;



        ///<summary>负责控件移动的类</summary>
        private Hashtable _HashUIMoveKnob;

        private List<Control> ListUi = new List<Control>();

 

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (g == null)
                return;
            g.DrawRectangle(new Pen(Color.Blue, 1), new Rectangle(downPos, new Size(upPos.X - downPos.X, upPos.Y - downPos.Y)));
        }

        #region 截取屏幕图像

        /// <summary>
        /// 截取屏幕图像
        /// </summary>
        /// <returns></returns>
        public static Bitmap GetScreenCapture()
        {
            Rectangle tScreenRect = new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Bitmap tSrcBmp = new Bitmap(tScreenRect.Width, tScreenRect.Height); // 用于屏幕原始图片保存
            Graphics gp = Graphics.FromImage(tSrcBmp);
            gp.CopyFromScreen(0, 0, 0, 0, tScreenRect.Size);
            gp.DrawImage(tSrcBmp, 0, 0, tScreenRect, GraphicsUnit.Pixel);
            return tSrcBmp;
        }

        #endregion 截取屏幕图像

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        /// <summary>
        /// 键盘钩子
        /// [以下代码来自某网友，并非本人原创]
        /// </summary>
        public class KeyboardHook
        {
            public event KeyEventHandler KeyDownEvent;

            public event KeyPressEventHandler KeyPressEvent;

            public event KeyEventHandler KeyUpEvent;

            public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

            private static int hKeyboardHook = 0; //声明键盘钩子处理的初始值

            //值在Microsoft SDK的Winuser.h里查询
            // http://www.bianceng.cn/Programming/csharp/201410/45484.htm
            public const int WH_KEYBOARD_LL = 13;   //线程键盘钩子监听鼠标消息设为2，全局键盘监听鼠标消息设为13

            private HookProc KeyboardHookProcedure; //声明KeyboardHookProcedure作为HookProc类型

            //键盘结构
            [StructLayout(LayoutKind.Sequential)]
            public class KeyboardHookStruct
            {
                public int vkCode;  //定一个虚拟键码。该代码必须有一个价值的范围1至254
                public int scanCode; // 指定的硬件扫描码的关键
                public int flags;  // 键标志
                public int time; // 指定的时间戳记的这个讯息
                public int dwExtraInfo; // 指定额外信息相关的信息
            }

            //使用此功能，安装了一个钩子
            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

            //调用此函数卸载钩子
            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern bool UnhookWindowsHookEx(int idHook);

            //使用此功能，通过信息钩子继续下一个钩子
            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

            // 取得当前线程编号（线程钩子需要用到）
            [DllImport("kernel32.dll")]
            private static extern int GetCurrentThreadId();

            //使用WINDOWS API函数代替获取当前实例的函数,防止钩子失效
            [DllImport("kernel32.dll")]
            public static extern IntPtr GetModuleHandle(string name);

            public void Start()
            {
                // 安装键盘钩子
                if (hKeyboardHook == 0)
                {
                    KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                    hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);
                    //hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                    //************************************
                    //键盘线程钩子
                    //SetWindowsHookEx( 2,KeyboardHookProcedure, IntPtr.Zero, GetCurrentThreadId());//指定要监听的线程idGetCurrentThreadId(),
                    //键盘全局钩子,需要引用空间(using System.Reflection;)
                    // SetWindowsHookEx( 13,MouseHookProcedure,Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),0);
                    //
                    //关于SetWindowsHookEx (int idHook, HookProc lpfn, IntPtr hInstance, int threadId)函数将钩子加入到钩子链表中，说明一下四个参数：
                    //idHook 钩子类型，即确定钩子监听何种消息，上面的代码中设为2，即监听键盘消息并且是线程钩子，如果是全局钩子监听键盘消息应设为13，
                    //线程钩子监听鼠标消息设为7，全局钩子监听鼠标消息设为14。lpfn 钩子子程的地址指针。如果dwThreadId参数为0 或是一个由别的进程创建的
                    //线程的标识，lpfn必须指向DLL中的钩子子程。 除此以外，lpfn可以指向当前进程的一段钩子子程代码。钩子函数的入口地址，当钩子钩到任何
                    //消息后便调用这个函数。hInstance应用程序实例的句柄。标识包含lpfn所指的子程的DLL。如果threadId 标识当前进程创建的一个线程，而且子
                    //程代码位于当前进程，hInstance必须为NULL。可以很简单的设定其为本应用程序的实例句柄。threaded 与安装的钩子子程相关联的线程的标识符
                    //如果为0，钩子子程与所有的线程关联，即为全局钩子
                    //************************************
                    //如果SetWindowsHookEx失败
                    if (hKeyboardHook == 0)
                    {
                        Stop();
                        throw new Exception("安装键盘钩子失败");
                    }
                }
            }

            public void Stop()
            {
                bool retKeyboard = true;

                if (hKeyboardHook != 0)
                {
                    retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                    hKeyboardHook = 0;
                }

                if (!(retKeyboard)) throw new Exception("卸载钩子失败！");
            }

            //ToAscii职能的转换指定的虚拟键码和键盘状态的相应字符或字符
            [DllImport("user32")]
            public static extern int ToAscii(int uVirtKey, //[in] 指定虚拟关键代码进行翻译。
                                             int uScanCode, // [in] 指定的硬件扫描码的关键须翻译成英文。高阶位的这个值设定的关键，如果是（不压）
                                             byte[] lpbKeyState, // [in] 指针，以256字节数组，包含当前键盘的状态。每个元素（字节）的数组包含状态的一个关键。如果高阶位的字节是一套，关键是下跌（按下）。在低比特，如果设置表明，关键是对切换。在此功能，只有肘位的CAPS LOCK键是相关的。在切换状态的NUM个锁和滚动锁定键被忽略。
                                             byte[] lpwTransKey, // [out] 指针的缓冲区收到翻译字符或字符。
                                             int fuState); // [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise.

            //获取按键的状态
            [DllImport("user32")]
            public static extern int GetKeyboardState(byte[] pbKeyState);

            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            private static extern short GetKeyState(int vKey);

            private const int WM_KEYDOWN = 0x100;//KEYDOWN
            private const int WM_KEYUP = 0x101;//KEYUP
            private const int WM_SYSKEYDOWN = 0x104;//SYSKEYDOWN
            private const int WM_SYSKEYUP = 0x105;//SYSKEYUP

            private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
            {
                // 侦听键盘事件
                if ((nCode >= 0) && (KeyDownEvent != null || KeyUpEvent != null || KeyPressEvent != null))
                {
                    KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                    // raise KeyDown
                    if (KeyDownEvent != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                    {
                        Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                        KeyEventArgs e = new KeyEventArgs(keyData);
                        KeyDownEvent(this, e);
                    }

                    //键盘按下
                    if (KeyPressEvent != null && wParam == WM_KEYDOWN)
                    {
                        byte[] keyState = new byte[256];
                        GetKeyboardState(keyState);

                        byte[] inBuffer = new byte[2];
                        if (ToAscii(MyKeyboardHookStruct.vkCode, MyKeyboardHookStruct.scanCode, keyState, inBuffer, MyKeyboardHookStruct.flags) == 1)
                        {
                            KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
                            KeyPressEvent(this, e);
                        }
                    }

                    // 键盘抬起
                    if (KeyUpEvent != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                    {
                        Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                        KeyEventArgs e = new KeyEventArgs(keyData);
                        KeyUpEvent(this, e);
                    }
                }
                //如果返回1，则结束消息，这个消息到此为止，不再传递。
                //如果返回0或调用CallNextHookEx函数则消息出了这个钩子继续往下传递，也就是传给消息真正的接受者
                return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
            }

            ~KeyboardHook()
            {
                Stop();
            }
        }
    }
}