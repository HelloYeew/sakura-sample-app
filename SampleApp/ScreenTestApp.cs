// This code is part of the Sakura framework project. Licensed under the MIT License.
// See the LICENSE file for full license text.

using System;
using Sakura.Framework;
using Sakura.Framework.Extensions.ColorExtensions;
using Sakura.Framework.Extensions.DrawableExtensions;
using Sakura.Framework.Graphics.Colors;
using Sakura.Framework.Graphics.Containers;
using Sakura.Framework.Graphics.Drawables;
using Sakura.Framework.Graphics.Primitives;
using Sakura.Framework.Graphics.Screens;
using Sakura.Framework.Graphics.Transforms;
using Sakura.Framework.Logging;
using Sakura.Framework.Maths;

namespace SampleApp;

public class ScreenTestApp : App
{
    private ScreenStack mainScreenStack;

    public override void Load()
    {
        base.Load();
        mainScreenStack = new ScreenStack()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(0.75f),
            RelativeSizeAxes = Axes.Both,
        };
        Add(mainScreenStack);
        mainScreenStack.Push(new DummyScreen()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(1),
            RelativeSizeAxes = Axes.Both
        });

        Add(new ClickableContainer()
        {
            Anchor = Anchor.BottomRight,
            Origin = Anchor.BottomRight,
            Size = new Vector2(100, 50),
            Position = new Vector2(-10, -10),
            Child = new Box()
            {
                RelativeSizeAxes = Axes.Both,
                Color = Color.Red,
                Size = new Vector2(1),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            },
            Action = () =>
            {
                Logger.Verbose("Exiting current screen");
                mainScreenStack.Exit();
            }
        });

        Add(new ClickableContainer()
        {
            Anchor = Anchor.BottomLeft,
            Origin = Anchor.BottomLeft,
            Size = new Vector2(100, 50),
            Position = new Vector2(10, -10),
            Child = new Box()
            {
                RelativeSizeAxes = Axes.Both,
                Color = Color.Green,
                Size = new Vector2(1),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            },
            Action = () =>
            {
                Logger.Verbose("Pushing new DummyScreen");
                mainScreenStack.Push(new DummyScreen());
            }
        });

        Add(new ClickableContainer()
        {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Size = new Vector2(100, 50),
            Position = new Vector2(10, 10),
            Child = new Box()
            {
                RelativeSizeAxes = Axes.Both,
                Color = Color.Blue,
                Size = new Vector2(1),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            },
            Action = () =>
            {
                Logger.Verbose("Pushing new SwipingScreen");
                mainScreenStack.Push(new SwipingScreen());
            }
        });

        Add(new ClickableContainer()
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Size = new Vector2(100, 50),
            Position = new Vector2(10, 10),
            Child = new Box()
            {
                RelativeSizeAxes = Axes.Both,
                Color = Color.Yellow,
                Size = new Vector2(1),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            },
            Action = () =>
            {
                Logger.Verbose("Exiting current screen");
                mainScreenStack.Exit();
            }
        });
    }
}

public class DummyScreen : Screen
{
    private const int transition_time = 500;

    public override void Load()
    {
        base.Load();
        Anchor = Anchor.Centre;
        Origin = Anchor.Centre;
        RelativeSizeAxes = Axes.Both;
        Size = new Vector2(1);

        // Start transparent. The position will be set in OnEntering.
        Alpha = 0f;

        Add(new Box()
        {
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(1),
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Color = ColorExtensions.FromRgb(
                (byte)Random.Shared.Next(0, 256),
                (byte)Random.Shared.Next(0, 256),
                (byte)Random.Shared.Next(0, 256)
            )
        });
    }

    public override void OnEntering(Screen? last)
    {
        Position = new Vector2(0, 1);
        Alpha = 0;
        this.MoveTo(Vector2.Zero, transition_time, Easing.OutQuint);
        this.FadeIn(transition_time, Easing.OutQuint);
    }

    public override void OnExiting(Screen? next)
    {
        this.MoveTo(new Vector2(0, 1), transition_time, Easing.InQuint);
        this.FadeOut(transition_time, Easing.InQuint);
    }

    public override void OnResuming(Screen? last)
    {
        this.FadeIn(transition_time / 2);
    }

    public override void OnSuspending(Screen next)
    {
        this.FadeOut(transition_time / 2);
    }
}

public class SwipingScreen : Screen
{
    public override void Load()
    {
        base.Load();
        Anchor = Anchor.Centre;
        Origin = Anchor.Centre;
        RelativeSizeAxes = Axes.Both;
        Size = new Vector2(1);
        Scale = new Vector2(0);
        Alpha = 0f;
        Add(new Box()
        {
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(1),
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Color = ColorExtensions.FromRgb(
                (byte)Random.Shared.Next(0, 256),
                (byte)Random.Shared.Next(0, 256),
                (byte)Random.Shared.Next(0, 256)
            )
        });
    }

    public override void OnEntering(Screen? last)
    {
        this.ScaleTo(1f, 300, Easing.OutQuint)
            .FadeIn(300);
    }

    public override void OnExiting(Screen? next)
    {
        this.ScaleTo(0f, 300, Easing.InQuint)
            .FadeOut(300);
    }
}
