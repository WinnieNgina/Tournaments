﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class TournamentOrganizerModel : UserModel
    {
        /// <summary>
        /// Gets or sets the organization name or office of the tournament organizer. 
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// Gets or sets the collection of tournaments organized by this organizer.
        /// </summary>
        public virtual ICollection<TournamentModel> Tournaments { get; set; }

    }
}