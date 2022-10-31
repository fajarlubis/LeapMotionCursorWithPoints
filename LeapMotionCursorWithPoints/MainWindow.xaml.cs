using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Ink;

using Leap;
using System.Reflection.Emit;
using LeapMotionCursorWithPoints;
using System.Reflection;

namespace LeapMotionCursorWithPoints
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller leap = new();
        float windowWidth = 400;
        float windowHeight = 300;
        DrawingAttributes touchIndicator = new();
        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += Update;
            touchIndicator.Width = 20;
            touchIndicator.Height = 20;
            touchIndicator.StylusTip = StylusTip.Ellipse;
        }

        protected void Update(object sender, EventArgs e)
        {
            paintCanvas.Strokes.Clear();
            windowWidth = (float)this.Width;
            windowHeight = (float)this.Height;

            Leap.Frame frame = leap.Frame();
            InteractionBox interactionBox = leap.Frame().InteractionBox;
            leap.EnableGesture(Gesture.GestureType.TYPEKEYTAP);

            for (int g = 0; g < frame.Gestures().Count; g++)
            {
                switch (frame.Gestures()[g].Type)
                {
                    case Gesture.GestureType.TYPE_KEY_TAP:
                        MouseCursor.SendClick();
                        break;
                    default:
                        //Handle unrecognized gestures
                        break;
                }
            }

            foreach (Pointable pointable in leap.Frame().Pointables)
            {
                Leap.Vector normalizedPosition =
                    interactionBox.NormalizePoint(pointable.StabilizedTipPosition);
                float tx = normalizedPosition.x * windowWidth;
                float ty = windowHeight - normalizedPosition.y * windowHeight;

                var cx = (int)(normalizedPosition.x * 1920);
                var cy = (int)(1080 - (normalizedPosition.y * 1080));

                Label.Content = cx;
                Label_Copy.Content = cy;

                int alpha = 255;
                if (pointable.TouchDistance > 0 && pointable.TouchZone != Pointable.Zone.ZONENONE)
                {
                    alpha = 255 - (int)(255 * pointable.TouchDistance);
                    touchIndicator.Color = Color.FromArgb((byte)alpha, 0x0, 0xff, 0x0);
                }
                else if (pointable.TouchDistance <= 0)
                {
                    alpha = -(int)(255 * pointable.TouchDistance);
                    touchIndicator.Color = Color.FromArgb((byte)alpha, 0xff, 0x0, 0x0);
                }
                else
                {
                    alpha = 50;
                    touchIndicator.Color = Color.FromArgb((byte)alpha, 0x0, 0x0, 0xff);
                }
                StylusPoint touchPoint = new(tx, ty);
                StylusPointCollection tips = new(new StylusPoint[] { touchPoint });
                Stroke touchStroke = new(tips, touchIndicator);
                paintCanvas.Strokes.Add(touchStroke);

                MouseCursor.MoveCursor(cx, cy);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }
    }
}
