﻿using System;
using System.Collections.Generic;
using System.Text;
using Estately.Services;
using Estately.Models;
using System.Collections.ObjectModel;
using MvvmHelpers;
using Xamarin.Forms;

namespace Estately.ViewModels
{
    public class MarketplaceViewModel : BaseViewModel
    {
        public new string Title { get; set; }
        public string Description { get; set; }

        private ObservableCollection<Listing> _listing = new ObservableCollection<Listing>();
        public ObservableCollection<Listing> Listings
        {
            get { return _listing; }
            set
            {
                _listing = value;
                OnPropertyChanged();
            }
        }
    }
}
