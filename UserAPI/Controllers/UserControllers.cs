using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using UserAPI;
using UserAPI.Dtos;
using Users;
//��������� �� ������� �������� ����� ������� � ��������� �����, �� � �� ������ ����� ��
namespace UserControllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository repository;
        //private readonly MongoRepository database;
        public UserController(IUserRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 1) �������� ������������ �� ������, ������, �����, ���� � ���� �������� + �������� ����� �� 
        /// ������������ ������� (�������� �������)
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<UserDto> CreateUser(string login, string password, CreateUserDto userDto)
        {
            User admin = repository.GetAdmin(login, password);

            if (admin == null)
                return NotFound("Admin is not found");

            if (repository.GetActiveUser(userDto.Login) != null)
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

            repository.CreateUser(user);

            return Ok(user.ToDto());
        }
        /// <summary>
        /// 2) ��������� �����, ���� ��� ���� �������� ������������
        /// (����� ������ �������������, ���� ����� ������������, ���� �� �������(����������� RevokedOn))
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut("changeinfo/{login}")]
        public ActionResult UpdateUserInfo(string login, string password, UpdateUserInfoDto userDto, string userLogin)
        {
            User user;

            if (userLogin == login)
            {
                user = repository.GetActiveUser(userLogin);
                if (user != null && user.Password != password)
                    return BadRequest("Wrong password");
            }
            else
            {
                User admin = repository.GetAdmin(login, password);
                if (admin == null)
                    return NotFound("Executor not found");

                user = repository.GetUser(userLogin);
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

            repository.UpdateUser(updatedUser);

            return Ok(updatedUser.ToDto());
        }
        /// <summary>
        /// 3) ��������� ������ 
        /// (������ ����� ������ ���� �������������, ���� ����� ������������, ������ �������(����������� RevokedOn))
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut("changepassword/{login}")]
        public ActionResult UpdateUserPassword(string login, string password, UpdateUserPasswordDto userDto, string userLogin)
        {
            User user;

            if (userLogin == login)
            {
                user = repository.GetActiveUser(userLogin);
                if (user != null && user.Password != password)
                    return BadRequest("Wrong password");
            }
            else
            {
                User admin = repository.GetAdmin(login, password);
                if (admin == null)
                    return NotFound("Executor not found");

                user = repository.GetUser(userLogin);
            }

            if (user == null)
                return NotFound("User not found");

            var updatedUser = user with
            {
                Password = userDto.Password,
                ModifiedBy = userLogin,
                ModifiedOn = DateTime.Now
            };

            repository.UpdateUser(updatedUser);

            return Ok(updatedUser.ToDto());
        }
        /// <summary>
        /// 4) ��������� ������
        /// (����� ����� ������ ���� �������������, ���� ����� ������������, ���� �� ������� (����������� RevokedOn), ����� ������ ���������� ����������)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut("changelogin/{login}")]
        public ActionResult UpdateUserLogin(string login, string password, UpdateUserLoginDto userDto, string userLogin)
        {
            User user;

            if (userLogin == login)
            {
                user = repository.GetActiveUser(userLogin);
                if (user != null && user.Password != password)
                    return BadRequest("Wrong password");
            }
            else
            {
                User admin = repository.GetAdmin(login, password);
                if (admin == null)
                    return NotFound("Executor not found");

                user = repository.GetUser(userLogin);
            }

            if (user == null)
                return NotFound("User not found");

            var updatedUser = user with
            {
                Login = userDto.Login,
                ModifiedBy = login,
                ModifiedOn = DateTime.Now
            };

            if (repository.GetActiveUser(userDto.Login) != null)
                return BadRequest("User with this login is already exist");

            repository.UpdateUser(updatedUser);

            return Ok(updatedUser.ToDto());
        }
        /// <summary>
        /// 5) ������ ������ ���� �������� (����������� RevokedOn) �������������, ������ ������������ �� CreatedOn
        /// (�������� �������)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetUsers(string login, string password)
        {
            User admin = repository.GetAdmin(login, password);

            if (admin == null)
                return NotFound("Admin is not found");

            var users = repository.GetUsers()
                            .Where(activeUser => activeUser.RevokedBy == null)
                            .OrderByDescending(created => created.CreatedOn)
                            .Select(user => user.ToDto());
            return Ok(users);
        }
        /// <summary>
        /// 6) ������ ������������ �� ������, � ������ ����� ���� ���, ��� � ���� �������� ������ �������� ��� ���
        /// (�������� �������)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpGet("userinfo/{login}")]
        public ActionResult<UserInfoDto> GetUser(string login, string password, string userLogin)
        {
            User admin = repository.GetAdmin(login, password);

            if (admin == null)
                return NotFound("Admin is not found");

            var user = repository.GetUser(userLogin);

            if (user == null)
                return NotFound();

            return Ok(user.ToInfoDto());
        }
        /// <summary>
        /// 7) ������ ������������ �� ������ � ������ 
        /// (�������� ������ ������ ������������, ���� �� �������(����������� RevokedOn))
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet("aboutme/{login}")]
        public ActionResult<UserDto> GetUserHimself(string login, string password)
        {
            var user = repository.GetActiveUser(login);

            if (user == null)
                return NotFound("User not found");

            if (user.Password != password)
                return BadRequest("Wrong password");

            return Ok(user.ToDto());
        }
        /// <summary>
        /// 8) ������ ���� ������������� ������ ������������ �������� 
        /// (�������� �������)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        [HttpGet("olderthan")]
        public ActionResult<IEnumerable<UserDto>> GetUsersOlderThan(string login, string password, [BindRequired, Range(1, 130)] int age)
        {
            User admin = repository.GetAdmin(login, password);

            if (admin == null)
                return NotFound("Admin is not found");

            var birthday = new DateTime(year: (DateTime.Today.Year - age), month: DateTime.Today.Month, day: DateTime.Today.Day);

            var users = repository.GetUsers()
                            .Where(activeUser => activeUser.RevokedBy == null)
                            .Where(userOlder => userOlder.Birthday < birthday)
                            .Select(user => user.ToDto());
            return Ok(users);
        }
        /// <summary>
        /// 9) �������� ������������ �� ������ ������ ��� ������ (��� ������ �������� ������ ����������� ����������� RevokedOn � RevokedBy) 
        /// (�������� �������)
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="isRevokable"></param>
        /// <returns></returns>
        [HttpDelete("{login}")]
        public ActionResult DeleteUser(string login, string password, string userLogin, [BindRequired] bool isRevokable)
        {
            User admin = repository.GetAdmin(login, password);

            if (admin == null)
                return NotFound("Admin is not found");

            User user = repository.GetUser(userLogin);

            if (user == null)
                return NotFound();

            if (isRevokable)
            {
                var updatedUser = user with
                {
                    RevokedOn = DateTime.Now,
                    RevokedBy = login
                };

                repository.UpdateUser(updatedUser);

                return Ok(updatedUser.ToDto());
            }
            repository.DeleteUser(userLogin);

            return NoContent();
        }
        /// <summary>
        /// 10) �������������� ������������ - ������� ����� (RevokedOn, RevokedBy) 
        /// (�������� �������)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPut("revokeuser/{login}")]
        public ActionResult RevokeUser(string login, string password, string userLogin)
        {
            User admin = repository.GetAdmin(login, password);

            if (admin == null)
                return NotFound("Admin is not found");

            User user = repository.GetUser(userLogin);

            if (user == null)
                return NotFound();
            if (user.RevokedBy is null)
                return BadRequest("User is active");

            var updatedUser = user with
            {
                RevokedOn = default(DateTime),
                RevokedBy = null
            };

            repository.UpdateUser(updatedUser);

            return Ok(updatedUser.ToDto());
        }
    }
}
