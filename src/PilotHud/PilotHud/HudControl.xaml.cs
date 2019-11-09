using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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


namespace PilotHud
{

    public class BorderTextLabel : Label
    {

        private static void Redraw(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BorderTextLabel)d).InvalidateVisual();
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BorderTextLabel), new FrameworkPropertyMetadata(string.Empty, Redraw));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(BorderTextLabel), new FrameworkPropertyMetadata(Brushes.Black, Redraw));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(BorderTextLabel), new FrameworkPropertyMetadata((double)1, Redraw));

        FormattedText ft;
        Typeface tf;
        Point startp;
        Pen pen;
        bool cache = true;

        public BorderTextLabel()
        {
            pen = new Pen(Stroke, StrokeThickness);
            startp = new Point(0, 0);
            tf = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
        }
        public void CreateCache()
        {
            pen = new Pen(Stroke, StrokeThickness);
            startp = new Point(0, 0);
            tf = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            ft = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Foreground);

            if (double.IsNaN(Width))
                Width = ft.Width;
            if (double.IsNaN(Height))
                Height = ft.Height;

            if (HorizontalContentAlignment == HorizontalAlignment.Right) startp.X = Width - ft.Width;
            if (HorizontalContentAlignment == HorizontalAlignment.Center) startp.X = (Width - ft.Width) / 2;
            if (VerticalContentAlignment == VerticalAlignment.Bottom) startp.X = Height - ft.Height;
            if (VerticalContentAlignment == VerticalAlignment.Center) startp.X = (Height - ft.Height) / 2;
            var textgeometry = ft.BuildGeometry(startp);
            drawingContext.DrawGeometry(Foreground, pen, textgeometry);

            if (cache)
            {
                cache = !cache;
                CreateCache();
            }
        }

    }
    public partial class HudControl : UserControl
    {
        private const double FPM_RADIUS = 24;
        private const double FPM_LINE_LEN = 6;

        private TransformGroup transformGroup;
        private RotateTransform rotateTransform;
        private TranslateTransform betaTransform;
        private HudCache cache;
        public HudControl()
        {
            MaxClimbRateArrowMag = 50;
            InitializeComponent();
            HudBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x80, 0xFF, 0x80));
            cache = new HudCache(HudBrush);

            transformGroup = new TransformGroup();
            rotateTransform = new RotateTransform();
            betaTransform = new TranslateTransform();
        }
        private static void GestureChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((HudControl)d).InvalidateVisual();
        }



        #region Properties

        public SolidColorBrush HudBrush { get; set; }
        public double RollAngle
        {
            get { return (double)GetValue(RollAngleProperty); }
            set { SetValue(RollAngleProperty, value); }
        }
        public static readonly DependencyProperty RollAngleProperty =
            DependencyProperty.Register("RollAngle", typeof(double), typeof(HudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double PitchAngle
        {
            get { return (double)GetValue(PitchAngleProperty); }
            set { SetValue(PitchAngleProperty, value); }
        }
        public static readonly DependencyProperty PitchAngleProperty =
            DependencyProperty.Register("PitchAngle", typeof(double), typeof(HudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double Altitude
        {
            get { return (double)GetValue(AltitudeProperty); }
            set { SetValue(AltitudeProperty, value); }
        }
        public static readonly DependencyProperty AltitudeProperty =
            DependencyProperty.Register("Altitude", typeof(double), typeof(HudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double ClimbRate
        {
            get { return (double)GetValue(ClimbRateProperty); }
            set { SetValue(ClimbRateProperty, value); }
        }
        public static readonly DependencyProperty ClimbRateProperty =
            DependencyProperty.Register("ClimbRate", typeof(double), typeof(HudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double MaxClimbRateArrowMag
        {
            get { return (double)GetValue(MaxClimbRateArrowMagProperty); }
            set
            {
                if (value > 0)
                {
                    SetValue(MaxClimbRateArrowMagProperty, value);
                }
            }
        }
        public static readonly DependencyProperty MaxClimbRateArrowMagProperty =
            DependencyProperty.Register("MaxClimbRateArrowMag", typeof(double), typeof(HudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double YawAngle
        {
            get { return (double)GetValue(YawAngleProperty); }
            set { SetValue(YawAngleProperty, value); }
        }
        public static readonly DependencyProperty YawAngleProperty =
            DependencyProperty.Register("YawAngle", typeof(double), typeof(HudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double Mach
        {
            get { return (double)GetValue(MachProperty); }
            set { SetValue(MachProperty, value); }
        }
        public static readonly DependencyProperty MachProperty =
            DependencyProperty.Register("Mach", typeof(double), typeof(HudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double G_Load
        {
            get { return (double)GetValue(G_LoadProperty); }
            set { SetValue(G_LoadProperty, value); }
        }
        public static readonly DependencyProperty G_LoadProperty =
            DependencyProperty.Register("G_Load", typeof(double), typeof(HudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double SpeedMs
        {
            get { return (double)GetValue(SpeedMsProperty); }
            set { SetValue(SpeedMsProperty, value); }
        }
        public static readonly DependencyProperty SpeedMsProperty =
            DependencyProperty.Register("SpeedMs", typeof(double), typeof(HudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double Alpha
        {
            get { return (double)GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }
        public static readonly DependencyProperty AlphaProperty =
            DependencyProperty.Register("Alpha", typeof(double), typeof(HudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double Beta
        {
            get { return (double)GetValue(BetaProperty); }
            set { SetValue(BetaProperty, value); }
        }
        public static readonly DependencyProperty BetaProperty =
            DependencyProperty.Register("Beta", typeof(double), typeof(HudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        #endregion

        #region Display Properties

        private const int VERTICAL_DEG_TO_DISP = 36;
        private const int HORIZONTAL_DEG_TO_DISP = 45;
        private const int YAW_COMPASS_DEG_TO_DISP = 26;

        #endregion

        #region Draw Methods

        private void DrawGroundAndSky(double pitchDeg)
        {
            double vertPixelsPerDeg = Grid_Viewport.ActualHeight / VERTICAL_DEG_TO_DISP;

            double offset = pitchDeg * vertPixelsPerDeg;

            // want to always make sure the canvas is filled by the sky and/or ground rectangles
            // - therefore, create them oversized and limit max offset, to prevent any issues
            // with silly window sizes/aspect ratios
            double maxDim = Grid_Viewport.ActualWidth;
            if (Grid_Viewport.ActualHeight > maxDim)
            {
                maxDim = Grid_Viewport.ActualHeight;
            }
            if (offset > maxDim)
            {
                offset = maxDim;
            }
            else if (offset < -maxDim)
            {
                offset = -maxDim;
            }
            const double OVERSIZE_RATIO = 5;
            double rectDimension = maxDim * OVERSIZE_RATIO;

            cache.GroundAndSky.gndRect.Width = rectDimension;
            cache.GroundAndSky.gndRect.Height = rectDimension;
            Canvas.SetLeft(cache.GroundAndSky.gndRect, -maxDim);
            Canvas.SetTop(cache.GroundAndSky.gndRect, offset);

            cache.GroundAndSky.skyRect.Width = rectDimension;
            cache.GroundAndSky.skyRect.Height = rectDimension;
            Canvas.SetLeft(cache.GroundAndSky.skyRect, -maxDim);
            Canvas.SetBottom(cache.GroundAndSky.skyRect, -offset);

            cache.GroundAndSky.line.X1 = -rectDimension;
            cache.GroundAndSky.line.X2 = rectDimension;
            cache.GroundAndSky.line.Y1 = offset;
            cache.GroundAndSky.line.Y2 = offset;
            cache.GroundAndSky.line.Stroke = HudBrush;
            cache.GroundAndSky.line.StrokeThickness = 2;

            Canvas_Background.Children.Add(cache.GroundAndSky.gndRect);
            Canvas_Background.Children.Add(cache.GroundAndSky.skyRect);
            Canvas_Background.Children.Add(cache.GroundAndSky.line);

        }
        private void DrawMajorPitchTick(int index, double offset, double val, bool dispTxt)
        {
            Canvas.SetLeft(cache.MajorPitchTick.lnL[index], -80);
            Canvas.SetTop(cache.MajorPitchTick.lnL[index], -offset);
            Canvas_PitchIndicator.Children.Add(cache.MajorPitchTick.lnL[index]);

            Canvas.SetLeft(cache.MajorPitchTick.lnR[index], 40);
            Canvas.SetTop(cache.MajorPitchTick.lnR[index], -offset);
            Canvas_PitchIndicator.Children.Add(cache.MajorPitchTick.lnR[index]);

            if (val != 0)
            {
                Canvas.SetLeft(cache.MajorPitchTick.left[index], -80);
                Canvas.SetTop(cache.MajorPitchTick.left[index], -offset);

                Canvas.SetRight(cache.MajorPitchTick.right[index], -80);
                Canvas.SetTop(cache.MajorPitchTick.right[index], -offset);

                Canvas_PitchIndicator.Children.Add(cache.MajorPitchTick.left[index]);
                Canvas_PitchIndicator.Children.Add(cache.MajorPitchTick.right[index]);
            }

            if (cache.MajorPitchTick.disp[index])
            {
                cache.MajorPitchTick.txtBlkL[index].Text = val.ToString("##0");
                Canvas.SetTop(cache.MajorPitchTick.txtBlkL[index], -offset - 3);
                Canvas.SetLeft(cache.MajorPitchTick.txtBlkL[index], -120);
                Canvas_PitchIndicator.Children.Add(cache.MajorPitchTick.txtBlkL[index]);

                cache.MajorPitchTick.txtBlkR[index].Text = val.ToString("##0");
                Canvas.SetTop(cache.MajorPitchTick.txtBlkR[index], -offset - 3);
                Canvas.SetRight(cache.MajorPitchTick.txtBlkR[index], -120);
                Canvas_PitchIndicator.Children.Add(cache.MajorPitchTick.txtBlkR[index]);
            }
        }
        private void DrawMinorPitchTick(int index, double offset, double val)
        {
            Canvas.SetLeft(cache.MinorPitchTick.lnL[index], -60);
            Canvas.SetTop(cache.MinorPitchTick.lnL[index], -offset);
            Canvas_PitchIndicator.Children.Add(cache.MinorPitchTick.lnL[index]);

            Canvas.SetLeft(cache.MinorPitchTick.lnR[index], 35);
            Canvas.SetTop(cache.MinorPitchTick.lnR[index], -offset);
            Canvas_PitchIndicator.Children.Add(cache.MinorPitchTick.lnR[index]);

            if (val != 0)
            {
                Canvas.SetLeft(cache.MinorPitchTick.left[index], -60);
                Canvas.SetTop(cache.MinorPitchTick.left[index], -offset);

                Canvas.SetRight(cache.MinorPitchTick.right[index], -60);
                Canvas.SetTop(cache.MinorPitchTick.right[index], -offset);

                Canvas_PitchIndicator.Children.Add(cache.MinorPitchTick.left[index]);
                Canvas_PitchIndicator.Children.Add(cache.MinorPitchTick.right[index]);
            }
        }
        private void DrawPitchTicks(double pitchDeg, double rollAngle)
        {
            double vertPixelsPerDeg = Grid_Viewport.ActualHeight / VERTICAL_DEG_TO_DISP;
            double zeroOffset = -(pitchDeg * vertPixelsPerDeg);

            // the pitch indicator grid is only a percentage of the overall viewport height
            // - need to account for this (sso pitch '0' indicator lines up with ground/sky border)
            double gridOffset = ((Grid_Viewport.ActualHeight - Grid_PitchIndicator.ActualHeight) / 2.0) * Math.Cos(rollAngle * Math.PI / 180.0);

            zeroOffset += gridOffset;

            for (int i = 1; i < 10; i += 2)
            {
                double pitchVal = i * 10;
                double offset = (pitchVal * vertPixelsPerDeg) + zeroOffset;
                DrawMajorPitchTick(i, offset, pitchVal, true);

                offset -= (5 * vertPixelsPerDeg);
                DrawMinorPitchTick(i, offset, pitchVal - 5);

                offset = -(pitchVal * vertPixelsPerDeg) + zeroOffset;
                DrawMajorPitchTick(i + 1, offset, -pitchVal, true);

                offset += (5 * vertPixelsPerDeg);
                DrawMinorPitchTick(i + 1, offset, -pitchVal + 5);
            }
        }
        private void DrawCompass(double yawDeg)
        {
            double wl = Grid_Compass.ActualWidth;

            double horzPixelsPerDeg = wl / YAW_COMPASS_DEG_TO_DISP;

            double startYaw = yawDeg - (YAW_COMPASS_DEG_TO_DISP / 2.0);
            int roundedStart = (int)Math.Ceiling(startYaw);

            // this is the x co-ord of the left-most tick to be displayed displayed
            double tickOffset = (roundedStart - startYaw) * horzPixelsPerDeg;

            for (int i = 0; i < YAW_COMPASS_DEG_TO_DISP; i++)
            {
                if (0 == ((i + roundedStart) % 2))
                {
                    cache.Compass.tl[i].X1 = tickOffset + (i * horzPixelsPerDeg);
                    cache.Compass.tl[i].X2 = cache.Compass.tl[i].X1;

                    if (0 == ((i + roundedStart) % 10))
                    {
                        cache.Compass.tl[i].Y1 = 21;
                        int txt = (i + roundedStart);
                        if (txt < 0)
                        {
                            txt += 360;
                        }
                        txt /= 10;
                        cache.Compass.ticktext[i].Text = txt.ToString("D2");
                        Canvas.SetTop(cache.Compass.ticktext[i], 2);
                        Canvas.SetLeft(cache.Compass.ticktext[i], cache.Compass.tl[i].X1 - 10);
                        Canvas_Compass.Children.Add(cache.Compass.ticktext[i]);

                    }
                    else
                    {
                        cache.Compass.tl[i].Y1 = 25;
                    }
                    cache.Compass.tl[i].Y2 = 30;
                    cache.Compass.tl[i].Stroke = HudBrush;
                    cache.Compass.tl[i].StrokeThickness = 1;
                    Canvas_Compass.Children.Add(cache.Compass.tl[i]);
                }
            }

        }
        private void DrawHeading(double yawDeg)
        {
            yawDeg = yawDeg % 360;

            if (yawDeg < 0)
            {
                yawDeg += 360;
            }

            int yawInt = (int)yawDeg;
            if (360 == yawInt)
            {
                yawInt = 0;
            }

            double left = (Grid_Compass.ActualWidth / 2) - 30;

            string hdgStr = "HDG ";
            if (yawInt < 100)
            {
                hdgStr += " ";
            }
            if (yawInt < 10)
            {
                hdgStr += " ";
            }

            cache.Heading.heading.Text = hdgStr + ((int)yawDeg).ToString();
            cache.Heading.heading.Foreground = HudBrush;

            Canvas.SetTop(cache.Heading.border, 44);
            Canvas.SetLeft(cache.Heading.border, left);
            Canvas_Compass.Children.Add(cache.Heading.border);

            cache.Heading.leftLn.X1 = (Grid_Compass.ActualWidth / 2) - 15;
            cache.Heading.leftLn.X2 = (Grid_Compass.ActualWidth / 2);
            Canvas_Compass.Children.Add(cache.Heading.leftLn);

            cache.Heading.rightLn.X1 = (Grid_Compass.ActualWidth / 2) + 15;
            cache.Heading.rightLn.X2 = (Grid_Compass.ActualWidth / 2);
            Canvas_Compass.Children.Add(cache.Heading.rightLn);
        }
        private void DrawRollTick(int index, double circleRad, double rollAngle, bool isLarge)
        {
            if (true == isLarge)
            {
                cache.RollTick.line[index].Y1 = -24;
            }
            else
            {
                cache.RollTick.line[index].Y1 = -12;
            }

            Canvas.SetTop(cache.RollTick.line[index], -circleRad);
            cache.RollTick.rotationTransform[index].Angle = rollAngle;
            cache.RollTick.rotationTransform[index].CenterX = 0;
            cache.RollTick.rotationTransform[index].CenterY = circleRad;

            Canvas_HUD.Children.Add(cache.RollTick.line[index]);
        }
        private void DrawZeroRollTick(double circleRad)
        {
            Canvas.SetTop(cache.ZeroRollTick.triangle, -circleRad);
            Canvas_HUD.Children.Add(cache.ZeroRollTick.triangle);
        }
        private void DrawRollIndicator(double circleRad, double rollAngle)
        {
            cache.RollIndicator.triangleRenderTransform.Angle = rollAngle;
            cache.RollIndicator.triangleRenderTransform.CenterX = 0;
            cache.RollIndicator.triangleRenderTransform.CenterY = circleRad;

            cache.RollIndicator.trapezoidRenderTransform.Angle = rollAngle;
            cache.RollIndicator.trapezoidRenderTransform.CenterX = 0;
            cache.RollIndicator.trapezoidRenderTransform.CenterY = circleRad;

            cache.RollIndicator.triangle.RenderTransform = cache.RollIndicator.triangleRenderTransform;
            cache.RollIndicator.trapezoid.RenderTransform = cache.RollIndicator.trapezoidRenderTransform;

            Canvas.SetTop(cache.RollIndicator.triangle, -circleRad);
            Canvas.SetTop(cache.RollIndicator.trapezoid, -circleRad);
            Canvas_HUD.Children.Add(cache.RollIndicator.triangle);
            Canvas_HUD.Children.Add(cache.RollIndicator.trapezoid);

        }
        private void DrawRoll(double rollAngle)
        {
            double circleRad = Grid_Viewport.ActualHeight / 3;

            DrawZeroRollTick(circleRad);
            for (int i = 0; i < cache.Roll.tickList.Count; i++)
            {
                DrawRollTick(i, circleRad, cache.Roll.tickList[i].Key, cache.Roll.tickList[i].Value);
            }
            DrawRollIndicator(circleRad, rollAngle);
        }
        private void DrawAltitude(double altitude)
        {
            cache.Altitude.txtBlk.Text = "ALT\nM: " + altitude.ToString();
            Canvas.SetTop(cache.Altitude.txtBlk, -150);
            Canvas_RightHUD.Children.Add(cache.Altitude.txtBlk);
        }
        private void DrawClimbRate(double climbRate)
        {
            cache.ClimbRate.txtBlk.Text = "VERT\n" + climbRate.ToString("+0.0;-0.0;0.0");
            Canvas.SetTop(cache.ClimbRate.txtBlk, -50);
            Canvas_RightHUD.Children.Add(cache.ClimbRate.txtBlk);

            Canvas_RightHUD.Children.Add(cache.ClimbRate.zeroLn);

            double maxHUD_Height = Grid_RightHUD.ActualHeight * 0.7;

            double magHeight = Math.Abs(climbRate);
            if (magHeight > MaxClimbRateArrowMag)
            {
                magHeight = MaxClimbRateArrowMag;
            }

            cache.ClimbRate.climbMagnitude.Height = maxHUD_Height * (magHeight / (2 * MaxClimbRateArrowMag));
            Canvas.SetLeft(cache.ClimbRate.climbMagnitude, -31);

            Canvas.SetLeft(cache.ClimbRate.triangle, -25);

            if (climbRate > 0)
            {
                Canvas.SetTop(cache.ClimbRate.climbMagnitude, -cache.ClimbRate.climbMagnitude.Height);
                Canvas.SetTop(cache.ClimbRate.triangle, -cache.ClimbRate.climbMagnitude.Height);
            }
            else
            {
                Canvas.SetTop(cache.ClimbRate.triangle, cache.ClimbRate.climbMagnitude.Height);
            }

            if (Math.Abs(climbRate) > 1.0)
            {
                // don't draw flickering arrow if bouncing around near 0
                Canvas_RightHUD.Children.Add(cache.ClimbRate.triangle);
            }
            Canvas_RightHUD.Children.Add(cache.ClimbRate.climbMagnitude);
        }
        private void DrawSpeedAndG(double speed_ms, double mach, double gLoad)
        {
            cache.SpeedAndG.txtBlkGM.Text = "G  " + gLoad.ToString("+0.0;-0.0;0.0") + "\n" + "M  " + mach.ToString("0.00");
            Canvas.SetTop(cache.SpeedAndG.txtBlkGM, -50);
            Canvas.SetLeft(cache.SpeedAndG.txtBlkGM, -25);
            Canvas_LeftHUD.Children.Add(cache.SpeedAndG.txtBlkGM);

            cache.SpeedAndG.txtBlkSpd.Text = "SPEED\nm/s    " + speed_ms.ToString("0");
            Canvas.SetTop(cache.SpeedAndG.txtBlkSpd, -150);
            Canvas.SetLeft(cache.SpeedAndG.txtBlkSpd, -25);
            Canvas_LeftHUD.Children.Add(cache.SpeedAndG.txtBlkSpd);
        }
        private void DrawAircraft()
        {
            Canvas_HUD.Children.Add(cache.Aircraft.waterline);
        }
        private void DrawFlightPathMarker(double alpha, double beta)
        {
            double vertPixelsPerDeg = Grid_Viewport.ActualHeight / VERTICAL_DEG_TO_DISP;
            double horzPixelsPerDeg = Grid_Viewport.ActualWidth / HORIZONTAL_DEG_TO_DISP;

            double leftOffset = (-FPM_RADIUS / 2.0) + (beta * horzPixelsPerDeg);
            double rightOffset = (FPM_RADIUS / 2.0) + (beta * horzPixelsPerDeg);
            double topOffset = (-FPM_RADIUS / 2.0) + (alpha * vertPixelsPerDeg);

            Canvas.SetLeft(cache.FlightPathMarker.body, leftOffset);
            Canvas.SetTop(cache.FlightPathMarker.body, topOffset);
            Canvas_HUD.Children.Add(cache.FlightPathMarker.body);

            Canvas.SetLeft(cache.FlightPathMarker.leftLn, leftOffset);
            Canvas.SetTop(cache.FlightPathMarker.leftLn, topOffset);
            Canvas_HUD.Children.Add(cache.FlightPathMarker.leftLn);


            Canvas.SetLeft(cache.FlightPathMarker.rightLn, rightOffset);
            Canvas.SetTop(cache.FlightPathMarker.rightLn, topOffset);
            Canvas_HUD.Children.Add(cache.FlightPathMarker.rightLn);

            Canvas.SetTop(cache.FlightPathMarker.topLn, topOffset);
            Canvas.SetLeft(cache.FlightPathMarker.topLn, beta * horzPixelsPerDeg);
            Canvas_HUD.Children.Add(cache.FlightPathMarker.topLn);
        }
        private void DrawAlphaBeta(double alpha, double beta)
        {
            cache.AlphaBeta.txtBlkSpd.Text = "\u03B1    " + alpha.ToString("+0.0;-0.0;0.0") + "\n\u03B2    " + beta.ToString("+0.0;-0.0;0.0");
            Canvas.SetTop(cache.AlphaBeta.txtBlkSpd, 50);
            Canvas.SetLeft(cache.AlphaBeta.txtBlkSpd, -25);
            Canvas_LeftHUD.Children.Add(cache.AlphaBeta.txtBlkSpd);
        }

        #endregion

        protected override void OnRender(DrawingContext drawingContext)
        {
            transformGroup.Children.Clear();
            base.OnRender(drawingContext);

            Canvas_Background.Children.Clear();
            Canvas_PitchIndicator.Children.Clear();
            Canvas_HUD.Children.Clear();
            Canvas_Compass.Children.Clear();
            Canvas_RightHUD.Children.Clear();
            Canvas_LeftHUD.Children.Clear();

            DrawAltitude(Altitude);
            DrawGroundAndSky(PitchAngle);
            DrawPitchTicks(PitchAngle, RollAngle);
            DrawCompass(YawAngle);
            DrawHeading(YawAngle);
            DrawRoll(RollAngle);
            DrawClimbRate(ClimbRate);
            DrawSpeedAndG(SpeedMs, Mach, G_Load);
            DrawAircraft();
            DrawFlightPathMarker(Alpha, Beta);
            DrawAlphaBeta(Alpha, Beta);

            rotateTransform.Angle = -RollAngle;
            Canvas_Background.RenderTransform = rotateTransform;

            double horzPixelsPerDeg = Grid_Viewport.ActualWidth / HORIZONTAL_DEG_TO_DISP;

            betaTransform.X = Beta * horzPixelsPerDeg;
            betaTransform.Y = 0;
            transformGroup.Children.Add(betaTransform);
            transformGroup.Children.Add(rotateTransform);


            Canvas_PitchIndicator.RenderTransform = transformGroup;
        }

    }
}
