using System;
using System.Collections.Generic;
using Sakura.Framework;
using Sakura.Framework.Allocation;
using Sakura.Framework.Audio;
using Sakura.Framework.Configurations;
using Sakura.Framework.Graphics.Colors;
using Sakura.Framework.Graphics.Drawables;
using Sakura.Framework.Graphics.Primitives;
using Sakura.Framework.Extensions.ColorExtensions;
using Sakura.Framework.Extensions.DrawableExtensions;
using Sakura.Framework.Graphics.Transforms;
using Sakura.Framework.Input;
using Sakura.Framework.Logging;
using Sakura.Framework.Maths;
using Sakura.Framework.Platform;
using Sakura.Framework.Reactive;

namespace SampleApp;

public class SampleApp : App
{
    private readonly List<DummyBox> boxes = new List<DummyBox>();
    private ITrack backgroundTrack;
    private IAudioChannel backgroundTrackChannel;

    private Box volumeUpBox;
    private Box volumeDownBox;

    private Reactive<double> masterVolume;

    protected override string ResourceRootNamespace => "SampleApp.Resources";

    public override void Load()
    {
        base.Load();
        backgroundTrack = TrackStore.Get("audio.mp3");

        masterVolume = Host.FrameworkConfigManager.Get<double>(FrameworkSetting.MasterVolume);

        Add(new Box()
        {
            Size = new Vector2(1),
            Color = ColorExtensions.FromHex("FF66AA"),
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both
        });
        Add(new Triangle()
        {
            Size = new Vector2(300, 300),
            Color = Color.PaleVioletRed.Lighten(0.25f),
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        });
        Add(new Line()
        {
            StartPoint = new Vector2(0, 0),
            EndPoint = new Vector2(1, 1),
            Thickness = 5,
            Color = Color.White,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
        });
        Add(volumeUpBox = new Box()
        {
            Size = new Vector2(100, 100),
            Color = Color.Green,
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            Margin = new MarginPadding()
            {
                Left = 20
            }
        });
        Add(volumeDownBox = new Box()
        {
            Size = new Vector2(100, 100),
            Color = Color.Red,
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

        Add(new Container()
        {
            Size = new Vector2(200, 200),
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Masking = true,
            CornerRadius = 25,
            Child = new Box()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Color = Color.Red,
                Size = new Vector2(1),
                Depth = int.MaxValue
            }
        });

        Add(new Container()
        {
            Size = new Vector2(100, 100),
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Masking = true,
            CornerRadius = 5,
            Children = new Drawable[]
            {
                new Box()
                {
                    Size = new Vector2(50, 50),
                    Color = Color.OrangeRed
                },
                new Triangle()
                {
                    Size = new Vector2(50, 50),
                    Color = Color.Yellow,
                    Position = new Vector2(25, 25)
                }
            }
        });
    }

    public override void LoadComplete()
    {
        backgroundTrack.RestartPoint = 119199;
        backgroundTrackChannel = backgroundTrack.Play();
        backgroundTrackChannel.Looping = true;
        base.LoadComplete();
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
        if (e.Key == Key.Up && IsLoaded)
        {
            masterVolume.Value = Math.Min(1.0, masterVolume.Value + 0.05);
            volumeUpBox.FlashColour(Color.White, 100, Easing.OutCubic);
        }

        if (e.Key == Key.Down && IsLoaded)
        {
            masterVolume.Value = Math.Max(0.0, masterVolume.Value - 0.05);
            volumeDownBox.FlashColour(Color.White, 100, Easing.OutCubic);
        }

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
    private ISample hoverSample;

    public DummyBox(Color color)
    {
        Color = color;
        originalColor = color;
        clickCount = new ReactiveInt(0);
        clickCount.ValueChanged += count => Logger.LogPrint($"Box {id} clicked {count.NewValue} times!");
    }

    [BackgroundDependencyLoader]
    private void load(IWindow window, IAudioStore<ISample> sampleStore)
    {
        Logger.LogPrint("Current window size: " + window.Width + "x" + window.Height);
        hoverSample = sampleStore.Get("hover.wav");
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

        double everythingGoesOnTransformDuration = 60000 / 157;

        this.RotateTo(0).Then()
            .FlashColour(originalColor.Lighten(0.8f), everythingGoesOnTransformDuration, Easing.OutCubic)
            .RotateTo(360, everythingGoesOnTransformDuration, Easing.OutCubic)
            .ScaleTo(0.7f, everythingGoesOnTransformDuration, Easing.OutCubic)
            .Loop();
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

    public override bool OnDrag(MouseEvent e)
    {
        if (RelativeSizeAxes == Axes.Both)
            Position += e.Delta;
        else
            Position += e.Delta / Parent!.ChildSize;

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

    public override bool OnHover(MouseEvent e)
    {
        hoverSample.Play();
        return base.OnHover(e);
    }
}
