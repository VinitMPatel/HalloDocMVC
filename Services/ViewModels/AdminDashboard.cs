﻿using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class AdminDashboard
    {

        public List<Requestclient> requestclients { get; set; }

        public int totalPages { get; set; }

        public int currentpage { get; set; }
    }


}
