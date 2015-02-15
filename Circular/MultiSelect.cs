using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using Circular.Words;

namespace Circular
{
    public class MyEditor : System.Drawing.Design.UITypeEditor
    {
        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,  
        // drop down dialog, or no UI outside of the properties window. 
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        private IWindowsFormsEditorService _editorService;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            // use a list box
            ListBox lb = new ListBox();
            lb.SelectionMode = SelectionMode.One;
            lb.SelectedValueChanged += OnListBoxSelectedValueChanged;

            // use the IBenchmark.Name property for list box display
            lb.DisplayMember = "Name";

            lb.Items.AddRange( ((DecorationLine)context.Instance).AllSources );
            // get the analytic object from context
            // this is how we get the list of possible benchmarks
            //Analytic analytic = (Analytic)context.Instance;
            //foreach (IBenchmark benchmark in analytic.Benchmarks)
            //{
            //    // we store benchmarks objects directly in the listbox
            //    int index = lb.Items.Add(benchmark);
            //    if (benchmark.Equals(value))
            //    {
            //        lb.SelectedIndex = index;
            //    }
            //}

            // show this model stuff
            _editorService.DropDownControl(lb);
            if (lb.SelectedItem == null) // no selection, return the passed-in value as is
                return value;

            return lb.SelectedItem;
        }

        private void OnListBoxSelectedValueChanged(object sender, EventArgs e)
        {
            // close the drop down as soon as something is clicked
            _editorService.CloseDropDown();
        }
    }

    public class MyEditor2 : System.Drawing.Design.UITypeEditor
    {
        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,  
        // drop down dialog, or no UI outside of the properties window. 
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        private IWindowsFormsEditorService _editorService;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            // use a list box
            ListBox lb = new ListBox();
            lb.SelectionMode = SelectionMode.One;
            lb.SelectedValueChanged += OnListBoxSelectedValueChanged;

            // use the IBenchmark.Name property for list box display
            lb.DisplayMember = "Name";

            lb.Items.AddRange(((DecorationLine)context.Instance).AllAnchors);
            // get the analytic object from context
            // this is how we get the list of possible benchmarks
            //Analytic analytic = (Analytic)context.Instance;
            //foreach (IBenchmark benchmark in analytic.Benchmarks)
            //{
            //    // we store benchmarks objects directly in the listbox
            //    int index = lb.Items.Add(benchmark);
            //    if (benchmark.Equals(value))
            //    {
            //        lb.SelectedIndex = index;
            //    }
            //}

            // show this model stuff
            _editorService.DropDownControl(lb);
            if (lb.SelectedItem == null) // no selection, return the passed-in value as is
                return value;

            return lb.SelectedItem;
        }

        private void OnListBoxSelectedValueChanged(object sender, EventArgs e)
        {
            // close the drop down as soon as something is clicked
            _editorService.CloseDropDown();
        }
    }

    [Serializable()]
    public class MultiSelect
    {
        private int iValue = 0;
        private string sText;
        public MultiSelect(string Text, int Value)
        {
            sText = Text;
            iValue = Value;
        }
        public int Value
        {
            get
            {
                return iValue;
            }
            set
            {
                iValue = value;
            }
        }
        public string Text
        {
            get
            {
                return sText;
            }
            set
            {
                sText = value;
            }
        }
    }
}
