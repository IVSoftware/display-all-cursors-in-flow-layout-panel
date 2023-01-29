using System.Reflection;

namespace draw_cursors_00
{
    public partial class MainForm : Form
    {
        public MainForm()=>InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            foreach (var pi in typeof(Cursors).GetProperties(BindingFlags.Static | BindingFlags.Public))
            {
                if(pi.GetValue(null) is Cursor cursor)
                {
                    var image = new Bitmap(100, 100);
                    using (var graphics = Graphics.FromImage(image))
                    {
                        cursor.DrawStretched(
                            graphics,
                            new Rectangle(new Point(), new Size(100, 100)));
                    }
                    var button = new Button
                    {
                        Size = image.Size,
                        BackgroundImage = image,
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Margin = new Padding(5),
                        Tag = cursor,
                    };
                    button.Click += onAnyClickCursorButton;
                    button.MouseHover += (sender, e) => Cursor = Cursors.Default;
                    flowLayoutPanel.Controls.Add(button);
                }
            }
        }

        private void onAnyClickCursorButton(object? sender, EventArgs e)
        {
            if((sender is Button button) && (button.Tag is Cursor cursor)) 
            {
                Cursor = cursor;
            }
        }
    }
}