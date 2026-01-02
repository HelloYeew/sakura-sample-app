// This code is part of the Sakura framework project. Licensed under the MIT License.
// See the LICENSE file for full license text.

using Sakura.Framework;
using Sakura.Framework.Extensions.ColorExtensions;
using Sakura.Framework.Graphics.Containers;
using Sakura.Framework.Graphics.Drawables;
using Sakura.Framework.Graphics.Primitives;
using Sakura.Framework.Input;
using Sakura.Framework.Logging;
using Sakura.Framework.Maths;

namespace SampleApp;

public class ScrollContainerApp : App
{
    private FlowContainer flowContainer;
    private ScrollableContainer scrollContainer;

    public override void Load()
    {
        base.Load();

        Add(new Box()
        {
            Size = new Vector2(1),
            Color = ColorExtensions.FromHex("FF66AA"),
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both
        });

        scrollContainer = new ScrollableContainer()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(0.5f, 0.75f),
            RelativeSizeAxes = Axes.Both,
            Direction = ScrollDirection.Both
        };

        flowContainer = new FlowContainer()
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Spacing = new Vector2(10),
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Width = 1.5f
            // Size = new Vector2(1, 0)
        };

        scrollContainer.Add(flowContainer);

        Add(scrollContainer);

        Logger.Verbose("FlowConrainer size before adding boxes: " + flowContainer.Size);
        Logger.Verbose("ScrollContainer size before adding boxes: " + scrollContainer.Size);
    }

    public override bool OnKeyDown(KeyEvent e)
    {
        if (e.Key == Key.Space)
        {
            flowContainer.Add(new Box()
            {
                Size = new Vector2(200),
                Color = ColorExtensions.FromHex("66FFAA")
            });
            Logger.Verbose("Added new box to FlowContainer");
            Logger.Verbose("FlowContainer size is now: " + flowContainer.Size);
            Logger.Verbose("ScrollContainer size is now: " + scrollContainer.Size);
            Logger.Verbose($"ScrollContainer Pixels: {scrollContainer.DrawSize}");
            Logger.Verbose($"FlowContainer Pixels: {flowContainer.DrawSize}");
            return true;
        }
        return base.OnKeyDown(e);
    }
}
