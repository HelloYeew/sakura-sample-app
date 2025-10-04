using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sakura.Framework;
using Sakura.Framework.Graphics.Colors;
using Sakura.Framework.Graphics.Drawables;
using Sakura.Framework.Graphics.Primitives;
using Sakura.Framework.Extensions.ColorExtensions;
using Sakura.Framework.Input;
using Sakura.Framework.Logging;
using Sakura.Framework.Maths;
using Sakura.Framework.Reactive;

namespace SampleApp;

public class SampleApp : App
{
    private List<DummyBox> boxes = new List<DummyBox>();

    private Box backgroundBox;

    public override void Load()
    {
        base.Load();
        Add(backgroundBox = new Box()
        {
            Size = new Vector2(1,1),
            Color = Color.OrangeRed,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both
        });
        Add(new DummyBox(Color.PaleVioletRed)
        {
            Size = new Vector2(0.2f, 0.2f),
            Color = Color.PaleVioletRed,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
        });
        Add(new Box()
        {
            Size = new Vector2(100, 100),
            Color = Color.AliceBlue,
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            Margin = new MarginPadding()
            {
                Left = 20
            }
        });
        Add(new Box()
        {
            Size = new Vector2(300, 200),
            Color = Color.MediumSpringGreen,
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Depth = -100,
            Margin = new MarginPadding()
            {
                Right = 20
            }
        });
        Add(new Box()
        {
            Size = new Vector2(100, 100),
            Color = Color.LawnGreen,
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Margin = new MarginPadding()
            {
                Right = 20
            }
        });
        Add(new Box()
        {
            Size = new Vector2(100, 100),
            Color = Color.CornflowerBlue,
            Anchor = Anchor.TopCentre,
            Origin = Anchor.TopCentre,
            Margin = new MarginPadding()
            {
                Top = 20
            }
        });
        Add(new Box()
        {
            Size = new Vector2(100, 100),
            Color = Color.Gold,
            Anchor = Anchor.BottomCentre,
            Origin = Anchor.BottomCentre,
            Margin = new MarginPadding()
            {
                Bottom = 20
            }
        });
        Add(new Box()
        {
            Size = new Vector2(50, 50),
            Color = Color.HotPink,
            Anchor = Anchor.BottomCentre,
            Origin = Anchor.BottomCentre,
            Margin = new MarginPadding()
            {
                Bottom = 20
            }
        });

        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(3000);
                Color randomColor = ColorExtensions.FromRgb(
                    (byte)Random.Shared.Next(0, 256),
                    (byte)Random.Shared.Next(0, 256),
                    (byte)Random.Shared.Next(0, 256)
                );
                backgroundBox.Color = randomColor;
            }
        }, CancellationToken.None);
    }

    public override bool OnMouseDown(MouseButtonEvent e)
    {
        if (e.Button != MouseButton.Right) return base.OnMouseDown(e);

        Color randomColor = ColorExtensions.FromRgb(
            (byte)Random.Shared.Next(0, 256),
            (byte)Random.Shared.Next(0, 256),
            (byte)Random.Shared.Next(0, 256)
        );
        var anchorPosition = ChildSize * GetAnchorOriginVector(Anchor.Centre);
        int randomSize = Random.Shared.Next(50, 150);
        DummyBox newBox = new DummyBox(randomColor)
        {
            Size = new Vector2(randomSize),
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativePositionAxes = Axes.Both,
            Position = (e.ScreenSpaceMousePosition - anchorPosition) / ChildSize
        };
        boxes.Add(newBox);
        Add(newBox);
        return base.OnMouseDown(e);
    }

    public override bool OnScroll(ScrollEvent e)
    {
        foreach (var box in boxes)
        {
            box.Size += e.ScrollDelta;
        }
        return base.OnScroll(e);
    }

    public override bool OnKeyDown(KeyEvent e)
    {
        foreach (var box in boxes)
        {
            if (box.IsHovered && e.Key == Key.BackSpace)
            {
                Remove(box);
                boxes.Remove(box);
                break;
            }
        }
        return base.OnKeyDown(e);
    }
}

public class DummyBox : Box
{
    private Color originalColor;
    private ReactiveInt clickCount;
    private Guid id = Guid.NewGuid();

    public DummyBox(Color color)
    {
        Color = color;
        originalColor = color;
        clickCount = new ReactiveInt(0);
        clickCount.ValueChanged += count => Logger.LogPrint($"Box {id} clicked {count.NewValue} times!");
    }

    public override void Load()
    {
        Logger.Verbose($"Loading box {id} with color: {Color}");
        base.Load();
    }

    public override void LoadComplete()
    {
        Logger.Verbose($"Box loaded {id} with color: {Color}");
        base.LoadComplete();
    }

    public override bool OnMouseDown(MouseButtonEvent e)
    {
        Color = originalColor.Darken(0.5f);
        return base.OnMouseDown(e);
    }

    public override bool OnMouseUp(MouseButtonEvent e)
    {
        Color = originalColor;
        return base.OnMouseUp(e);
    }

    public override bool OnHover(MouseEvent e)
    {
        Color = originalColor.Lighten(0.5f);
        return base.OnHover(e);
    }

    public override bool OnHoverLost(MouseEvent e)
    {
        base.OnHoverLost(e);
        Color = originalColor;
        return true;
    }

    public override bool OnDrag(MouseEvent e)
    {
        if (RelativeSizeAxes == Axes.Both)
            Position += e.Delta;
        else
            Position += e.Delta / Parent.ChildSize;

        return true;
    }

    public override bool OnClick(MouseButtonEvent e)
    {
        clickCount.Value++;
        return base.OnClick(e);
    }

    public override bool OnDoubleClick(MouseButtonEvent e)
    {
        clickCount.Value = 0;
        return base.OnDoubleClick(e);
    }
}
