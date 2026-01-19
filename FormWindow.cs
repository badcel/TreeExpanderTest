using Gtk;
using static Gtk.Orientation;

class FormWindow : Window
{
    Notebook notebook = Notebook.New();

    public FormWindow(Application? app)
    {
        Application = app;
        Title = "Window";
        SetDefaultSize(800, 800);

        Button button = Button.NewWithLabel("Button");
        button.OnClicked += OnClicked;
        
        var vBox = Box.New(Vertical, 0);
        vBox.Append(button);
        vBox.Append(notebook);
        
        Child = vBox;
    }

    private void OnClicked(Button button, EventArgs args)
    {
        notebook.AppendPage(Fill(), Label.New("Page"));
    }

    private static Widget Fill()
    {
        List<Data> dataList = [];
        for (var i = 0; i < 10; i++)
        {
            Data data = new($"Name {i}");

            for (var j = 0; j < 200; j++)
                data.Value.Add($"Child {i}-{j}", new($"Name {i}"));

            dataList.Add(data);
        }
        
        var store = Gio.ListStore.New(ConfiguratorItemRow.GetGType());

        foreach (Data data in dataList)
        {
            store.Append(new ConfiguratorItemRow
            {
                Name = data.Name,
                Obj = data
            });
        }

        var list = TreeListModel.New(store, false, false, CreateFunc);
        var model = SingleSelection.New(list);
        var columnView = ColumnView.New(model);

        var factory = SignalListItemFactory.New();
        factory.OnSetup += (_, args) =>
        {
            var cell = Label.New(null);

            var expander = TreeExpander.New();
            expander.SetChild(cell);

             var listItem = (ListItem)args.Object;
            listItem.SetChild(expander);
        };

        factory.OnBind += (_, args) =>
        {
            if (args.Object is not ListItem listItem) return;
            if (listItem.GetItem() is not TreeListRow row) return;
            if (listItem.GetChild() is not TreeExpander expander) return;
            if (expander.GetChild() is not Label cell) return;
            if (row.GetItem() is not ConfiguratorItemRow itemRow) return;

            expander.SetListRow(row);
            cell.SetText(itemRow.Name);
        };
        var column = ColumnViewColumn.New("Documents", factory);
        column.Resizable = true;
        columnView.AppendColumn(column);

        ScrolledWindow scroll = new();
        scroll.Vexpand = scroll.Hexpand = true;
        scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        scroll.Child = columnView;

        return scroll;
    }

    private static Gio.ListModel CreateFunc(GObject.Object item)
    {
        var itemRow = (ConfiguratorItemRow)item;

        var data = itemRow.Obj as Data;

        var store = Gio.ListStore.New(ConfiguratorItemRow.GetGType());

        foreach (var field in data.Value)
        {
            store.Append(new ConfiguratorItemRow
            {
                Name = field.Key,
                Obj = field.Value,
            });
        }

        return store;
    }
}