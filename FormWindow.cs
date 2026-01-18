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
        for (int i = 0; i < 10; i++)
        {
            Data data = new($"Name {i}");

            for (int j = 0; j < 200; j++)
                data.Value.Add($"Child {i}-{j}", new($"Name {i}"));

            List.Add(data);
        }

        vBox.Append(Fill(List));

        notebook.AppendPage(vBox, Label.New("Page"));
    }

    private Box Fill(List<Data> dataList)
    {
        var store = Gio.ListStore.New(ConfiguratorItemRow.GetGType());
        Box hBox = Box.New(Orientation.Horizontal, 0);

        //Заповнення сховища початковими даними
        foreach (Data data in dataList)
            store.Append(new ConfiguratorItemRow()
            {
                Group = "Documents",
                Name = data.Name,
                Obj = data
            });

        TreeListModel list = TreeListModel.New(store, false, false, CreateFunc);

        SingleSelection model = SingleSelection.New(list);
        ColumnView columnView = ColumnView.New(model);

        //Tree
        {
            SignalListItemFactory factory = SignalListItemFactory.New();
            factory.OnSetup += (_, args) =>
            {
                ListItem listItem = (ListItem)args.Object;
                var cell = Label.New(null);

                TreeExpander expander = TreeExpander.New();
                expander.SetChild(cell);

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
        }

        ScrolledWindow scroll = new();
        scroll.Vexpand = scroll.Hexpand = true;
        scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        scroll.Child = columnView;

        hBox.Append(scroll);

        return hBox;
    }

    Gio.ListModel? CreateFunc(GObject.Object item)
    {
        ConfiguratorItemRow itemRow = (ConfiguratorItemRow)item;

        var data = itemRow.Obj as Data;

        Gio.ListStore Store = Gio.ListStore.New(ConfiguratorItemRow.GetGType());

        foreach (KeyValuePair<string, Data> field in data.Value)
        {
            Store.Append(new ConfiguratorItemRow()
            {
                Group = "Field",
                Name = field.Key,
                Obj = field.Value,
                Type = "Type",
                Desc = "Pointer"
            });
        }

        return Store;
    }
}