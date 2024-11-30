﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        
        claims.AddRange(user.UserRoles.Select(urm => 
            new Claim(ClaimTypes.Role, urm.Role.ToString())));

        // claims.AddRange(user.Roles.Select(role =>
        //     new Claim(ClaimTypes.Role, Enum.GetName(typeof(UserRole), role)!.ToUpperInvariant())));

        Console.WriteLine("Token was created with claims: " + claims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("JwtSettings:Token").Value!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var header = new JwtHeader(credentials);

        var payload = new JwtPayload("OrderPickingSystem", "http://localhost:5076",
            claims,
            null, DateTime.Today.AddDays(7));

        var token = new JwtSecurityToken(header, payload);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public JwtSecurityToken Verify(string jwt)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtSettings:Token").Value!);

        tokenHandler.ValidateToken(jwt, new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false
        }, out var validatedToken);

        return (JwtSecurityToken)validatedToken;
    }

    public int GetUserIdFromClaims(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            throw new ArgumentException("userIdClaim == null.");

        if (!int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new ArgumentException("!int.TryParse(userIdClaim.Value, out var userId)");
        }

        return userId;
    }
}