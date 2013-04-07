using System.Windows;
using System.Windows.Input;
using DiagramDesigner;

namespace ActyvityDiagram_Pal
{
    /// <summary>
    /// Interaction logic for AD_Action_ToolBox.xaml
    /// </summary>
    public partial class AD_Action_ToolBox
    {
        public AD_Action_ToolBox()
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

            var dataObject = new DragObject { formatStr = BlockToolBoxPalleter.blockFormatName, data = AD_Action_Element.ElementName };
            dataObject.DesiredSize = new Size(100, 40);
            DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);
            e.Handled = true;
            isDragging = false;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.isDragging = true;
        }
    }
}
