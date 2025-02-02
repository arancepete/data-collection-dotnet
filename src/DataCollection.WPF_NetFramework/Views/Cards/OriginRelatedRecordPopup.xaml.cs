﻿/*******************************************************************************
  * Copyright 2019 Esri
  *
  *  Licensed under the Apache License, Version 2.0 (the "License");
  *  you may not use this file except in compliance with the License.
  *  You may obtain a copy of the License at
  *
  *  https://www.apache.org/licenses/LICENSE-2.0
  *
  *   Unless required by applicable law or agreed to in writing, software
  *   distributed under the License is distributed on an "AS IS" BASIS,
  *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  *   See the License for the specific language governing permissions and
  *   limitations under the License.
******************************************************************************/

using System.Windows;
using Esri.ArcGISRuntime.OpenSourceApps.DataCollection.WPF.Helpers;

namespace Esri.ArcGISRuntime.OpenSourceApps.DataCollection.WPF.Views.Cards
{
    /// <summary>
    /// Interaction logic for RelatedRecordPopup.xaml
    /// </summary>
    public partial class OriginRelatedRecordPopup
    {
        public OriginRelatedRecordPopup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for user clicking the Add Attachment button
        /// </summary>
        private void AddAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            AttachmentPathTextBlock.Text = BrowseHelper.GetFileFromUser();
        }
    }
}
