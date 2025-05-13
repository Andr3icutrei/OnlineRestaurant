using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace OnlineRestaurant.UI.Models
{
    // Create this model class in your project
    public class ImageItem : INotifyPropertyChanged
    {
        private BitmapImage _imageSource;
        private bool _isSelected;
        private string _name;

        // Store the original entity for reference
        public ItemPicture SourceEntity { get; private set; }

        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int Id => SourceEntity.Id;
        public string PicturePath => SourceEntity.PicturePath;
        public int? ItemId => SourceEntity.ItemId;

        // Factory method to create from entity
        public static ImageItem FromEntity(ItemPicture entity)
        {
            var item = new ImageItem
            {
                SourceEntity = entity,
                Name = Path.GetFileName(entity.PicturePath)
            };

            // Load the image
            item.LoadImage();

            return item;
        }

        // Method to load the image from the path
        public void LoadImage()
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();

                // Check if the path is relative or absolute
                if (Path.IsPathRooted(PicturePath))
                {
                    bitmap.UriSource = new Uri(PicturePath, UriKind.Absolute);
                }
                else
                {
                    // Handle relative paths - assuming they are relative to application folder
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    string fullPath = Path.Combine(baseDir, PicturePath);
                    bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                }

                bitmap.CacheOption = BitmapCacheOption.OnLoad; // Load in memory to avoid file locks
                bitmap.EndInit();

                ImageSource = bitmap;
            }
            catch (Exception ex)
            {
                // Handle image loading error - maybe load a placeholder
                Debug.WriteLine($"Error loading image from {PicturePath}: {ex.Message}");

                // Load a placeholder image
                var placeholder = new BitmapImage();
                placeholder.BeginInit();
                placeholder.UriSource = new Uri("pack://application:,,,/YourAppNamespace;component/Resources/placeholder.png", UriKind.Absolute);
                placeholder.EndInit();

                ImageSource = placeholder;
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
