using Abdrakov.Engine.MVVM.Events;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace Abdrakov.CommonWPF.MVVM
{
    public class DialogViewModelBase : ViewModelBase, IDialogAware
    {
        protected DialogViewModelBase()
        {
            KeyDownCommand = new DelegateCommand(OnKeyDown);
        }

        protected void CloseDialog(ButtonResult result)
        {
            RaiseRequestClose(new DialogResult(result));
        }

        protected void CloseDialog<T>(ButtonResult result, T value)
        {
            RaiseRequestClose(new CustomDialogResult<T>(result, value));
        }

        protected void CloseDialogWithDefaultResult<T>(ButtonResult result)
        {
            CloseDialog<T>(result, default);
        }

        protected void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            EventAggregator.GetEvent<DialogClosedEvent>().Publish();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            EventAggregator.GetEvent<DialogOpenedEvent>().Publish();
            if (parameters == null)
            {
                return;
            }

            var viewmodelType = GetType();
            foreach (var key in parameters.Keys)
            {
                var viewmodelProperty = viewmodelType.GetProperty(key);
                viewmodelProperty.SetValue(this, parameters.GetValue<object>(key));
            }

            OnReady();
        }

        protected virtual void OnReady()
        {
        }

        protected virtual void OnKeyDown()
        {
        }

        string IDialogAware.Title => "";

        private ICommand keyDownCommand;
        public ICommand KeyDownCommand 
        { 
            get { return keyDownCommand; }
            set { SetProperty(ref keyDownCommand, value); } 
        }

        public event Action<IDialogResult> RequestClose;
    }

    public class CustomDialogResult<T> : IDialogResult
    {
        public CustomDialogResult(ButtonResult result, T value)
        {
            Result = result;
            Value = value;
        }

        public IDialogParameters Parameters { get; }
        public ButtonResult Result { get; }
        public T Value { get; }
    }

    public class CustomDialogParameters : DialogParameters
    {
        public CustomDialogParameters(object obj)
        {
            Parameters = obj;
            var parametersType = obj.GetType();
            foreach (var parameterProperty in parametersType.GetProperties())
            {
                Add(parameterProperty.Name, parameterProperty.GetValue(obj));
            }
        }

        public object Parameters { get; }
    }
}
