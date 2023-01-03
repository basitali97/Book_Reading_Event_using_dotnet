﻿using System;
using System.Collections.Generic;
using System.Text;

namespace common.Models
{
    public class EventDTO
    {
       
        public int EventId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public string Type { get; set; }

        public string Duration { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        public string Invite { get; set; }
    }
}