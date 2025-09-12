namespace QuickStart.Application.Features.Identity;

public class UserProfile
{

    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool Status { get; set; } = true;
    public bool IsLDAP { get; set; }
    public DateTime? LastUpdated { get; set; }
    public DateTime? CreatedOn { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
     

     public string FirstName { get; set; }

     public string LastName { get; set; }

     public string MiddleName { get; set; }


     public string Email { get; set; }

     public string Location { get; set; }

     public string ZipCode { get; set; }

     public string PRNR { get; set; }

  
     public string PrimaryContact { get; set; }


     public string SecondaryContact { get; set; }


     public string Address { get; set; }

     public int? CompanyId { get; set; }

 
     public string City { get; set; }


     public string State { get; set; }

  
     public string Country { get; set; }


   public string ProfileName { get; set; }
   public string ShortName { get; set; }


}
