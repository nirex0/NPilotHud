using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Globalization;

namespace PilotHud
{
    public class HudCache
    {
        const int RollCacheCount = 10;
        public GroundAndSkyCache GroundAndSky;
        public MajorPitchTickCache MajorPitchTick;
        public MinorPitchTickCache MinorPitchTick;
        public CompassCache Compass;
        public HeadingCache Heading;
        public RollTickCache RollTick;
        public ZeroRollTickCache ZeroRollTick;
        public RollIndicatorCache RollIndicator;
        public RollCache Roll;
        public AltitudeCache Altitude;
        public ClimbRateCache ClimbRate;
        public SpeedAndGCache SpeedAndG;
        public AircraftCache Aircraft;
        public FlightPathMarkerCache FlightPathMarker;
        public AlphaBetaCache AlphaBeta;
        public HudCache(SolidColorBrush HudBrush)
        {
            GroundAndSky = new GroundAndSkyCache(HudBrush);
            MajorPitchTick = new MajorPitchTickCache(HudBrush);
            MinorPitchTick = new MinorPitchTickCache(HudBrush);
            Compass = new CompassCache(HudBrush);
            Heading = new HeadingCache(HudBrush);
            RollTick = new RollTickCache(HudBrush);
            ZeroRollTick = new ZeroRollTickCache(HudBrush);
            RollIndicator = new RollIndicatorCache(HudBrush);
            Roll = new RollCache(HudBrush);
            Altitude = new AltitudeCache(HudBrush);
            ClimbRate = new ClimbRateCache(HudBrush);
            SpeedAndG = new SpeedAndGCache(HudBrush);
            Aircraft = new AircraftCache(HudBrush);
            FlightPathMarker = new FlightPathMarkerCache(HudBrush);
            AlphaBeta = new AlphaBetaCache(HudBrush);
        }
        public class GroundAndSkyCache
        {
            public LinearGradientBrush gndGradBrush;
            public LinearGradientBrush skyGradBrush;

            public Rectangle gndRect;
            public Rectangle skyRect;
            public Line line;

            public GroundAndSkyCache(SolidColorBrush HudBrush)
            {
                gndGradBrush = new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1)
                };

                gndGradBrush.GradientStops.Add(new GradientStop(Color.FromRgb(101, 90, 77), 0.0));
                gndGradBrush.GradientStops.Add(new GradientStop(Color.FromRgb(85, 77, 66), 0.25));

                skyGradBrush = new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1)
                };

                skyGradBrush.GradientStops.Add(new GradientStop(Color.FromRgb(104, 161, 172), 0.75));
                skyGradBrush.GradientStops.Add(new GradientStop(Color.FromRgb(128, 167, 175), 1));

                gndRect = new Rectangle
                {
                    Fill = gndGradBrush
                };

                skyRect = new Rectangle
                {
                    Fill = skyGradBrush
                };

                line = new Line();
            }
        }
        public class MajorPitchTickCache
        {
            public Line[] lnL;
            public Line[] lnR;
            public Line[] left;
            public Line[] right;
            public bool[] disp;

            public BorderTextLabel[] txtBlkL;
            public BorderTextLabel[] txtBlkR;

            public MajorPitchTickCache(SolidColorBrush HudBrush)
            {
                lnL = new Line[18];
                for (int i = 0; i < 18; i++)
                {
                    lnL[i] = new Line
                    {
                        X1 = 0,
                        X2 = 40,
                        Y1 = 0,
                        Y2 = 0,
                        Stroke = HudBrush,
                        StrokeThickness = 2
                    };
                }

                lnR = new Line[18];
                for (int i = 0; i < 18; i++)
                {
                    lnR[i] = new Line
                    {
                        X1 = 0,
                        X2 = 40,
                        Y1 = 0,
                        Y2 = 0,
                        Stroke = HudBrush,
                        StrokeThickness = 2
                    };
                }

                left = new Line[18];
                for (int i = 0; i < 18; i++)
                {
                    left[i] = new Line
                    {
                        X1 = 0,
                        X2 = 0,
                        Y1 = 0,
                        Y2 = 7,
                        Stroke = HudBrush,
                        StrokeThickness = 2
                    };
                }

                for (int i = 0; i < 9; i++)
                {
                    left[i * 2].Y2 = -5;
                }

                right = new Line[18];
                for (int i = 0; i < 18; i++)
                {
                    right[i] = new Line
                    {
                        X1 = 0,
                        X2 = 0,
                        Y1 = 0,
                        Y2 = 7,
                        Stroke = HudBrush,
                        StrokeThickness = 2
                    };
                }

                for (int i = 0; i < 9; i++)
                {
                    right[i * 2].Y2 = -5;
                }

                disp = new bool[18];
                for (int i = 0; i < 18; i++)
                {
                    disp[i] = true;
                }

                txtBlkL = new BorderTextLabel[18];
                for (int i = 0; i < 18; i++)
                {
                    txtBlkL[i] = new BorderTextLabel();
                    txtBlkL[i].Stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    txtBlkL[i].HorizontalContentAlignment = HorizontalAlignment.Left;
                    txtBlkL[i].Foreground = HudBrush;
                    txtBlkL[i].FontSize = 12;
                    txtBlkL[i].FontWeight = FontWeights.Bold;
                }

                txtBlkR = new BorderTextLabel[18];
                for (int i = 0; i < 18; i++)
                {
                    txtBlkR[i] = new BorderTextLabel();
                    txtBlkR[i].Stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    txtBlkR[i].HorizontalContentAlignment = HorizontalAlignment.Right;
                    txtBlkR[i].Foreground = HudBrush;
                    txtBlkR[i].FontSize = 12;
                    txtBlkR[i].FontWeight = FontWeights.Bold;
                }

            }

        }
        public class MinorPitchTickCache
        {
            public Line[] lnL;
            public Line[] lnR;
            public Line[] left;
            public Line[] right;
            public MinorPitchTickCache(SolidColorBrush HudBrush)
            {
                lnL = new Line[18];
                for (int i = 0; i < 18; i++)
                {
                    lnL[i] = new Line
                    {
                        X1 = 0,
                        X2 = 25,
                        Y1 = 0,
                        Y2 = 0,
                        Stroke = HudBrush,
                        StrokeThickness = 1
                    };
                }

                lnR = new Line[18];
                for (int i = 0; i < 18; i++)
                {
                    lnR[i] = new Line
                    {
                        X1 = 0,
                        X2 = 25,
                        Y1 = 0,
                        Y2 = 0,
                        Stroke = HudBrush,
                        StrokeThickness = 1
                    };
                }

                left = new Line[18];
                for (int i = 0; i < 18; i++)
                {
                    left[i] = new Line
                    {
                        X1 = 0,
                        X2 = 0,
                        Y1 = 0,
                        Y2 = 5,
                        Stroke = HudBrush,
                        StrokeThickness = 1
                    };
                }

                for (int i = 0; i < 9; i++)
                {
                    left[i * 2].Y2 = -5;
                }

                right = new Line[18];
                for (int i = 0; i < 18; i++)
                {
                    right[i] = new Line
                    {
                        X1 = 0,
                        X2 = 0,
                        Y1 = 0,
                        Y2 = 5,
                        Stroke = HudBrush,
                        StrokeThickness = 1
                    };
                }

                for (int i = 0; i < 9; i++)
                {
                    right[i * 2].Y2 = -5;
                }
            }
        }
        public class CompassCache
        {
            public Line[] tl;
            public BorderTextLabel[] ticktext;
            public CompassCache(SolidColorBrush HudBrush)
            {
                tl = new Line[26];
                for (int i = 0; i < tl.Length; i++)
                {
                    tl[i] = new Line();
                }

                ticktext = new BorderTextLabel[26];
                for (int i = 0; i < ticktext.Length; i++)
                {
                    ticktext[i] = new BorderTextLabel
                    {
                        FontSize = 14,
                        Stroke = HudBrush,
                        Foreground = HudBrush,
                        FontFamily = new FontFamily("Courier New")
                    };
                }
            }
        }
        public class HeadingCache
        {
            public BorderTextLabel heading;
            public Border border;
            public Thickness thickness;
            public Line leftLn;
            public Line rightLn;

            public HeadingCache(SolidColorBrush HudBrush)
            {
                heading = new BorderTextLabel
                {
                    FontFamily = new FontFamily("Courier New"),
                    Stroke = HudBrush,
                    FontSize = 12,
                    Foreground = HudBrush
                };

                thickness = new Thickness(1);
                border = new Border
                {
                    BorderThickness = thickness,
                    Child = heading,
                    BorderBrush = HudBrush
                };

                leftLn = new Line
                {
                    Y1 = 44,
                    Y2 = 30,
                    Stroke = HudBrush,
                    StrokeThickness = 1
                };

                rightLn = new Line
                {
                    Y1 = 44,
                    Y2 = 30,
                    Stroke = HudBrush,
                    StrokeThickness = 1
                };
            }

        }
        public class RollTickCache
        {
            public Line[] line;
            public RotateTransform[] rotationTransform;

            public RollTickCache(SolidColorBrush HudBrush)
            {
                rotationTransform = new RotateTransform[RollCacheCount];

                for (int i = 0; i < RollCacheCount; i++)
                {
                    rotationTransform[i] = new RotateTransform();
                }

                line = new Line[RollCacheCount];
                for (int i = 0; i < RollCacheCount; i++)
                {
                    line[i] = new Line
                    {
                        X1 = 0,
                        X2 = 0,
                        Y2 = 0,
                        Stroke = HudBrush,
                        StrokeThickness = 2,
                        RenderTransform = rotationTransform[i]
                    };
                };
            }
        }
        public class ZeroRollTickCache
        {
            private Polygon CreateTriangle(SolidColorBrush HudBrush, double x1, double y1, double x2, double y2, double x3, double y3)
            {
                Polygon triangle = new Polygon
                {
                    Stroke = HudBrush,
                    Fill = HudBrush,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Point triP1 = new Point(x1, y1);
                Point triP2 = new Point(x2, y2);
                Point triP3 = new Point(x3, y3);
                PointCollection pc = new PointCollection
                {
                    triP1,
                    triP2,
                    triP3
                };
                triangle.Points = pc;

                return triangle;
            }

            public Polygon triangle;

            public ZeroRollTickCache(SolidColorBrush HudBrush)
            {
                triangle = CreateTriangle(HudBrush, 0, 0, 12, -16, -12, -16);
            }
        }
        public class RollIndicatorCache
        {
            public Polygon triangle;
            public Polygon trapezoid;
            public Point trapP1;
            public Point trapP2;
            public Point trapP3;
            public Point trapP4;
            public PointCollection pcTrap;
            public RotateTransform triangleRenderTransform;// = new RotateTransform(rollAngle, 0, circleRad);
            public RotateTransform trapezoidRenderTransform;// = new RotateTransform(rollAngle, 0, circleRad);

            private Polygon CreateTriangle(SolidColorBrush HudBrush, double x1, double y1, double x2, double y2, double x3, double y3)
            {
                Polygon triangle = new Polygon
                {
                    Stroke = HudBrush,
                    Fill = HudBrush,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Point triP1 = new Point(x1, y1);
                Point triP2 = new Point(x2, y2);
                Point triP3 = new Point(x3, y3);
                PointCollection pc = new PointCollection
                {
                    triP1,
                    triP2,
                    triP3
                };
                triangle.Points = pc;

                return triangle;
            }

            public RollIndicatorCache(SolidColorBrush HudBrush)
            {
                triangleRenderTransform = new RotateTransform();
                trapezoidRenderTransform = new RotateTransform();

                triangle = CreateTriangle(HudBrush, 0, 0, 9, 12, -9, 12);

                trapezoid = new Polygon
                {
                    Stroke = HudBrush,
                    Fill = HudBrush,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };

                trapP1 = new Point(-12, 16);
                trapP2 = new Point(12, 16);
                trapP3 = new Point(15, 20);
                trapP4 = new Point(-15, 20);

                pcTrap = new PointCollection
                {
                    trapP1,
                    trapP2,
                    trapP3,
                    trapP4
                };

                trapezoid.Points = pcTrap;

            }

        }
        public class RollCache
        {
            public List<KeyValuePair<double, bool>> tickList;
            public RollCache(SolidColorBrush HudBrush)
            {
                tickList = new List<KeyValuePair<double, bool>>
                {
                    new KeyValuePair<double, bool>(10, false),
                    new KeyValuePair<double, bool>(20, false),
                    new KeyValuePair<double, bool>(30, true),
                    new KeyValuePair<double, bool>(45, false),
                    new KeyValuePair<double, bool>(60, true),

                    new KeyValuePair<double, bool>(-10, false),
                    new KeyValuePair<double, bool>(-20, false),
                    new KeyValuePair<double, bool>(-30, true),
                    new KeyValuePair<double, bool>(-45, false),
                    new KeyValuePair<double, bool>(-60, true),
                };
            }
        }
        public class AltitudeCache
        {
            public BorderTextLabel txtBlk;

            public AltitudeCache(SolidColorBrush HudBrush)
            {
                txtBlk = new BorderTextLabel
                {
                    Stroke = HudBrush,
                    Foreground = HudBrush,
                    FontSize = 12
                };
            }
        }
        public class ClimbRateCache
        {
            public BorderTextLabel txtBlk;
            public Line zeroLn;
            public Rectangle climbMagnitude;
            public RotateTransform triangleRenderTransform;
            public Polygon triangle;

            public ClimbRateCache(SolidColorBrush HudBrush)
            {
                triangleRenderTransform = new RotateTransform(180);

                triangle = CreateTriangle(HudBrush, -9, 0, 9, 0, 0, 12);
                triangle.RenderTransform = triangleRenderTransform;

                txtBlk = new BorderTextLabel
                {
                    Stroke = HudBrush,
                    Foreground = HudBrush,
                    FontSize = 12
                };

                zeroLn = new Line
                {
                    X1 = -10,
                    X2 = -40,
                    Y1 = 0,
                    Y2 = 00,
                    Stroke = HudBrush,
                    StrokeThickness = 1
                };

                climbMagnitude = new Rectangle
                {
                    Fill = HudBrush,
                    Width = 12
                };
            }

            private Polygon CreateTriangle(SolidColorBrush HudBrush, double x1, double y1, double x2, double y2, double x3, double y3)
            {
                Polygon triangle = new Polygon
                {
                    Stroke = HudBrush,
                    Fill = HudBrush,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Point triP1 = new Point(x1, y1);
                Point triP2 = new Point(x2, y2);
                Point triP3 = new Point(x3, y3);
                PointCollection pc = new PointCollection
                {
                    triP1,
                    triP2,
                    triP3
                };
                triangle.Points = pc;

                return triangle;
            }


        }
        public class SpeedAndGCache
        {
            public BorderTextLabel txtBlkGM;
            public BorderTextLabel txtBlkSpd;

            public SpeedAndGCache(SolidColorBrush HudBrush)
            {
                txtBlkGM = new BorderTextLabel
                {
                    Stroke = HudBrush,
                    Foreground = HudBrush,
                    FontSize = 12
                };

                txtBlkSpd = new BorderTextLabel
                {
                    Stroke = HudBrush,
                    Foreground = HudBrush,
                    FontSize = 12
                };
            }

        }
        public class AircraftCache
        {
            public Polyline waterline;

            public AircraftCache(SolidColorBrush HudBrush)
            {
                double segmentLength = 6;
                waterline = new Polyline
                {
                    Stroke = HudBrush,
                    StrokeThickness = 2
                };

                Point p1 = new Point(-4 * segmentLength, 0);
                Point p2 = new Point(-2 * segmentLength, 0);
                Point p3 = new Point(-segmentLength, segmentLength);
                Point p4 = new Point(0, 0);
                Point p5 = new Point(segmentLength, segmentLength);
                Point p6 = new Point(2 * segmentLength, 0);
                Point p7 = new Point(4 * segmentLength, 0);

                PointCollection pc = new PointCollection
                {
                    p1,
                    p2,
                    p3,
                    p4,
                    p5,
                    p6,
                    p7
                };

                waterline.Points = pc;
            }
        }
        public class FlightPathMarkerCache
        {
            public Ellipse body;
            public Line leftLn;
            public Line rightLn;
            public Line topLn;

            private const double FPM_RADIUS = 24;
            private const double FPM_LINE_LEN = 6;

            public FlightPathMarkerCache(SolidColorBrush HudBrush)
            {
                body = new Ellipse
                {
                    Stroke = HudBrush,
                    StrokeThickness = 2,
                    Width = FPM_RADIUS,
                    Height = FPM_RADIUS
                };

                leftLn = new Line
                {
                    X1 = 0,
                    X2 = -FPM_LINE_LEN,
                    Y1 = FPM_RADIUS / 2.0,
                    Y2 = FPM_RADIUS / 2.0,
                    Stroke = HudBrush,
                    StrokeThickness = 1
                };

                rightLn = new Line
                {
                    X1 = 0,
                    X2 = FPM_LINE_LEN,
                    Y1 = FPM_RADIUS / 2.0,
                    Y2 = FPM_RADIUS / 2.0,
                    Stroke = HudBrush,
                    StrokeThickness = 1
                };

                topLn = new Line
                {
                    X1 = 0,
                    X2 = 0,
                    Y1 = 0,
                    Y2 = -FPM_LINE_LEN,
                    Stroke = HudBrush,
                    StrokeThickness = 1
                };
            }
        }
        public class AlphaBetaCache
        {
            public BorderTextLabel txtBlkSpd;

            public AlphaBetaCache(SolidColorBrush HudBrush)
            {
                txtBlkSpd = new BorderTextLabel
                {
                    Stroke = HudBrush,
                    Foreground = HudBrush,
                    FontSize = 12
                };
            }
        }
    }
}