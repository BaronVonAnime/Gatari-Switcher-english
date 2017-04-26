﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GatariSwitcher
{
    public partial class MainWindow : Window
    {
        bool certStatus = false;
        bool servStatus = false;

        public MainWindow()
        {
            InitializeComponent();
            CheckStatus();
        }

        private async void CheckStatus()
        {
            string servAddr = await GeneralHelper.GetActualGatariAddress();
            var manager = new CertificateManager();
            certStatus = await manager.GetStatus();
            var switcher = new ServerSwitcher(servAddr);
            servStatus = switcher.GetCurrentServer();

            statusLabel.Content = servStatus ? "Вы играете на гатарях с кентами!" : "Вы играете на офе с чертями!";
            switchButton.Content = servStatus ? "Перейти на официальный сервер" : "Перейти на гатари";
            certButton.Content = certStatus ? "Удалить сертификат" : "Установить сертификат";
        }

        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void switchButton_Click(object sender, RoutedEventArgs e)
        {
            switchButton.IsEnabled = false;
            string servAddr = await GeneralHelper.GetActualGatariAddress();
            var switcher = new ServerSwitcher(servAddr);
            try
            {
                if (servStatus)
                {
                    switcher.SwitchToOfficial();
                }
                else
                {
                    switcher.SwitchToGatari();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CheckStatus();
            switchButton.IsEnabled = true;
        }

        private void sertButton_Click(object sender, RoutedEventArgs e)
        {
            var manager = new CertificateManager();
            try
            {
                if (certStatus)
                {
                    manager.Uninstall();
                }
                else
                {
                    manager.Install();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CheckStatus();
        }

        private void websiteText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://osu.gatari.pw");
        }
    }
}