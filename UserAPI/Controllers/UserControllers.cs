using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using UserAPI;
using UserAPI.Dtos;
using UserAPI.Repository;
using Users;

namespace UserControllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository repository;
        
        public UserController(IUserRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 1) Creating a user by login, password, name, gender and date of birth + specifying whether the user will be an admin 
        /// (Available to Admins)
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(string login, string password, CreateUserDto userDto)
        {
            User admin = await repository.GetAdminAsync(login, password);

            if (admin == null)
                return NotFound("Admin is not found");

            if (await repository.GetActiveUserAsync(userDto.Login) != null)
                return BadRequest("User already exist");
            var user = new User()
            {
                Name = userDto.Name,
                Login = userDto.Login,
                Password = userDto.Password,
                Gender = userDto.Gender,
                Birthday = userDto.Birthday,
                Admin = userDto.Admin,
                Guid = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                CreatedBy = admin.Login,
                ModifiedOn = DateTime.Now,
                ModifiedBy = admin.Login
            };

            await repository.CreateUserAsync(user);

            return Ok(user.ToDto());
        }
        /// <summary>
        /// 2) Changing the user's name, gender, or date of birth 
        /// (Can be changed by the Administrator, or by the user personally, if he is active (no RevokedOn))
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut("changeinfo/{login}")]
        public async Task<ActionResult> UpdateUserInfo(string login, string password, UpdateUserInfoDto userDto, string userLogin)
        {
            User user;

            if (userLogin == login)
            {
                user = await repository.GetActiveUserAsync(userLogin);
                if (user != null && user.Password != password)
                    return BadRequest("Wrong password");
            }
            else
            {
                User admin = await repository.GetAdminAsync(login, password);
                if (admin == null)
                    return NotFound("Executor not found");

                user = await repository.GetUserAsync(userLogin);
            }

            if (user == null)
                return NotFound("User not found");

            var updatedUser = user with
            {
                Name = userDto.Name == null ? user.Name : userDto.Name,
                Gender = userDto.Gender,
                Birthday = userDto.Birthday == null ? user.Birthday : userDto.Birthday,
                ModifiedBy = login,
                ModifiedOn = DateTime.Now
            };

            await repository.UpdateUserAsync(updatedUser);

            return Ok(updatedUser.ToDto());
        }
        /// <summary>
        /// 3) Password change 
        /// (The password can be changed either by the Administrator or by the user personally, if it is active (no RevokedOn))
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut("changepassword/{login}")]
        public async Task<ActionResult> UpdateUserPassword(string login, string password, UpdateUserPasswordDto userDto, string userLogin)
        {
            User user;

            if (userLogin == login)
            {
                user = await repository.GetActiveUserAsync(userLogin);
                if (user != null && user.Password != password)
                    return BadRequest("Wrong password");
            }
            else
            {
                User admin = await repository.GetAdminAsync(login, password);
                if (admin == null)
                    return NotFound("Executor not found");

                user = await repository.GetUserAsync(userLogin);
            }

            if (user == null)
                return NotFound("User not found");

            var updatedUser = user with
            {
                Password = userDto.Password,
                ModifiedBy = login,
                ModifiedOn = DateTime.Now
            };

            await repository.UpdateUserAsync(updatedUser);

            return Ok(updatedUser.ToDto());
        }
        /// <summary>
        /// 4) Login change
        /// (The login can be changed either by the Administrator or by the user personally, if it is active (no RevokedOn), the login must remain unique)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut("changelogin/{login}")]
        public async Task<ActionResult> UpdateUserLogin(string login, string password, UpdateUserLoginDto userDto, string userLogin)
        {
            User user;

            if (userLogin == login)
            {
                user = await repository.GetActiveUserAsync(userLogin);
                if (user != null && user.Password != password)
                    return BadRequest("Wrong password");
            }
            else
            {
                User admin = await repository.GetAdminAsync(login, password);
                if (admin == null)
                    return NotFound("Executor not found");

                user = await repository.GetUserAsync(userLogin);
            }

            if (user == null)
                return NotFound("User not found");

            var updatedUser = user with
            {
                Login = userDto.Login,
                ModifiedBy = login,
                ModifiedOn = DateTime.Now
            };

            if (await repository.GetActiveUserAsync(userDto.Login) != null)
                return BadRequest("User with this login is already exist");

            await repository.UpdateUserAsync(updatedUser);

            return Ok(updatedUser.ToDto());
        }
        /// <summary>
        /// 5) Request a list of all active (no RevokedOn) users, the list is sorted by CreatedOn
        /// (Available to Admins)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(string login, string password)
        {
            User admin = await repository.GetAdminAsync(login, password);

            if (admin == null)
                return NotFound("Admin is not found");

            var users = (await repository.GetUsersAsync())
                            .Where(activeUser => activeUser.RevokedBy == null)
                            .OrderByDescending(created => created.CreatedOn)
                            .Select(user => user.ToDto());
            return Ok(users);
        }
        /// <summary>
        /// 6) Request user by login, the list should contain name, gender and date of birth status active or not
        /// (Available to Admins)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpGet("userinfo/{login}")]
        public async Task<ActionResult<UserInfoDto>> GetUser(string login, string password, string userLogin)
        {
            User admin = await repository.GetAdminAsync(login, password);

            if (admin == null)
                return NotFound("Admin is not found");

            var user = await repository.GetUserAsync(userLogin);

            if (user == null)
                return NotFound();

            return Ok(user.ToInfoDto());
        }
        /// <summary>
        /// 7) User request by login and password
        /// (Available only to the user himself if he is active(no RevokedOn))
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet("aboutme/{login}")]
        public async Task<ActionResult<UserDto>> GetUserHimself(string login, string password)
        {
            var user = await repository.GetActiveUserAsync(login);

            if (user == null)
                return NotFound("User not found");

            if (user.Password != password)
                return BadRequest("Wrong password");

            return Ok(user.ToDto());
        }
        /// <summary>
        /// 8) Request all users over a certain age
        /// (Available to Admins)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        [HttpGet("olderthan")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersOlderThan(string login, string password, [BindRequired, Range(1, 130)] int age)
        {
            User admin = await repository.GetAdminAsync(login, password);

            if (admin == null)
                return NotFound("Admin is not found");

            var birthday = new DateTime(year: (DateTime.Today.Year - age), month: DateTime.Today.Month, day: DateTime.Today.Day);

            var users = (await repository.GetUsersAsync())
                            .Where(userOlder => userOlder.Birthday < birthday)
                            .Select(user => user.ToDto());
            return Ok(users);
        }
        /// <summary>
        /// 9) Deleting a user by login is complete or soft (With soft deletion, RevokedOn and RevokedBy should be installed)
        /// (Available to Admins)
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="isRevokable"></param>
        /// <returns></returns>
        [HttpDelete("{login}")]
        public async Task<ActionResult> DeleteUser(string login, string password, string userLogin, [BindRequired] bool isRevokable)
        {
            User admin = await repository.GetAdminAsync(login, password);

            if (admin == null)
                return NotFound("Admin is not found");

            User user = await repository.GetUserAsync(userLogin);

            if (user == null)
                return NotFound();

            if (isRevokable)
            {
                var updatedUser = user with
                {
                    RevokedOn = DateTime.Now,
                    RevokedBy = login
                };

                await repository.UpdateUserAsync(updatedUser);

                return Ok(updatedUser.ToDto());
            }
            await repository.DeleteUserAsync(userLogin);

            return NoContent();
        }
        /// <summary>
        /// 10) User Recovery - Clearing fields (RevokedOn, RevokedBy)
        /// (Available to Admins)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPut("revokeuser/{login}")]
        public async Task<ActionResult> RevokeUser(string login, string password, string userLogin)
        {
            User admin = await repository.GetAdminAsync(login, password);

            if (admin == null)
                return NotFound("Admin is not found");

            User user = await repository.GetUserAsync(userLogin);

            if (user == null)
                return NotFound();
            if (user.RevokedBy is null)
                return BadRequest("User is active");

            var updatedUser = user with
            {
                RevokedOn = default,
                RevokedBy = null
            };
            
            await repository.UpdateUserAsync(updatedUser);

            return Ok(updatedUser.ToDto());
        }
    }
}
