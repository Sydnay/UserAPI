using UserAPI.Dtos;
using Users;

namespace UserAPI
{
    static public class ConvertToDto
    {
        static public UserDto ToDto(this User user)
        {
            return new UserDto() { 
                Guid = user.Guid,
                Name = user.Name,
                Login = user.Login,
                Password = user.Password,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Admin = user.Admin,
                CreatedOn = user.CreatedOn,
                CreatedBy = user.CreatedBy,
                ModifiedOn = user.ModifiedOn,
                ModifiedBy = user.ModifiedBy,
                RevokedBy = user.RevokedBy,
                RevokedOn = user.RevokedOn
            };
        }
        static public UserInfoDto ToInfoDto(this User user)
        {
            return new UserInfoDto()
            {
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Active = user.RevokedBy is null ? true : false
            };
        }
    }
}
