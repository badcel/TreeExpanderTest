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

        var button = Button.NewWithLabel("Button");
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
        var store = Gio.ListStore.New(ConfiguratorItemRow.GetGType());

        for (var i = 0; i < 10; i++)
        {
            var item = new ConfiguratorItemRow { Name = $"Name {i}" };
            
            for (var j = 0; j < 200; j++)
                item.Sub.Add(new ConfiguratorItemRow { Name = $"Child {i}-{j}" }); 
            
            store.Append(item);
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

        var scroll = new ScrolledWindow();
        scroll.Vexpand = scroll.Hexpand = true;
        scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        scroll.Child = columnView;

        return scroll;
    }

    private static Gio.ListModel CreateFunc(GObject.Object item)
    {
        var itemRow = (ConfiguratorItemRow)item;

        var store = Gio.ListStore.New(ConfiguratorItemRow.GetGType());

        foreach (var subElement in itemRow.Sub)
            store.Append(subElement);

        return store;
    }
}