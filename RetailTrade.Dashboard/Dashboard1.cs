﻿using DevExpress.DashboardCommon;
using DevExpress.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace RetailTrade.Dashboard
{
    public partial class Dashboard1 : DevExpress.DashboardCommon.Dashboard
    {
        public Dashboard1()
        {
            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
            // Instantiate a new DBContext
            RetailTrade.Dashboard.RetailTradeDbEntities1 dbContext = new RetailTrade.Dashboard.RetailTradeDbEntities1();
            // Call the Load method to get the data for the given DbSet from the database.
            dbContext.Receipts.Load();
            // This line of code is generated by Data Source Configuration Wizard
            ReceiptDataSource.DataSource = dbContext.Receipts.Local.ToBindingList();
        }
    }
}
