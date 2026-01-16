

using Gtk;

public class ConfiguratorDocumentsTree(List<Data> list)
{
    List<Data> List = list;

    //Сховище
    readonly Gio.ListStore Store = Gio.ListStore.New(ConfiguratorItemRow.GetGType());

    public Box Fill()
    {
        Box hBox = Box.New(Orientation.Horizontal, 0);

        //Заповнення сховища початковими даними
        foreach (Data data in List)
            Store.Append(new ConfiguratorItemRow()
            {
                Group = "Documents",
                Name = data.Name,
                Obj = data
            });

        TreeListModel list = TreeListModel.New(Store, false, false, CreateFunc);

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
                ListItem listItem = (ListItem)args.Object;
                TreeListRow? row = (TreeListRow?)listItem.GetItem();
                if (row != null)
                {
                    TreeExpander? expander = (TreeExpander?)listItem.GetChild();

                    var cell = (Label?)expander?.GetChild();
                    ConfiguratorItemRow? itemRow = (ConfiguratorItemRow?)row.GetItem();
                    if (expander != null && cell != null && itemRow != null)
                    {
                        expander.SetListRow(row);
                        cell.SetText(itemRow.Name);
                    }
                }
            };
            var column = ColumnViewColumn.New("Documents", factory);
            column.Resizable = true;
            columnView.AppendColumn(column);
        }

        //Data type
        {
            SignalListItemFactory factory = SignalListItemFactory.New();
            factory.OnSetup += (_, args) =>
            {
                var listItem = (ListItem)args.Object;
                var cell = Label.New(null);
                listItem.SetChild(cell);

            };
            factory.OnBind += (_, args) =>
            {
                ListItem listItem = (ListItem)args.Object;
                TreeListRow? row = (TreeListRow?)listItem.GetItem();
                if (row != null)
                {
                    var cell = (Label?)listItem.Child;
                    ConfiguratorItemRow? itemRow = (ConfiguratorItemRow?)row.GetItem();
                    if (cell != null && itemRow != null)
                        cell.SetText(itemRow.Type);
                }
            };
            var column = ColumnViewColumn.New("Data type", factory);
            column.Resizable = true;
            columnView.AppendColumn(column);
        }

        //Details
        {
            SignalListItemFactory factory = SignalListItemFactory.New();
            factory.OnSetup += (_, args) =>
            {
                var listItem = (ListItem)args.Object;
                var cell = Label.New(null);
                listItem.SetChild(cell);

            };
            factory.OnBind += (_, args) =>
            {
                ListItem listItem = (ListItem)args.Object;
                TreeListRow? row = (TreeListRow?)listItem.GetItem();
                if (row != null)
                {
                    var cell = (Label?)listItem.Child;
                    ConfiguratorItemRow? itemRow = (ConfiguratorItemRow?)row.GetItem();
                    if (cell != null && itemRow != null)
                        cell.SetText(itemRow.Desc);
                }
            };
            var column = ColumnViewColumn.New("Details", factory);
            column.Resizable = true;
            columnView.AppendColumn(column);
        }

        //Empty
        {
            ColumnViewColumn column = ColumnViewColumn.New(null, null);
            column.Resizable = true;
            column.Expand = true;
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

        string group = itemRow.Group;
        object? obj = itemRow.Obj;

        Console.WriteLine($"group {group}, obj {obj}");

        Gio.ListStore Store = Gio.ListStore.New(ConfiguratorItemRow.GetGType());

        switch (group)
        {
            case "Documents" when obj is Data data && data.Value.Count > 0:
                {
                    foreach (KeyValuePair<string, Data> field in data.Value)
                        Store.Append(new ConfiguratorItemRow()
                        {
                            Group = "Field",
                            Name = field.Key,
                            Obj = field.Value,
                            Type = "Type",
                            Desc = "Pointer"
                        });

                    Store.Append(new ConfiguratorItemRow()
                    {
                        Group = "TablePartGroup",
                        Name = "[ TableParts ]",
                        Obj = data
                    });

                    return Store;
                }
            case "TablePartGroup" when obj is Data data:
                {
                    foreach (KeyValuePair<string, Data> field in data.Value)
                        Store.Append(new ConfiguratorItemRow()
                        {
                            Group = "TablePart",
                            Name = field.Key,
                            Obj = field.Value,
                            Type = "Type",
                            Desc = "Pointer"
                        });

                    return Store;
                }
            case "TablePart" when obj is Data data:
                {
                    foreach (KeyValuePair<string, Data> field in data.Value)
                        Store.Append(new ConfiguratorItemRow()
                        {
                            Group = "TablePartField",
                            Name = field.Key,
                            Obj = field.Value,
                            Type = "Type",
                            Desc = "Pointer"
                        });

                    return Store;
                }
            case "Field" when obj is Data data:
                {
                    foreach (KeyValuePair<string, Data> field in data.Value)
                        Store.Append(new ConfiguratorItemRow()
                        {
                            Group = "Field2",
                            Name = field.Key,
                            Obj = field.Value,
                            Type = "Type",
                            Desc = "Pointer"
                        });

                    return Store;
                }
            default:
                return null;
        }
    }
}