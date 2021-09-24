namespace GVUZ.Helper.MVC
{
    public class BaseEditViewModel
    {
        public bool IsEdit;

        public BaseEditViewModel()
        {
        }

        public BaseEditViewModel(bool isEdit)
        {
            IsEdit = isEdit;
        }
    }
}