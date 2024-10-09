using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace EstrellaAccesoriosWpf.Common.Controls;

public class VirtualizingWrapPanel : VirtualizingPanel, IScrollInfo
{

    private double _horizontalOffset;
    private double _verticalOffset;
    private Size _viewportSize;
    private Size _extentSize;
    private readonly Dictionary<UIElement, Rect> _elementRects = new Dictionary<UIElement, Rect>();

    protected override Size MeasureOverride(Size availableSize)
    {
        double x = 0;
        double y = 0;
        double rowHeight = 0;

        foreach (UIElement child in InternalChildren)
        {
            child.Measure(availableSize);
            Size desiredSize = child.DesiredSize;

            if (x + desiredSize.Width > availableSize.Width)
            {
                x = 0;
                y += rowHeight;
                rowHeight = 0;
            }

            _elementRects[child] = new Rect(x, y, desiredSize.Width, desiredSize.Height);
            x += desiredSize.Width;
            rowHeight = Math.Max(rowHeight, desiredSize.Height);
        }

        _extentSize = new Size(availableSize.Width, y + rowHeight);
        return _extentSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double x = 0;
        double y = 0;
        double rowHeight = 0;

        foreach (UIElement child in InternalChildren)
        {
            Rect rect = _elementRects[child];
            child.Arrange(new Rect(new Point(rect.X, rect.Y - _verticalOffset), rect.Size));
            x += rect.Width;
            rowHeight = Math.Max(rowHeight, rect.Height);
        }

        return finalSize;
    }
    //LineHeight
    public void LineUp() => SetVerticalOffset(VerticalOffset - 0);
    public void LineDown() => SetVerticalOffset(VerticalOffset + 0);
    public void PageUp() => SetVerticalOffset(VerticalOffset - ViewportHeight);
    public void PageDown() => SetVerticalOffset(VerticalOffset + ViewportHeight);
    public void MouseWheelDown() => SetVerticalOffset(VerticalOffset + 0);
    public void MouseWheelUp() => SetVerticalOffset(VerticalOffset - 0);

    public double ExtentHeight => _extentSize.Height;
    public double ExtentWidth => _extentSize.Width;
    public double ViewportHeight => _viewportSize.Height;
    public double ViewportWidth => _viewportSize.Width;
    public double VerticalOffset => _verticalOffset;
    public double HorizontalOffset => _horizontalOffset;

    public void SetVerticalOffset(double offset)
    {
        _verticalOffset = offset;
        InvalidateArrange();
    }

    public void SetHorizontalOffset(double offset)
    {
        _horizontalOffset = offset;
        InvalidateArrange();
    }

    public ScrollViewer ScrollOwner { get; set; }
    bool IScrollInfo.CanHorizontallyScroll { get; set; } = true;
    bool IScrollInfo.CanVerticallyScroll { get; set; } = true;

    public void LineLeft() { }
    public void LineRight() { }
    public void PageLeft() { }
    public void PageRight() { }
    public void MouseWheelLeft() { }
    public void MouseWheelRight() { }

    public Rect MakeVisible(Visual visual, Rect rectangle)
    {
        throw new NotImplementedException();
    }
}
