using Microsoft.EntityFrameworkCore;

namespace SignatureWeb.Models
{
    [Keyless]
    public class UserMain
    {
        public string Name { get; set; }
        public virtual UserUnitPosition UserUnitPosition { get; set; }
    }
}
