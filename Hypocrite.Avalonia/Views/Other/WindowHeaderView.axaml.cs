using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;
using System.Windows.Input;

namespace Hypocrite.Avalonia.Views.Other
{
    public partial class WindowHeaderView : UserControl
    {
        public Bitmap LogoImage
        {
            get { return (Bitmap)GetValue(LogoImageProperty); }
            set { SetValue(LogoImageProperty, value); }
        }

        public static readonly StyledProperty<Bitmap> LogoImageProperty =
            AvaloniaProperty.Register<WindowHeaderView, Bitmap>("LogoImage");

        public string ProductName
        {
            get { return (string)GetValue(ProductNameProperty); }
            set { SetValue(ProductNameProperty, value); }
        }

        public static readonly StyledProperty<string> ProductNameProperty =
            AvaloniaProperty.Register<WindowHeaderView, string>("ProductName");

        public Window WindowParameter
        {
            get { return (Window)GetValue(WindowParameterProperty); }
            set { SetValue(WindowParameterProperty, value); }
        }

        public static readonly StyledProperty<Window> WindowParameterProperty =
            AvaloniaProperty.Register<WindowHeaderView, Window>("WindowParameter");

        public ICommand MinimizeWindowCommand
        {
            get { return (ICommand)GetValue(MinimizeWindowCommandProperty); }
            set { SetValue(MinimizeWindowCommandProperty, value); }
        }

        public static readonly StyledProperty<ICommand> MinimizeWindowCommandProperty =
            AvaloniaProperty.Register<WindowHeaderView, ICommand>("MinimizeWindowCommand");

        public ICommand MaximizeWindowCommand
        {
            get { return (ICommand)GetValue(MaximizeWindowCommandProperty); }
            set { SetValue(MaximizeWindowCommandProperty, value); }
        }

        public static readonly StyledProperty<ICommand> MaximizeWindowCommandProperty =
            AvaloniaProperty.Register<WindowHeaderView, ICommand>("MaximizeWindowCommand");

        public ICommand RestoreWindowCommand
        {
            get { return (ICommand)GetValue(RestoreWindowCommandProperty); }
            set { SetValue(RestoreWindowCommandProperty, value); }
        }

        public static readonly StyledProperty<ICommand> RestoreWindowCommandProperty =
            AvaloniaProperty.Register<WindowHeaderView, ICommand>("RestoreWindowCommand");

        public ICommand CloseWindowCommand
        {
            get { return (ICommand)GetValue(CloseWindowCommandProperty); }
            set { SetValue(CloseWindowCommandProperty, value); }
        }

        public static readonly StyledProperty<ICommand> CloseWindowCommandProperty =
            AvaloniaProperty.Register<WindowHeaderView, ICommand>("CloseWindowCommand");

        public bool MinimizeButtonVisibility
        {
            get { return (bool)GetValue(MinimizeButtonVisibilityProperty); }
            set { SetValue(MinimizeButtonVisibilityProperty, value); }
        }

        public static readonly StyledProperty<bool> MinimizeButtonVisibilityProperty =
            AvaloniaProperty.Register<WindowHeaderView, bool>("MinimizeButtonVisibility");

        public bool MaximizeButtonVisibility
        {
            get { return (bool)GetValue(MaximizeButtonVisibilityProperty); }
            set { SetValue(MaximizeButtonVisibilityProperty, value); }
        }

        public static readonly StyledProperty<bool> MaximizeButtonVisibilityProperty =
            AvaloniaProperty.Register<WindowHeaderView, bool>("MaximizeButtonVisibility");

        public bool RestoreButtonVisibility
        {
            get { return (bool)GetValue(RestoreButtonVisibilityProperty); }
            set { SetValue(RestoreButtonVisibilityProperty, value); }
        }

        public static readonly StyledProperty<bool> RestoreButtonVisibilityProperty =
            AvaloniaProperty.Register<WindowHeaderView, bool>("RestoreButtonVisibility");

        public bool ProgressBarVisibility
        {
            get { return (bool)GetValue(ProgressBarVisibilityProperty); }
            set { SetValue(ProgressBarVisibilityProperty, value); }
        }

        public static readonly StyledProperty<bool> ProgressBarVisibilityProperty =
            AvaloniaProperty.Register<WindowHeaderView, bool>("ProgressBarVisibility");

        public bool CheckAllDoneVisibility
        {
            get { return (bool)GetValue(CheckAllDoneVisibilityProperty); }
            set { SetValue(CheckAllDoneVisibilityProperty, value); }
        }

        public static readonly StyledProperty<bool> CheckAllDoneVisibilityProperty =
            AvaloniaProperty.Register<WindowHeaderView, bool>("CheckAllDoneVisibility");

        public IRegionManager RegionManager
        {
            get { return (IRegionManager)GetValue(RegionManagerProperty); }
            set { SetValue(RegionManagerProperty, value); }
        }

        public static readonly StyledProperty<IRegionManager> RegionManagerProperty =
            AvaloniaProperty.Register<WindowHeaderView, IRegionManager>("RegionManager");

        public WindowHeaderView()
        {
            InitializeComponent();
        }
    }
}
