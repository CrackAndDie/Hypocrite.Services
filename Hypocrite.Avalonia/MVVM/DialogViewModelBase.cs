﻿using Avalonia.Input;
using Hypocrite.Core.Mvvm.Events;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Hypocrite.Mvvm
{
    public class DialogViewModelBase : ViewModelBase, IDialogAware
    {
        protected DialogViewModelBase()
        {
        }

        protected void ForceCloseDialog()
        {
            CloseDialog(ButtonResult.Abort);
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

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
            EventAggregator.GetEvent<DialogClosedEvent>().Publish();
            EventAggregator.GetEvent<CloseOpenedDialogsEvent>().Unsubscribe(ForceCloseDialog);
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            EventAggregator.GetEvent<DialogOpenedEvent>().Publish();
            EventAggregator.GetEvent<CloseOpenedDialogsEvent>().Subscribe(ForceCloseDialog);
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

        protected virtual void OnKeyDown(KeyEventArgs keyArgs)
        {
        }

        string IDialogAware.Title => "";

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

    public enum DialogButtons
    {
        YesNo,
        OKCancel,
        OK
    }
}
