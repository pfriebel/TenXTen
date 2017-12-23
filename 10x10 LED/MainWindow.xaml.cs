using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _10x10_LED
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte rValue = 0;
        byte gValue = 0;
        byte bValue = 0;

        LED[] ledArray = new LED[100];

        SolidColorBrush newColor = null;

        public MainWindow()
        {
            InitializeComponent();

            Brush thisBrush = grdSwatch.Background;
            newColor = (SolidColorBrush)thisBrush;
            InitLEDArray();
        }

        private void InitLEDArray()
        {
            for (int i = 0; i < 100; i++)
            {
                ledArray[i] = new LED { redValue = 0, greenValue = 0, blueValue = 0 };
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid thisGrid = (Grid)sender;
            thisGrid.Background = newColor;

            int thisPosition = int.Parse(thisGrid.Name.Substring(8, 2));
            Brush thisBrush = grdSwatch.Background;

            byte r = ((Color)thisBrush.GetValue(SolidColorBrush.ColorProperty)).R;
            byte g = ((Color)thisBrush.GetValue(SolidColorBrush.ColorProperty)).G;
            byte b = ((Color)thisBrush.GetValue(SolidColorBrush.ColorProperty)).B;

            LED thisLED = new LED{ redValue = (int)r, greenValue = (int)g, blueValue = (int)b,position = thisPosition };

            ledArray[thisPosition] = thisLED;
        }

        private void sldrRed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider thisSlider = (Slider)sender;

            byte thisValue = (byte)thisSlider.Value;

            string name = thisSlider.Name;
            switch (name)
            {
                case "sldrRed":
                    rValue = thisValue;
                    break;
                case "sldrGreen":
                    gValue = thisValue;
                    break;
                case "sldrBlue":
                    bValue = thisValue;
                    break;
            }

            newColor = new SolidColorBrush(Color.FromRgb(rValue, gValue, bValue));
            grdSwatch.Background = newColor;
        }

        private void MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid thisGrid = (Grid)sender;

            Brush thisBrush = thisGrid.Background;
            newColor = (SolidColorBrush)thisBrush;
            grdSwatch.Background = newColor;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 100; i++)
            {
                sb.AppendLine("leds[" + i + "] = CRGB("
                    + ledArray[i].redValue.ToString() + ","
                    + ledArray[i].greenValue.ToString() + ","
                    + ledArray[i].blueValue.ToString() + ");");
            }

            Clipboard.SetText(sb.ToString());
        }

        private void btnFillAll_Click(object sender, RoutedEventArgs e)
        {
            //this is a helper class used to gather all the children of a given control
            ChildControls ccChildren = new ChildControls();

            foreach (object o in ccChildren.GetChildren(canvasBase, 1))
            {
                if (o.GetType() == typeof(Grid))
                {
                    Grid grd = (Grid)o;
                    if(grd.Name.StartsWith("grdPixel"))
                    {
                        grd.Background = newColor;
                    }
                }
            }

            ResetArray();
        }

        private void ResetArray()
        {
            byte r = ((Color)newColor.GetValue(SolidColorBrush.ColorProperty)).R;
            byte g = ((Color)newColor.GetValue(SolidColorBrush.ColorProperty)).G;
            byte b = ((Color)newColor.GetValue(SolidColorBrush.ColorProperty)).B;

            for (int i = 0; i < 100; i++)
            {
                ledArray[i] = new LED { redValue = r, greenValue = g, blueValue = b };
            }
        }
    }
}
