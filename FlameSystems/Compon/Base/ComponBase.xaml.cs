﻿namespace FlameSystems.Compon.Base
{
    /// <summary>
    ///     Interaction logic for ComponBase.xaml
    /// </summary>
    public partial class ComponBaseModel : IComponentView
    {
        public ComponBaseModel()
        {
            InitializeComponent();
        }

        public IComponentViewModel ViewModel
        {
            get => (IComponentViewModel) DataContext;
            set => DataContext = value;
        }
    }
}