
using Gtk;

class Program
{
    public static readonly Application BasicApp = Application.New("ua.org.accounting.test", Gio.ApplicationFlags.FlagsNone);

    static void Main()
    {
        BasicApp.OnActivate += (app, args) => new FormWindow(BasicApp).Show();
        BasicApp.OnShutdown += (app, args) => { };

        BasicApp.RunWithSynchronizationContext(null);
    }
}
