
using Gtk;
using static Gtk.Orientation;

class FormWindow : Window
{
    public FormWindow(Application? app) : base()
    {
        Application = app;
        Title = "Window";

        SetDefaultSize(500, 300);

        Box vBox = Box.New(Vertical, 0);
        vBox.MarginTop = vBox.MarginBottom = vBox.MarginStart = vBox.MarginEnd = 10;
        Child = vBox;

        Button button = Button.NewWithLabel("Button");
        button.OnClicked += A;

        Box hBox = Box.New(Horizontal, 10);
        hBox.Append(button);

        vBox.Append(hBox);
    }

    void A(Button button, EventArgs args)
    {
        Popover popover = Popover.New();
        popover.Position = PositionType.Right;
        popover.SetParent(button);
        popover.WidthRequest = 300;
        popover.HeightRequest = 600;

        Box vBox = Box.New(Vertical, 0);
        popover.Child = vBox;

        Box hBox = Box.New(Horizontal, 0);
        vBox.Append(hBox);

        Label label = Label.New("Text");
        hBox.Append(label);

        List<Data> List = [];
        for (int i = 0; i < 10; i++)
        {
            Data data = new()
            {
                Name = $"Name {i}",
                Value = new Dictionary<string, Data>() { { $"Child {i}", new() } }
            };

            for (int j = 0; j < 100; j++)
            {
                data.Value.Add($"Child {i}-{j}", new());
            }

            List.Add(data);
        }

        vBox.Append(new ConfiguratorDocumentsTree(List).Fill());
        popover.Show();
    }
}