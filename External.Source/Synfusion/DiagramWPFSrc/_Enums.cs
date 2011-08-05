// <copyright file="_Enums.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syncfusion.Windows.Diagram
{
    /// <summary>
    /// Specifies the decorator shapes.
    /// </summary>
    public enum DecoratorShape
    {
        /// <summary>
        /// None shape
        /// </summary>
        None,

        /// <summary>
        /// Arrow shape
        /// </summary>
        Arrow,

        /// <summary>
        /// Diamond shape
        /// </summary>
        Diamond,

        /// <summary>
        /// Circle shape
        /// </summary>
        Circle,

        /// <summary>
        /// Custom Shape
        /// </summary>
        Custom
    }

    /// <summary>
    /// Specifies the Port shape.
    /// </summary>
    public enum PortShapes
    {
        /// <summary>
        /// None shape
        /// </summary>
        None,

        /// <summary>
        /// Arrow shape
        /// </summary>
        Arrow,

        /// <summary>
        /// Diamond shape
        /// </summary>
        Diamond,

        /// <summary>
        /// Circle shape
        /// </summary>
        Circle,
        /// <summary>
        /// Custom Shape
        /// </summary>
        Custom
    }

    /// <summary>
    /// Specifies the layout types.
    /// </summary>
    public enum LayoutType
    {
        /// <summary>
        /// None type.
        /// </summary>
        None,

        /// <summary>
        /// Directed tree layout.
        /// </summary>
        DirectedTreeLayout,

        /// <summary>
        /// Hierarchical tree layout.
        /// </summary>
        HierarchicalTreeLayout,

        /// <summary>
        /// Table layout.
        /// </summary>
        TableLayout,

		/// <summary>
        /// Radial tree layout.
        /// </summary>
        RadialTreeLayout
    }

    /// <summary>
    /// Specifies the tree orientation .
    /// </summary>
    public enum TreeOrientation
    {
        /// <summary>
        /// Left to right
        /// </summary>
        LeftRight,

        /// <summary>
        /// Right to left
        /// </summary>
        RightLeft,

        /// <summary>
        /// Top to Bottom
        /// </summary>
        TopBottom,

        /// <summary>
        /// Bottom to top
        /// </summary>
        BottomTop,
    }

    /// <summary>
    /// Specifies the Expand Mode.
    /// </summary>
    public enum ExpandMode
    {
        /// <summary>
        /// Horizontal expansion.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Vertical expansion.
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Specifies the shapes.
    /// </summary>
    public enum Shapes
    {
        /// <summary>
        /// Rectangle shape
        /// </summary>
        Rectangle,

        /// <summary>
        /// Star shape
        /// </summary>
        Star,

        /// <summary>
        /// Hexagon shape
        /// </summary>
        Hexagon,

        /// <summary>
        /// Octagon shape
        /// </summary>
        Octagon,

        /// <summary>
        /// Pentagon shape
        /// </summary>
        Pentagon,

        /// <summary>
        /// Heptagon shape
        /// </summary>
        Heptagon,

        /// <summary>
        /// Triangle shape
        /// </summary>
        Triangle,

        /// <summary>
        /// Ellipse shape
        /// </summary>
        Ellipse,

        /// <summary>
        /// Plus shape
        /// </summary>
        Plus,

        /// <summary>
        /// Rounded Rectangle
        /// </summary>
        RoundedRectangle,

        /// <summary>
        /// Rounded Square
        /// </summary>
        RoundedSquare,

        /// <summary>
        /// Right Triangle
        /// </summary>
        RightTriangle,

        /// <summary>
        /// ThreeDBox shape
        /// </summary>
        ThreeDBox,

        /// <summary>
        /// FlowChart Process shape
        /// </summary>
        FlowChart_Process,

        /// <summary>
        /// FlowChart Start shape
        /// </summary>
        FlowChart_Start,

        /// <summary>
        /// FlowChart Decision shape
        /// </summary>
        FlowChart_Decision,

        /// <summary>
        /// FlowChart_Predefined shape
        /// </summary>
        FlowChart_Predefined,

        /// <summary>
        /// FlowChart_StoredData shape
        /// </summary>
        FlowChart_StoredData,

        /// <summary>
        /// FlowChart_Document shape
        /// </summary>
        FlowChart_Document,

        /// <summary>
        /// FlowChart_Data shape
        /// </summary>
        FlowChart_Data,

        /// <summary>
        /// FlowChart_InternalStorage shape
        /// </summary>
        FlowChart_InternalStorage,

        /// <summary>
        /// FlowChart_PaperTape shape
        /// </summary>
        FlowChart_PaperTape,

        /// <summary>
        /// FlowChart_SequentialData shape
        /// </summary>
        FlowChart_SequentialData,

        /// <summary>
        /// FlowChart_DirectData shape
        /// </summary>
        FlowChart_DirectData,

        /// <summary>
        /// FlowChart_ManualInput shape
        /// </summary>
        FlowChart_ManualInput,

        /// <summary>
        /// FlowChart_Card shape
        /// </summary>
        FlowChart_Card,

        /// <summary>
        /// FlowChart_Delay shape
        /// </summary>
        FlowChart_Delay,

        /// <summary>
        /// FlowChart_Terminator shape
        /// </summary>
        FlowChart_Terminator,

        /// <summary>
        /// FlowChart_Display shape
        /// </summary>
        FlowChart_Display,

        /// <summary>
        /// FlowChart_LoopLimit shape
        /// </summary>
        FlowChart_LoopLimit,

        /// <summary>
        /// FlowChart_Preparation shape
        /// </summary>
        FlowChart_Preparation,

        /// <summary>
        /// FlowChart_ManualOperation shape
        /// </summary>
        FlowChart_ManualOperation,

        /// <summary>
        /// FlowChart_OffPageReference shape
        /// </summary>
        FlowChart_OffPageReference,

        /// <summary>
        /// FlowChart_Star shape
        /// </summary>
        FlowChart_Star,

        /// <summary>
        /// Default shape
        /// </summary>
        Default,

        /// <summary>
        /// CustomPath shape
        /// </summary>
        CustomPath
    }

    /// <summary>
    /// Specifies the units.
    /// </summary>
    public enum MeasureUnits
    {
        /// <summary>
        /// Pixel unit
        /// </summary>
        Pixel = 0,

        /// <summary>
        /// Points unit
        /// </summary>
        Point,

        /// <summary>
        /// Document unit
        /// </summary>
        Document,

        /// <summary>
        /// Display unit
        /// </summary>
        Display,

        /// ENGLISH
        /// <summary>
        /// Sixteenth Inches
        /// </summary>
        SixteenthInch,

        /// <summary>
        /// Eighth Inches
        /// </summary>
        EighthInch,

        /// <summary>
        /// Quarter Inches
        /// </summary>
        QuarterInch,

        /// <summary>
        /// Half Inches
        /// </summary>
        HalfInch,

        /// <summary>
        /// Inches unit
        /// </summary>
        Inch,

        /// <summary>
        /// Feet measurement unit
        /// </summary>
        Foot,

        /// <summary>
        /// Yards measurement unit
        /// </summary>
        Yard,

        /// <summary>
        /// Miles measurement unit
        /// </summary>
        Mile,

        /// METRIC
        /// <summary>
        /// Millimeters unit
        /// </summary>
        Millimeter,

        /// <summary>
        /// Centimeters unit
        /// </summary>
        Centimeter,

        /// <summary>
        /// Meters measurement unit
        /// </summary>
        Meter,

        /// <summary>
        /// Kilometers measurement unit
        /// </summary>
        Kilometer,
    }

    /// <summary>
    /// Specifies the connector types.
    /// </summary>
    public enum ConnectorType
    {
        /// <summary>
        /// Orthogonal line
        /// </summary>
        Orthogonal,

        /// <summary>
        /// Bezier line
        /// </summary>
        Bezier,

        /// <summary>
        /// Straight line
        /// </summary>
        Straight
    }

    /// <summary>
    /// Specifies the intersection mode.
    /// </summary>
    public enum IntersectionMode
    {
        /// <summary>
        /// Connects to the content.
        /// </summary>
        OnContent,

        /// <summary>
        /// Connects to outer boudaries
        /// </summary>
        OnBorder
    }

    /// <summary>
    /// Specifies Theme styles.
    /// </summary>
    public enum Themes
    {
        /// <summary>
        /// Default Theme
        /// </summary>
        Default,

        /// <summary>
        /// Custom Theme
        /// </summary>
        Custom,

        /// <summary>
        /// OutLine Themes
        /// </summary>
        OutLineBlack, 
        OutLineBlue,
        OutLineRed,
        OutLineOliveGreen,
        OutLinePurple,
        OutLineAqua, 
        OutLineOrange,

        /// <summary>
        /// Fill Themes
        /// </summary>
        FillBlack, 
        FillBlue,
        FillRed, 
        FillOliveGreen,
        FillPurple, 
        FillAqua,
        FillOrange,

        /// <summary>
        /// OutLine Fill Themes
        /// </summary>
        OutLineFillBlack,
        OutLineFillBlue, 
        OutLineFillRed, 
        OutLineFillOliveGreen, 
        OutLineFillPurple, 
        OutLineFillAqua, 
        OutLineFillOrange,

        /// <summary>
        /// SubtleEffect Themes
        /// </summary>
        SubtleEffectBlack, 
        SubtleEffectBlue,
        SubtleEffectRed,
        SubtleEffectOliveGreen,
        SubtleEffectPurple,
        SubtleEffectAqua,
        SubtleEffectOrange,

        /// <summary>
        /// ModerateEffect Themes
        /// </summary>
        ModerateEffectBlack,
        ModerateEffectBlue,
        ModerateEffectRed,
        ModerateEffectOliveGreen,
        ModerateEffectPurple,
        ModerateEffectAqua,
        ModerateEffectOrange,
    }

    /// <summary>
    /// Specifies Command's Parameters.
    /// </summary>
    public enum SelectionFilter
    {
        /// <summary>
        /// Both Nodes and Line Connectors
        /// </summary>
        None,

        /// <summary>
        /// Only Nodes
        /// </summary>
        FilterLines,

        /// <summary>
        /// Only Line Connectors
        /// </summary>
        FilterNodes
    }

    public enum DrawingTools
    {
        Ellipse,

        Rectangle,

        RoundedRectangle,

        Polygon,

        StraightLine,

        BezierLine,

        PolyLine,

        OrthogonalLine
    }


}
