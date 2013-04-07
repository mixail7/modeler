using System.Windows;
using System.Windows.Input;
using DiagramDesigner;

namespace ActyvityDiagram_Pal
{
    public partial class AD_Division2_ToolBox
    {
        public AD_Division2_ToolBox()
        {
            InitializeComponent();
        }
        private bool isDragging;

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            isDragging = true;
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                isDragging = false;
            }
            if (isDragging == false)
            {
                return;
            }
            var dataObject = new DragObject { formatStr = BlockToolBoxPalleter.blockFormatName, data = AD_Division2_Element.ElementName };
            dataObject.DesiredSize = new Size(80, 20);
            DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);
            e.Handled = true;
            isDragging = false;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
        }
    }
}
