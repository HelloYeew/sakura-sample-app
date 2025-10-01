using Sakura.Framework;
using Sakura.Framework.Graphics.Colors;
using Sakura.Framework.Graphics.Drawables;
using Sakura.Framework.Graphics.Primitives;
using Sakura.Framework.Maths;

namespace SampleApp;

public class SampleApp : App
{
    public override void Load()
    {
        base.Load();
        Root.Add(new Box()
        {
            Size = new Vector2(1,1),
            Color = Color.Red,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both
        });
        Root.Add(new Box()
        {
            Size = new Vector2(0.2f, 0.2f),
            Color = Color.PaleVioletRed,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both
        });
        Root.Add(new Box()
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
        Root.Add(new Box()
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
        Root.Add(new Box()
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
        Root.Add(new Box()
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
        Root.Add(new Box()
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
        Root.Add(new Box()
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
    }
}
