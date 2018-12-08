using Newtonsoft.Json;
using System;
using System.Windows.Controls;

namespace MyWpfApp
{
    /// <summary>
    /// Interaction logic for ChatroomView.xaml
    /// </summary>
    public partial class ChatroomView : UserControl
    {
        public ChatroomView()
        {
            InitializeComponent();            
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            Console.WriteLine(JsonConvert.SerializeObject(new
            {
                e.ExtentHeight,
                e.ExtentHeightChange,                
                e.VerticalChange,
                e.VerticalOffset,
                e.ViewportHeight,
                e.ViewportHeightChange,                
            }, Formatting.Indented));
            if (e.ExtentHeightChange > 0 && sender is ScrollViewer scroll)
            {
                if (e.VerticalOffset + e.ExtentHeightChange >= scroll.ScrollableHeight)
                    scroll.ScrollToVerticalOffset(e.VerticalOffset + e.ExtentHeightChange);
            }
        }
    }
}
