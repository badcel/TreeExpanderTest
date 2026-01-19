
using Gtk;

class Program
{
    private static readonly Application BasicApp = Application.New("ua.org.accounting.test", Gio.ApplicationFlags.FlagsNone);

    private static void Main()
    {
        BasicApp.OnActivate += (app, args) => new FormWindow(BasicApp).Show();

        BasicApp.RunWithSynchronizationContext(null);
    }
}
