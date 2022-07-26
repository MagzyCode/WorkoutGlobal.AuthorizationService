﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WorkoutGlobal.AuthorizationServiceApi.Contracts;
using WorkoutGlobal.AuthorizationServiceApi.DbContext;
using WorkoutGlobal.AuthorizationServiceApi.Models;
using WorkoutGlobal.AuthorizationServiceApi.Models.Dto;

namespace WorkoutGlobal.AuthorizationServiceApi.Repositories
{
    /// <summary>
    /// Represents authentication repository.
    /// </summary>
    public class AuthenticationRepository : BaseRepository<UserCredential>, IAuthenticationRepository
    {
        private UserManager<UserCredential> _userManager;
        private IMapper _mapper;

        /// <summary>
        /// Ctor for authentication repository.
        /// </summary>
        /// <param name="userManager">User manager class instance.</param>
        /// <param name="autorizationServiceContext">Project database context instance.</param>
        /// <param name="configuration">Project configuration instance.</param>
        /// <param name="mapper">AutoMapper instance.</param>
        public AuthenticationRepository(
            UserManager<UserCredential> userManager,
            AutorizationServiceContext autorizationServiceContext,
            IConfiguration configuration,
            IMapper mapper)
            : base(autorizationServiceContext, configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Represents user manager instanse for wotking with EF Identity.
        /// </summary>
        public UserManager<UserCredential> UserManager
        {
            get => _userManager;
            private set => _userManager = value ?? throw new ArgumentNullException(nameof(_userManager), "User manager cannot be null.");
        }

        /// <summary>
        /// Represent AutoMapper instanse.
        /// </summary>
        public IMapper Mapper 
        { 
            get => _mapper;
            set => _mapper = value ?? throw new ArgumentNullException(nameof(_mapper), "AutoMapper instance cannot be null.");
        }

        /// <summary>
        /// Create JWT-token for user.
        /// </summary>
        /// <param name="userAuthorizationDto">User credential.</param>
        /// <returns>JWT-token in string format.</returns>
        /// <exception cref="ArgumentNullException">Throws if argument is null.</exception>
        public string CreateToken(UserAuthorizationDto userAuthorizationDto)
        {
            if (userAuthorizationDto is null)
                throw new ArgumentNullException(nameof(userAuthorizationDto));

            var tokenOptions = GenerateTokenOptions(userAuthorizationDto.UserName);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.CanWriteToken
                ? jwtSecurityTokenHandler.WriteToken(tokenOptions)
                : string.Empty;

            return token;
        }

        /// <summary>
        /// Generation of password hash base on password and secret salt.
        /// </summary>
        /// <param name="password">User password.</param>
        /// <param name="salt">Secret key.</param>
        /// <returns>Returns hashed password.</returns>
        public async Task<string> GenerateHashPasswordAsync(string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(salt))
                throw new ArgumentNullException(nameof(salt), "Salt cannot be null or empty");

            using var sha256 = SHA256.Create();
            var hashedBytes = await sha256.ComputeHashAsync(
                inputStream: new MemoryStream(Encoding.UTF8.GetBytes(password + salt)));

            var hashPassword = ConvertByteArrayToString(hashedBytes);

            return hashPassword;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultRegistrationInfoDto"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<UserCredential> GenerateUserCredentialsAsync(DefaultRegistrationInfoDto defaultRegistrationInfoDto, string password)
        {
            if (defaultRegistrationInfoDto is null)
                throw new ArgumentNullException(nameof(defaultRegistrationInfoDto), "Default registration info cannot be null.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password), "User password cannot be null.");

            var userCredential = _mapper.Map<UserCredential>(defaultRegistrationInfoDto);

            var saltBytes = RandomNumberGenerator.GetBytes(8);
            var passwordSalt = ConvertByteArrayToString(saltBytes);

            userCredential.PasswordSalt = passwordSalt;
            userCredential.PasswordHash = await GenerateHashPasswordAsync(password, passwordSalt);

            return userCredential;
        }

        /// <summary>
        /// Checks is registration use credentials already exists in system.
        /// </summary>
        /// <param name="userRegistrationDto">User registration credentials.</param>
        /// <returns>If user existed in system, return true, otherwise return false.</returns>
        public bool IsUserExisted(UserRegistrationDto userRegistrationDto)
        {
            var existedUser = FindUserByCredentials(userRegistrationDto.UserName);

            return existedUser is not null;
        }

        /// <summary>
        /// Registrate user in system.
        /// </summary>
        /// <param name="userRegistrationDto">Registration user credentials.</param>
        /// <returns>Returns generated user credential id.</returns>
        /// <exception cref="ArgumentNullException">Throws if incoming DTO model is null.</exception>
        public async Task<string> RegistrateUserAsync(UserRegistrationDto userRegistrationDto)
        {
            if (userRegistrationDto is null)
                throw new ArgumentNullException(nameof(userRegistrationDto), "Incoming DTO model cannot be null.");

            var defaultRegistrationInfoDto = _mapper.Map<DefaultRegistrationInfoDto>(userRegistrationDto);

            var userCredential = await GenerateUserCredentialsAsync(defaultRegistrationInfoDto, userRegistrationDto.Password);
            var userAccount = _mapper.Map<UserAccount>(userRegistrationDto);

            userCredential.Id = Guid.NewGuid().ToString();
            await _userManager.CreateAsync(userCredential);

            await _userManager.AddToRoleAsync(userCredential, "User");

            userAccount.UserCredentialsId = userCredential.Id;
            await Context.UserAccounts.AddAsync(userAccount);
            await Context.SaveChangesAsync();

            return userCredential.Id;
        }

        /// <summary>
        /// Checks the user's data when log in.
        /// </summary>
        /// <param name="userAuthorizationDto">User credentials.</param>
        /// <returns>If user credentials exists in system, return true,
        /// otherwise return false.</returns>
        public async Task<bool> ValidateUserAsync(UserAuthorizationDto userAuthorizationDto)
        {
            if (userAuthorizationDto is null)
                return false;

            var userCredentials = FindUserByCredentials(userAuthorizationDto.UserName);

            if (userCredentials is null)
                return false;

            var userPasswordHash = await GenerateHashPasswordAsync(userAuthorizationDto.Password, userCredentials.PasswordSalt);
            
            return userCredentials is not null && userCredentials.PasswordHash == userPasswordHash;
        }

        /// <summary>
        /// Generate token options.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>Returns JwtSecurityToken instanse.</returns>
        private JwtSecurityToken GenerateTokenOptions(string userName)
        {
            var jwtSettings = Configuration.GetSection("JwtSettings");

            var signingCredentials = new SigningCredentials(
                key: new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.GetSection("Key").Value)),
                algorithm: SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName)
            };

            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings.GetSection("ValidIssuer").Value,
                audience: jwtSettings.GetSection("ValidAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("Expires").Value)),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }

        /// <summary>
        /// Convert byte array to string in lower case and without "-".
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private static string ConvertByteArrayToString(byte[] array) => BitConverter.ToString(array).ToLower().Replace("-", "");

        /// <summary>
        /// Find user by his credentials.
        /// </summary>
        /// <param name="username">User name in system.</param>
        /// <returns>Existed user.</returns>
        private UserCredential FindUserByCredentials(string username)
        {
            // cannot be users with single username
            var userCredentials = Context.Users
                .Where(user => user.UserName == username)
                .SingleOrDefault();

            return userCredentials;
        }
    }
}
