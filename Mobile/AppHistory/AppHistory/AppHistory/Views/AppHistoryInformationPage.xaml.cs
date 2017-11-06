/*
* Copyright (c) 2017 Samsung Electronics Co., Ltd All Rights Reserved
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppHistory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppHistoryInformationPage : ContentPage
    {
        /// <summary>
        /// Interface for using AppHistory APIs
        /// </summary>
        private static IAppHistoryAPIs appHistoryAPIs;
        
        /// <summary>
        /// Queried result of application history
        /// </summary>
        private List<StatsInfoItem> statsInfo;

        /// <summary>
        /// Enumeration for application history query type
        /// </summary>
        enum QueryType
        {
            RecentlyUsedApplications,
            FrequentlyUsedApplications,
            BatteryConsumingApplications,
        }

        /// <summary>
        /// Constructor of AppHistoryInformationPage
        /// </summary>
        /// <param name="title">The title of the content page to present</param>
        public AppHistoryInformationPage(string title)
        {
            try
            {
                InitializeComponent();

                // Set the title of this content page
                this.Title = title;

                // Initialize StatisticsInfo list
                statsInfo = new List<StatsInfoItem>();

                // Query application history 
                appHistoryAPIs = DependencyService.Get<IAppHistoryAPIs>();

                if (title.Equals("Top 5 recently used applications"))
                {
                    Query(QueryType.RecentlyUsedApplications);
                }
                else if (title.Equals("Top 10 frequently used applications"))
                {
                    Query(QueryType.FrequentlyUsedApplications);
                }
                else if (title.Equals("Top 10 battery consuming applications"))
                {
                    Query(QueryType.BatteryConsumingApplications);
                }

                


                // Set the item source of list view
                appHistoryInformationList.ItemsSource = statsInfo;
            }
            catch (Exception e)
            {
                DependencyService.Get<ILog>().Log(e.Message.ToString());
            }
        }

        /// <summary>
        /// Query application history according to its type
        /// </summary>
        /// <param name="queryType">Application history type to query</param>
        private void Query(QueryType queryType)
        {
            try
            {
                switch (queryType)
                {
                    case QueryType.RecentlyUsedApplications:
                        statsInfo = appHistoryAPIs.QueryRecentlyUsedApplications();
                        break;
                    case QueryType.FrequentlyUsedApplications:
                        statsInfo = appHistoryAPIs.QueryFrequentlyUsedApplications();
                        break;
                    case QueryType.BatteryConsumingApplications:
                        statsInfo = appHistoryAPIs.QueryBatteryConsumingApplications();
                        break;
                    default:
                        statsInfo = null;
                        break;
                }
            }
            catch (Exception e)
            {
                DependencyService.Get<ILog>().Log(e.Message.ToString());
            }
        }

        /// <summary>
        /// This method will be called when a list item is selected.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
