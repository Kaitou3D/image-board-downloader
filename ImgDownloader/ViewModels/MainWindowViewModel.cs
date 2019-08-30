using Prism.Mvvm;
using PropertyChanged;

namespace ImgDownloader.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel 
    {
        private string _title = "Prism Application";
        public string Title
        {
            get; set;
        } = "Downloader";

        public MainWindowViewModel()
        {
            
        }
    }
}
