using Sakura.Framework;
using Sakura.Framework.Graphics.Colors;
using Sakura.Framework.Graphics.Containers;
using Sakura.Framework.Graphics.Drawables;
using Sakura.Framework.Graphics.Primitives;
using Sakura.Framework.Maths;

namespace SampleApp;

public class FlowTestApp : App
{
    public override void Load()
    {
        base.Load();

        var horizontalFlowWrapper = new Container
        {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Size = new Vector2(0.9f, 0.45f),
            Position = new Vector2(0.05f, 0.05f),

            Children = new Drawable[]
            {
                new Box
                {
                    Size = new Vector2(1),
                    RelativeSizeAxes = Axes.Both,
                    Color = Color.DarkGray,
                },
                new FlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FlowDirection.Horizontal,
                    Size = new Vector2(1),
                    Spacing = new Vector2(10, 10),
                    Padding = new MarginPadding(15),

                    Children = new Drawable[]
                    {
                        new Box { Size = new Vector2(100, 100), Color = Color.Red },
                        new Box { Size = new Vector2(50, 50), Color = Color.Blue },
                        new Box { Size = new Vector2(100, 70), Color = Color.Green, Margin = new MarginPadding { Left = 20 } },
                        new Box { Size = new Vector2(150, 50), Color = Color.Yellow },
                        new Box { Size = new Vector2(200, 50), Color = Color.Orange },
                        new Box { Size = new Vector2(100, 100), Color = Color.Purple },
                        new Box { Size = new Vector2(100, 50), Color = Color.White },
                    }
                }
            }
        };
        Add(horizontalFlowWrapper);

        var verticalFlowWrapper = new Container
        {
            Anchor = Anchor.BottomLeft,
            Origin = Anchor.BottomLeft,
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Size = new Vector2(0.9f, 0.45f),
            Position = new Vector2(0.05f, -0.05f),

            Children = new Drawable[]
            {
                // Background
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1),
                    Color = Color.DarkGray,
                },
                // Flow Container
                new FlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FlowDirection.Vertical,
                    Size = new Vector2(1),
                    Spacing = new Vector2(5, 5),
                    Padding = new MarginPadding(10),

                    Children = new Drawable[]
                    {
                        new Box { Size = new Vector2(100, 100), Color = Color.Red },
                        new Box { Size = new Vector2(50, 50), Color = Color.Blue, Margin = new MarginPadding { Top = 10 } },
                        new Box { Size = new Vector2(100, 70), Color = Color.Green },
                        new Box { Size = new Vector2(150, 50), Color = Color.Yellow },
                        new Box { Size = new Vector2(200, 50), Color = Color.Orange },
                        new Box { Size = new Vector2(100, 100), Color = Color.Purple },
                        new Box { Size = new Vector2(100, 50), Color = Color.White },
                    }
                }
            }
        };
        Add(verticalFlowWrapper);
    }
}
