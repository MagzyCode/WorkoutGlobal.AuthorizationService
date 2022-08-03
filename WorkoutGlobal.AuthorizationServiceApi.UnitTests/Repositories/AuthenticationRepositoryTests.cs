using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Tasks;
using WorkoutGlobal.AuthorizationServiceApi.Models;
using WorkoutGlobal.AuthorizationServiceApi.Dtos;
using WorkoutGlobal.AuthorizationServiceApi.Repositories;
using WorkoutGlobal.AuthorizationServiceApi.UnitTests.Configuration;
using Xunit;

namespace WorkoutGlobal.AuthorizationServiceApi.UnitTests.Repositories
{
    public class AuthenticationRepositoryTests
    {
        private readonly AuthenticationRepository _authenticationRepository;
        private readonly IConfiguration _testConfiguration;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IConfigurationSection> _mockJwtSettings;
        private readonly Fixture _fixture;

        public AuthenticationRepositoryTests()
        {
            _fixture = new();
            _testConfiguration = ConfigurationAccessor.GetTestConfiguration();

            _mockJwtSettings = new();
            _mockJwtSettings
                .Setup(x => x.GetSection("ValidIssuer").Value)
                .Returns(_testConfiguration["Issuer"]);
            _mockJwtSettings
                .Setup(x => x.GetSection("ValidAudience").Value)
                .Returns(_testConfiguration["Audience"]);

            _mockConfiguration = new();
            _mockConfiguration
                .Setup(configuration => configuration.GetSection("JwtSettings"))
                .Returns(_mockJwtSettings.Object);

            _mockMapper = new();
            _mockMapper
                .Setup(x => x.Map<UserCredential>(It.IsAny<UserRegistrationDto>()))
                .Returns(new UserCredential());
            _mockMapper
                .Setup(x => x.Map<UserCredential>(It.IsAny<DefaultRegistrationInfoDto>()))
                .Returns(new UserCredential());

            _authenticationRepository = new(
                userManager: null,
                autorizationServiceContext: null,
                configuration: _mockConfiguration.Object,
                mapper: _mockMapper.Object);
        }

        [Fact]
        public void CreateToken_NullArgument_ReturnArgumentNullException()
        {
            // arrange
            UserAuthorizationDto? userAuthorizationDto = null;

            // act
            var result = () => _authenticationRepository.CreateToken(userAuthorizationDto);

            // assert
            result.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateToken_ValidUserCredentials_ReturnValidJWTToken()
        {
            // arrange
            var userAuthorizationDto = _fixture.Create<UserAuthorizationDto>();

            _mockJwtSettings
                .Setup(x => x.GetSection("Key").Value)
                .Returns(_testConfiguration["Key"]);
            _mockJwtSettings
                .Setup(x => x.GetSection("Expires").Value)
                .Returns(_testConfiguration["Expires"]);

            // act
            var result = _authenticationRepository.CreateToken(userAuthorizationDto);

            // assert
            result.Should().BeOfType<string>();
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().MatchEquivalentOf("*.*.*");
        }

        [Fact]
        public void CreateToken_InvalidKeyLength_ReturnArgumentOutOfRangeException()
        {
            // arrange
            var userAuthorizationDto = _fixture.Create<UserAuthorizationDto>();

            _mockJwtSettings
                .Setup(x => x.GetSection("Key").Value)
                .Returns(_testConfiguration["InvalidKey"]);
            _mockJwtSettings
                .Setup(x => x.GetSection("Expires").Value)
                .Returns(_testConfiguration["Expires"]);

            // act
            var result = () => _authenticationRepository.CreateToken(userAuthorizationDto);

            // assert
            result.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void CreateToken_InvalidUserName_ReturnArgumentNullException()
        {
            // arrange
            var userAuthorizationDto = new UserAuthorizationDto();

            _mockJwtSettings
                .Setup(x => x.GetSection("Key").Value)
                .Returns(_testConfiguration["Key"]);

            // act
            var result = () => _authenticationRepository.CreateToken(userAuthorizationDto);

            // assert
            result.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateToken_InvalidExpires_ReturnFormatException()
        {
            // arrange
            var userAuthorizationDto = _fixture.Create<UserAuthorizationDto>();

            _mockJwtSettings
                .Setup(x => x.GetSection("Key").Value)
                .Returns(_testConfiguration["Key"]);
            _mockJwtSettings
                .Setup(x => x.GetSection("Expires").Value)
                .Returns(_testConfiguration["InvalidExpires"]);

            // act
            var result = () => _authenticationRepository.CreateToken(userAuthorizationDto);

            // assert
            result.Should().Throw<FormatException>();
        }

        [Fact]
        public void CreateToken_InvalidPassword_ReturnJWTToken()
        {
            // arrange

            var userAuthorizationDto = _fixture.Build<UserAuthorizationDto>()
                .OmitAutoProperties()
                .With(user => user.UserName)
                .Create();

            _mockJwtSettings
                .Setup(x => x.GetSection("Key").Value)
                .Returns(_testConfiguration["Key"]);
            _mockJwtSettings
                .Setup(x => x.GetSection("Expires").Value)
                .Returns(_testConfiguration["Expires"]);

            // act
            var result = _authenticationRepository.CreateToken(userAuthorizationDto);

            // assert
            result.Should().BeOfType<string>();
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().MatchEquivalentOf("*.*.*");
        }

        [Fact]
        public async Task GenerateUserCredentialsAsync_NullDefaultRegistrationInfoDto_ReturnArgumentNullException()
        {
            // arrange
            var password = _fixture.Create<string>();

            // act
            var result = async () => await _authenticationRepository.GenerateUserCredentialsAsync(null, password);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GenerateUserCredentialsAsync_NullPassword_ReturnArgumentNullException()
        {
            // arrange
            var defaultRegistrationInfoDto = _fixture.Create<DefaultRegistrationInfoDto>();

            // act
            var result = async () => await _authenticationRepository.GenerateUserCredentialsAsync(defaultRegistrationInfoDto, null);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GenerateUserCredentialsAsync_ValidParams_ReturnUserCredential()
        {
            // arrange
            var password = _fixture.Create<string>();
            var defaultRegistrationInfoDto = _fixture.Create<DefaultRegistrationInfoDto>();

            // act
            var result = await _authenticationRepository.GenerateUserCredentialsAsync(defaultRegistrationInfoDto, password);

            // assert
            result.Should().BeOfType<UserCredential>();
            result.Should().NotBeNull();
            result.PasswordHash.Should().NotBeNull();
        }

        [Fact]
        public void IsUserExisted_NullUserRegistration_ReturnArgumentNullException()
        {
            // arrange
            UserRegistrationDto? userRegistrationDto = null;

            // act
            var result = () => _authenticationRepository.IsUserExisted(userRegistrationDto);

            // assert
            result.Should().Throw<ArgumentNullException>();
        }
    }
}
