﻿using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Storage;
using Xamarin.Forms;
using Estately.Models;
using Estately.Services;
using MvvmHelpers;

namespace Estately.ViewModels
{
    public class NewListingViewModel : BaseViewModel
    {
        readonly FirebaseDB services;

        public new string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Featured { get; set; }
        public string Feature1 { get; set; }
        public string Feature2 { get; set; }
        public string Feature3 { get; set; }
        public string Feature4 { get; set; }
        public double Price;

        private string _image;
        public string Image {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        public Command AddListingCommand { get; set; }
        public Command UploadCommand { get; set; }
        public NewListingViewModel()
        {
            services = new FirebaseDB();
            AddListingCommand = new Command(() => AddListing());
            UploadCommand = new Command(() => UploadPhotoClicked());
        }

        public async void UploadPhotoClicked()
        {
            var photo = await Xamarin.Essentials.MediaPicker.PickPhotoAsync();

            if (photo == null)
            {
                return;
            }

            var task = new FirebaseStorage("estately-9a428.appspot.com",
                new FirebaseStorageOptions
                {
                    ThrowOnCancel = true
                }).Child($"{Title} Images")
                .Child(photo.FileName)
                .PutAsync(await photo.OpenReadAsync());

            Image = await task;
        }

        public async void AddListing()
        {
            if (string.IsNullOrEmpty(Title))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Title is Empty!", "Ok");
            }
            else if (string.IsNullOrEmpty(Description))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Description is Empty!", "Ok");
            }
            else if (string.IsNullOrEmpty(Type))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Type is Empty!", "Ok");
            }
            else if (string.IsNullOrEmpty(Location))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Location is Empty!", "Ok");
            }
            else if (string.IsNullOrEmpty(Feature1))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Please enter at least 1 feature", "Ok");
            }
            else if (string.IsNullOrEmpty(Image))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Please Upload an Image!", "Ok");
            }
            else
            {
                try
                {
                    var listing = new Listing { Title = Title, Description = Description, Price = Price, Type = Type, Location = Location, Featured = "No", Image = Image };
                    await services.AddListing(listing);
                    await App.Current.MainPage.DisplayAlert("Success", Title + " Added", "Ok");
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", $"Incorrect or empty inputs, {ex.Message}", "Ok");
                }
            }

        }
    }
}
