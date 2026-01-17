
using Gtk;
using static Gtk.Orientation;

class FormWindow : Window
{
    Notebook notebook = Notebook.New();

    public FormWindow(Application? app) : base()
    {
        Application = app;
        Title = "Window";

        SetDefaultSize(800, 800);

        Box vBox = Box.New(Vertical, 0);
        Child = vBox;

        Button button = Button.NewWithLabel("Button");
        button.OnClicked += A;

        Box hBox = Box.New(Horizontal, 10);
        hBox.Append(button);

        vBox.Append(hBox);
        vBox.Append(notebook);
    }

    void A(Button button, EventArgs args)
    {
        Box vBox = Box.New(Vertical, 0);

        Box hBox = Box.New(Horizontal, 0);
        vBox.Append(hBox);

        List<Data> List = [];
        for (int i = 0; i <10; i++)
        {
            Data data = new($"Name {i}");

            for (int j = 0; j < 200; j++)
                data.Value.Add($"Child {i}-{j}", new($"Name {i}"));

            List.Add(data);
        }

        vBox.Append(new ConfiguratorDocumentsTree(List).Fill());

        notebook.AppendPage(vBox, Label.New("Page"));
    }
}