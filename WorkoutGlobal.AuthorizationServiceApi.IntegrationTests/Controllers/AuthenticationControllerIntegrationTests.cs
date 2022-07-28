using AutoFixture;
using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WorkoutGlobal.AuthorizationServiceApi.Dtos;
using Xunit;

namespace WorkoutGlobal.AuthorizationServiceApi.IntegrationTests.Controllers
{
    public class AuthenticationControllerIntegrationTests : IAsyncLifetime
    {
        private readonly AppTestConnection<string> _appTestConnection;
        private readonly Fixture _fixture = new();
        private UserRegistrationDto _registrationModel;

        public AuthenticationControllerIntegrationTests()
        {
            _appTestConnection = new();
        }

        public async Task InitializeAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                _appTestConnection.PurgeList.Clear();

                _registrationModel = _fixture
                    .Build<UserRegistrationDto>()
                    .With(user => user.UserName, "MagzyCodeTest")
                    .With(user => user.Email, "magzycode@mail.ru")
                    .With(user => user.Password, "qwerty123")
                    .With(user => user.Height, 186)
                    .With(user => user.Weight, 120)
                    .Create();
            });
        }

        public async Task DisposeAsync()
        {
            foreach (var id in _appTestConnection.PurgeList)
                _ = await _appTestConnection.AppClient.DeleteAsync($"api/authentication/purge/{id}");

            await Task.CompletedTask;
        }

        [Fact]
        public async Task Registrate_ValidUser_ReturnCreatedResult()
        {
            // arrange
            var regex = @"[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}";

            // act
            var registrationResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/authentication/registration", _registrationModel);
            var createdUserCredentialId = await registrationResponse.Content.ReadFromJsonAsync<string>();
            var userCredentialsResponse = await _appTestConnection.AppClient.GetAsync($"api/userCredentials/{createdUserCredentialId}");
            var userCredential = await userCredentialsResponse.Content.ReadFromJsonAsync<UserCredentialDto>();
            var roleResponse = await _appTestConnection.AppClient.GetAsync($"api/userCredentials/{createdUserCredentialId}/roles");
            var roles = await roleResponse.Content.ReadFromJsonAsync<List<string>>();
            var userCredentialAccountResponse = await _appTestConnection.AppClient.GetAsync($"api/userCredentials/{createdUserCredentialId}/account");
            var userCredentialAccount = await userCredentialAccountResponse.Content.ReadFromJsonAsync<UserAccountDto>();
            
            _appTestConnection.PurgeList.Add(createdUserCredentialId);

            // assert
            registrationResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            createdUserCredentialId.Should().MatchRegex(regex);

            userCredentialsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            userCredential.Should().NotBeNull();
            userCredential.Should().BeOfType<UserCredentialDto>();

            roleResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            roles.Count.Should().Be(1);
            roles.Should().Contain("User");

            userCredentialAccountResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            userCredentialAccount.Should().BeOfType<UserAccountDto>();
            userCredentialAccount.Should().NotBeNull();

            userCredentialAccount.UserCredentialsId.Should().Be(createdUserCredentialId);
        }

        [Fact]
        public async Task RegistrateAndAuthorize_ValidUser_ReturnToken()
        {
            // arrange
            var regex = "*.*.*";

            // act
            var registrationResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/authentication/registration", _registrationModel);
            var createdUserCredentialsId = await registrationResponse.Content.ReadFromJsonAsync<string>();

            var authorizationResponce = await _appTestConnection.AppClient.PostAsJsonAsync("api/authentication/login", new UserAuthorizationDto()
            {
                Password = _registrationModel.Password,
                UserName = _registrationModel.UserName
            });
            var token = await authorizationResponce.Content.ReadAsStringAsync();

            _appTestConnection.PurgeList.Add(createdUserCredentialsId);

            // assert
            authorizationResponce.StatusCode.Should().Be(HttpStatusCode.OK);

            token.Should().NotBeEmpty();
            token.Should().MatchEquivalentOf(regex);
        }
    }
}
