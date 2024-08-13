using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BatteryMonitor
{
    internal class Responsive
    {
        private List<Responsive.component> componentList;
        private bool showRowHeader = false;
        private Form form { get; set; }
        private Size formSize { get; set; }
        private float fontsize { get; set; }

        public Responsive(Form form)
        {
            this.componentList = new List<Responsive.component>();
            this.form = form;
        }

        public void initialSize()
        {
            this.formSize = this.form.Size;
            foreach (Control allControl in Responsive.get_all_controls((Control)this.form))
            {
                this.componentList.Add(new Responsive.component(allControl, allControl.Bounds, allControl.Font.Size));
                if (allControl.GetType() == typeof(DataGridView))
                    this._dgv_Column_Adjust((DataGridView)allControl, this.showRowHeader);
            }
        }
        private void _dgv_Column_Adjust(DataGridView dgv, bool _showRowHeader)
        {
            int num = 0;
            if (_showRowHeader)
                num = dgv.RowHeadersWidth;
            else
                dgv.RowHeadersVisible = false;
            for (int index = 0; index < dgv.ColumnCount; ++index)
                dgv.Columns[index].Width = dgv.Dock != DockStyle.Fill ? (dgv.Width - num - 5) / dgv.ColumnCount : (dgv.Width - num) / dgv.ColumnCount;
        }

        private static IEnumerable<Control> get_all_controls(Control c)
        {
            return c.Controls.Cast<Control>().SelectMany<Control, Control>((Func<Control, IEnumerable<Control>>)(item => Responsive.get_all_controls(item))).Concat<Control>(c.Controls.Cast<Control>()).Where<Control>((Func<Control, bool>)(control => control.Name != string.Empty));
        }

        public class component
        {
            public Control _control = new Control();
            public Rectangle _bound = new Rectangle();
            public float _fontsize_ = 0.0f;

            public component(Control control, Rectangle bound, float _thisFontSize)
            {
                this._control = control;
                this._bound = bound;
                this._fontsize_ = _thisFontSize;
            }
        }

        public void _resize()
        {
            Size size1 = this.form.ClientSize;
            double width1 = (double)size1.Width;
            size1 = this.formSize;
            double width2 = (double)size1.Width;
            double num1 = width1 / width2;
            Size size2 = this.form.ClientSize;
            double height1 = (double)size2.Height;
            size2 = this.formSize;
            double height2 = (double)size2.Height;
            double num2 = height1 / height2;
            foreach (Responsive.component component in this.componentList)
            {
                this.fontsize = (float)((double)component._fontsize_ * num1 / 2.5 + (double)component._fontsize_ * num2 / 2.5);
                if ((double)this.fontsize > 0.0 && (double)this.fontsize <= 3.4028234663852886E+38)
                    component._control.Font = new Font(this.form.Font.FontFamily, this.fontsize);
                Point location = new Point((int)((double)component._bound.X * num1), (int)((double)component._bound.Y * num2));
                Size size3 = new Size((int)((double)component._bound.Width * num1), (int)((double)component._bound.Height * num2));
                component._control.Bounds = new Rectangle(location, size3);
                if (component._control.GetType() == typeof(DataGridView))
                    this._dgv_Column_Adjust((DataGridView)component._control, this.showRowHeader);
            }

        }
    }

}
