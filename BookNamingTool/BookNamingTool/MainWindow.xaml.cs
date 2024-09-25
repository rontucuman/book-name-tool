using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace BookNamingTool
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private string _author;
    private string _title;
    private string _year;
    private string _otherTitle;
    private string _extension;
    private string _generatedFileName;

    public MainWindow()
    {
      InitializeComponent();
      ResetFields();
    }

    private void ResetFields()
    {
      _author = string.Empty;
      _title = string.Empty;
      _otherTitle = string.Empty;
      _year = string.Empty;
      _extension = string.Empty;
      _generatedFileName = string.Empty;
    }

    private void SearchFileBtn_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog = new();

      if (openFileDialog.ShowDialog() == true)
      {
        FilePathTextBox.Text = openFileDialog.FileName;
        _extension = System.IO.Path.GetExtension(openFileDialog.FileName);
      }
    }

    private void SearchFolderBtn_Click(object sender, RoutedEventArgs e)
    {
      var dialog = new OpenFolderDialog();

      if (dialog.ShowDialog() == true)
      {
        DestinationFolderLbl.Content = dialog.FolderName;
      }
    }

    private void AuthorTxt_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
      if (sender is TextBox authorTxt)
      {
        _author = authorTxt.Text.Trim();
      }

      UpdateGeneratedFileName();
    }

    private void UpdateGeneratedFileName()
    {
      StringBuilder sb = new();

      if (!string.IsNullOrWhiteSpace(_title))
      {
        sb.Append(_title);
      }

      if (!string.IsNullOrWhiteSpace(_otherTitle))
      {
        if (!string.IsNullOrWhiteSpace(_title))
        {
          sb.Append(" _ ");
        }

        sb.Append(_otherTitle);
      }

      if (!(string.IsNullOrWhiteSpace(_author) && string.IsNullOrWhiteSpace(_year)))
      {
        sb.Append(" - ");
      }

      if (!string.IsNullOrWhiteSpace(_author))
      {
        sb.Append('[');
        sb.Append(_author);
        sb.Append(']');
      }

      if (!string.IsNullOrWhiteSpace(_year))
      {
        sb.Append('[');
        sb.Append(_year);
        sb.Append(']');
      }

      sb.Append(_extension);

      _generatedFileName = sb.ToString();

      GeneratedFileNameLbl.Content = _generatedFileName;
    }

    private void TitleTxt_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (sender is TextBox titleTxt)
      {
        _title = titleTxt.Text.Trim();
      }

      UpdateGeneratedFileName();
    }

    private void OtherTitleTxt_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (sender is TextBox otherTitle)
      {
        _otherTitle = otherTitle.Text.Trim();
      }

      UpdateGeneratedFileName();
    }

    private void YearTxt_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (sender is TextBox year)
      {
        _year = year.Text.Trim();
      }

      UpdateGeneratedFileName();
    }

    private void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
      string sourceFilePath = FilePathTextBox.Text;
      string? destinationFolder = DestinationFolderLbl.Content?.ToString();

      if (destinationFolder == null)
      {
        // Handle the case where destinationFolder is null
        System.Windows.MessageBox.Show("Please select a destination folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      // Continue with the save operation
      string destinationFilePath = System.IO.Path.Combine(destinationFolder, _generatedFileName);

      try
      {
        System.IO.File.Copy(sourceFilePath, destinationFilePath, true);
        System.Windows.MessageBox.Show("File saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
      }
      catch (Exception ex)
      {
        System.Windows.MessageBox.Show($"Error copying file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void CleanBtn_Click(object sender, RoutedEventArgs e)
    {
      ResetFields();
      ClearUI();
    }

    private void ClearUI()
    {
      FilePathTextBox.Text = string.Empty;
      TitleTxt.Text = string.Empty;
      OtherTitleTxt.Text = string.Empty;
      AuthorTxt.Text = string.Empty;
      YearTxt.Text = string.Empty;
      GeneratedFileNameLbl.Content = string.Empty;
    }

    private void CleanAllBtn_Click(object sender, RoutedEventArgs e)
    {
      ResetFields();
      ClearUI();
      DestinationFolderLbl.Content = string.Empty;
    }
  }
}