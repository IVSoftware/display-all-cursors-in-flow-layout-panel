Reading your code carefully, if I'm following correctly your objective is to have a `FlowLayoutPanel` that displays all the available cursors. What we end up with here could be considered an [X-Y Problem](https://meta.stackexchange.com/a/66378) because doing that is straightforward, but the manner in which you're trying to solve your problem is not. 

Please allow me to steer the conversation back to what you were trying to do to begin with:

[![screenshot][1]][1]

***


To achieve this objective, use reflection to obtain the cursors from the `Cursors` class, draw each one, and place a corresponding button in the `FlowLayoutPanel`.

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
                    // Avoid confusion - see note.
                    button.MouseHover += (sender, e) => Cursor = Cursors.Default;
                    flowLayoutPanel.Controls.Add(button);
                }
            }
        }

By attaching a `Click` event to each button, we can change the current `Cursor` to the one clicked. To avoid visual confusion, if a different button is hovered over then return to the default cursor to more clearly indicate that the new style can now be clicked.

        private void onAnyClickCursorButton(object? sender, EventArgs e)
        {
            if((sender is Button button) && (button.Tag is Cursor cursor)) 
            {
                Cursor = cursor;
            }
        }
    }


  [1]: https://i.stack.imgur.com/3SBtK.png