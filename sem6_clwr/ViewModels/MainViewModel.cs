using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using Microsoft.Win32;
using sem6_clwr.Models;

namespace sem6_clwr.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private int _blockSize = 5;
        private CryptoModel model;
        
        public int Key { get; set; } = 1234567890;

        public String Message { get; set; }
        public int BlockSize
        {
            get => _blockSize;
            set
            {
                _blockSize = value;
                if (_blockSize < 18)
                {
                    IsProcessing = true;
                    Message = "Creating rules for block replacement...\n" +
                              "Please, wait...";
                    Task.Factory.StartNew(() =>
                    {
                        model.CreateBlockReplacementRules(BlockSize, Key);
                        Application.Current.Dispatcher.Invoke(() => IsProcessing = false);
                        Application.Current.Dispatcher.Invoke(() => Message = "");
                    });
                }
                else
                {
                    Message = "The block length is too large :(";
                }
            }
        }

        public bool IsProcessing { get; set; }

        public bool IsNotProcessing => !IsProcessing;

        public MainViewModel()
        {
            model=new CryptoModel();
            model.CreateBlockReplacementRules(BlockSize,Key);
        }

        public ICommand EncryptCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    StartCrypto((arg1, arg2) => model.Encrypt(arg1, arg2));
                });
            }
        }

        public ICommand DecryptCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    StartCrypto((arg1,arg2)=>model.Decrypt(arg1,arg2));
                });
            }
        }

        private void StartCrypto(Action<String,int> crypto)
        {
            Message = "Processing...";
            IsProcessing = true;
            Task.Factory.StartNew(() =>
            {
                var path = GetFileName();
                if (path.Length != 0)
                {
                    crypto.Invoke(path,BlockSize);
                }
                Application.Current.Dispatcher.Invoke(() => Message = "Done!");
                Application.Current.Dispatcher.Invoke(() => IsProcessing = false);
            });
        }

        private static String GetFileName()
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (theDialog.ShowDialog() == true)
            {
                return theDialog.FileName;
            }
                return "";
        }
    }
}
