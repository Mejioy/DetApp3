using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DetApp3.Areas.Identity.Data;

// Add profile data for application users by adding properties to the DetApp3User class
public class DetApp3User : IdentityUser
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronym { get; set; }
}

