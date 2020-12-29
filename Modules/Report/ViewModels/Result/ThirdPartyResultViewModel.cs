namespace Fanex.Razorbill.Module.Report.ViewModels.Result
{
    public class ThirdPartyResultViewModel
    {
        public ThirdPartyResultViewModel(string productName, string result)
        {
            Title = productName;
            Result = result;
        }

        public string Title { get; }

        public string Result { get; }
    }
}
