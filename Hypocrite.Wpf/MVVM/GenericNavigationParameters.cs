﻿using Prism.Regions;

namespace Hypocrite.Wpf.Mvvm
{
    public class GenericNavigationParameters<T> : NavigationParameters
    {
        public GenericNavigationParameters(T parameters)
        {
            Parameters = parameters;

            foreach (var property in typeof(T).GetProperties())
            {
                var name = property.Name;
                var value = property.GetValue(parameters);
                Add(name, value);
            }
        }

        public T Parameters { get; set; }
    }
}
