using ImgDownloader.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace ImgDownloader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<QueryTab>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
