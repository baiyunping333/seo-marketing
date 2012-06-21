using Microsoft.Practices.Prism.ViewModel;

namespace KeywordExtractor
{
    public abstract class ViewModelBase<TModel> : NotificationObject
    {
        public TModel Model { get; set; }
    }
}
