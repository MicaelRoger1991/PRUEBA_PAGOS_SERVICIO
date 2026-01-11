using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using EsApp.Application.Auth.Request;
using EsApp.Application.Auth.Response;
using EsApp.Authentication.JWT;
using EsApp.CROSS.Encrypt;
using EsApp.CROSS.Shared.Abstractions;
using EsApp.Domain.Auth;
using EsApp.Domain.Token;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace EsApp.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly ISecurityEncrypt _securityEncrypt;
    private readonly IManagerAuthentication _managerAuthentication;
    private readonly int _timeSession;
    private readonly string _sectionTimeSession = "timeSession";

    public AuthService(IAuthRepository authRepository, ITokenRepository tokenRepository, IManagerAuthentication managerAuthentication, ISecurityEncrypt securityEncrypt, IConfiguration configuration)
    {
        _authRepository = authRepository;
        _tokenRepository = tokenRepository;
        _managerAuthentication = managerAuthentication;
        _securityEncrypt = securityEncrypt;

        _timeSession = int.Parse(configuration.GetSection(_sectionTimeSession).Value!);
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, IValidator<LoginRequest> validator, CancellationToken cancellationToken)
    {

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var loginResult = await _authRepository.GetLoginByUserAndPasswordAsync(request.user, request.password);
        if (loginResult == null)
        {
            return Result.Failure<LoginResponse>(AuthError.NotFound);
        }

        var resultToken = _managerAuthentication.getToken(new AuthenticationRequest(
            user: loginResult.usersMasterId.ToString(),
            pass: string.Empty,
            role: loginResult.role,
            name: loginResult.lastName + " " + loginResult.firstName
        ));

        if (resultToken == null)
        {
            return Result.Failure<LoginResponse>(AuthError.NotToken);
        }


        if (await _tokenRepository.addTokenByIdUser(UserSession.SaveSession(
            usersMasterId: loginResult.usersMasterId,
            token: resultToken.token,
            refreshToken: resultToken.refreshToken,
            description: "LOGIN",
            dateGenerate: DateTime.Now,
            dateExpiration: DateTime.Now.AddMinutes(_timeSession * 2)
        )))
        {
            return new LoginResponse(
                token: resultToken.token,
                refreshToken: resultToken.refreshToken
            );
        }
        else
        {
            return Result.Failure<LoginResponse>(AuthError.NotToken);
        }
    }

    public async Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request, IValidator<RefreshTokenRequest> validator, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        if (!_managerAuthentication.getValidateTokenFromRefresh(request.Token))
        {
            return Result.Failure<LoginResponse>(TokenError.CurrentSession);
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenExpiradoSupuestamente = tokenHandler.ReadJwtToken(request.Token);

        string userValue = tokenExpiradoSupuestamente.Claims.First(x =>
                    x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();
        string roleJson = tokenExpiradoSupuestamente.Claims.First(x =>
            x.Type == "role").Value.ToString();
        string name = tokenExpiradoSupuestamente.Claims.First(x =>
            x.Type == JwtRegisteredClaimNames.UniqueName).Value.ToString();


        var resultSession = await _tokenRepository.GetSessionByTokenAsync(
            request.Token,
            request.RefreshToken,
            userValue);

        if (resultSession == null)
        {
            return Result.Failure<LoginResponse>(TokenError.NotFound);
        }
        else if (resultSession.dateExpiration < DateTime.Now)
        {
            return Result.Failure<LoginResponse>(TokenError.Expired);
        }
        else if (resultSession.dateGenerate <= DateTime.Now
            && DateTime.Now <= resultSession.dateExpiration)
        {
            var requestToken = new AuthenticationToken(
               token: request.Token,
               refreshToken: request.RefreshToken
           );
            var requestLogin = new AuthenticationRequest(
                user: userValue,
                pass: string.Empty,
                role: roleJson,
                name: name
            );
            var resultToken = _managerAuthentication.getRefreshToken(requestToken, requestLogin);
            if (await _tokenRepository.updateTokenByIdUser(UserSession.GetSession(
                    usersSessionsMasterId: resultSession.usersSessionsMasterId,
                    usersMasterId: resultSession.usersMasterId,
                    token: resultToken.token,
                    refreshToken: resultToken.refreshToken,
                    description: "GENERATE REFRESH TOKEN",
                    dateGenerate: DateTime.Now,
                    dateExpiration: DateTime.Now.AddMinutes(_timeSession * 2),
                    amountToken: resultSession.amountToken + 1
                )))
            {
                return new LoginResponse(
                    token: resultToken.token,
                    refreshToken: resultToken.refreshToken
                );
            }
            else
            {
                return Result.Failure<LoginResponse>(TokenError.NotUpdated);
            }
        }
        else
        {
            return Result.Failure<LoginResponse>(TokenError.Expired);
        }
    }
}
