using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OnlineRestaurant.UI.Converters
{
    public class RelativeToAbsolutePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return null;

            try
            {
                string relativePath = value.ToString();
                System.Diagnostics.Debug.WriteLine($"Original path: {relativePath}");

                // Get application's base directory
                string basePath = AppDomain.CurrentDomain.BaseDirectory;

                // For paths starting with ../ or ..\\
                if (relativePath.StartsWith("../") || relativePath.StartsWith("..\\"))
                {
                    // Get just the file name (ignore the ../Images/ part)
                    string fileName = Path.GetFileName(relativePath);

                    // Try multiple possible locations

                    // 1. Check if image exists in an "Images" folder at the application root
                    string path1 = Path.Combine(basePath, "Images", fileName);
                    if (File.Exists(path1))
                    {
                        System.Diagnostics.Debug.WriteLine($"Found at path1: {path1}");
                        return path1;
                    }

                    // 2. Check one level up from app directory
                    string path2 = Path.Combine(Directory.GetParent(basePath).FullName, "Images", fileName);
                    if (File.Exists(path2))
                    {
                        System.Diagnostics.Debug.WriteLine($"Found at path2: {path2}");
                        return path2;
                    }

                    // 3. Check two levels up from app directory
                    string path3 = Path.Combine(Directory.GetParent(Directory.GetParent(basePath).FullName).FullName, "Images", fileName);
                    if (File.Exists(path3))
                    {
                        System.Diagnostics.Debug.WriteLine($"Found at path3: {path3}");
                        return path3;
                    }

                    // 4. Check for the complete path directly
                    string path4 = Path.GetFullPath(Path.Combine(basePath, relativePath));
                    if (File.Exists(path4))
                    {
                        System.Diagnostics.Debug.WriteLine($"Found at path4: {path4}");
                        return path4;
                    }

                    // If none of the above work, try one more approach
                    string projectDir = Directory.GetParent(Directory.GetParent(Directory.GetParent(basePath).FullName).FullName).FullName;
                    string path5 = Path.Combine(projectDir, "Images", fileName);
                    if (File.Exists(path5))
                    {
                        System.Diagnostics.Debug.WriteLine($"Found at path5: {path5}");
                        return path5;
                    }

                    System.Diagnostics.Debug.WriteLine($"Could not find image at any location: {fileName}");
                    return null;
                }
                else
                {
                    string fullPath = Path.GetFullPath(Path.Combine(basePath, relativePath));
                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error converting path: {ex.Message}");
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
