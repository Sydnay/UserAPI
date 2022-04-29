
public class MemoryRepo
{
    private readonly List<User> list = new()
    {
        new User("aboba", "Abobich", "NoBitches", 1, new DateTime(1990, 12, 30), false)
        {
            Guid = Guid.NewGuid(),
            CreatedBy = "Putin",
            CreatedOn = DateTime.Now,
            ModifiedOn = DateTime.Now,
            ModifiedBy = "Putin"
        },
        new User("aboba", "Abobich", "NoBitches", 0, new DateTime(1988,12,3), false)
        {
            Guid = Guid.NewGuid(),
            CreatedBy = "Zelenski",
            CreatedOn = DateTime.Now,
            ModifiedOn = DateTime.Now,
            ModifiedBy = "Zelenski"
        },
        new User("aboba", "Abobich", "NoBitches", 1, new DateTime(1991, 2, 30), false)
        {
            Guid = Guid.NewGuid(),
            CreatedBy = "Bidon",
            CreatedOn = DateTime.Now,
            ModifiedOn = DateTime.Now,
            ModifiedBy = "Bidon"
        }
    }
    public  GetUsers
    
}
